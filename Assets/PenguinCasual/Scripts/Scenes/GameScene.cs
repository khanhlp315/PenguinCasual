using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Penguin.Ads;
using Penguin.Sound;
using Penguin.Utilities;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Penguin
{
    /// <summary>
    /// Controller to manage whole game scene
    /// </summary>
    public class GameScene : MonoBehaviour
    {
        [Header("-------------Game Setting------------")]
        [SerializeField]
        private SkinSetting _skinSetting;
        [SerializeField]
        private GameSetting _gameSetting;

        [Header("------------Character---------------")]
        [SerializeField]
        private Transform _camera;

        [Header("------------Character---------------")]
        [SerializeField]
        private Character _mainCharacter;

        [Header("-----------Backgrounds-------------")]
        [SerializeField]
        private Vector3 _backgroundPosition;

        [SerializeField]
        private GameObject _background;

        [Header("-----------UI--------------")]
        [SerializeField]
        private GameObject _hideGroup;
        [SerializeField]
        private ShadowTextUGUI _labelScore;
        [SerializeField]
        private TextMeshProUGUI _labelCountdown;
        //[SerializeField]
        //private TextMeshProUGUI _labelWarning;
        [SerializeField]
        private GameObject _readyImage;
        [SerializeField]
        private GameObject _goImage;
        [SerializeField]
        private GameObject _timeupImage;
        [SerializeField]
        private Image _countdownFillRing;
        [SerializeField]
        private List<FishEscapeEffect> _fishEscapeEffectPrefabs;
        [SerializeField]
        private DestroyPedestalLayerEffect _destroyPedestalLayerPrefab;

        [Header("-------Timeout----------")]
        [SerializeField]
        private Image _timeOutEffect;
        [SerializeField]
        private Transform _clockScaleAnchor;

        [Header("-----------UI Menu-----------")]
        [SerializeField]
        private GameObject _gamePanel;
        [SerializeField]
        private EndGamePanel _endGamePanel;
        [SerializeField]
        private ParticleSystem _endGameParticle;

        [SerializeField]
        private TextMeshProUGUI _labelScoreIncrease;

        private CoreGameModel _coreGameModel;

        private GenericGOPool _effectPool = new GenericGOPool();

        private long _currentScore;

        private bool _isTimeoutEffectActive;

        private Sequence _timeoutSeq;

        private void Start()
        {
            Advertiser.AdvertisementSystem.HideNormalBanner();
            Advertiser.AdvertisementSystem.HideEndGameBanner();
            _coreGameModel = MemCached.Get<CoreGameModel>(typeof(CoreGameModel).ToString(), true);
            if (_coreGameModel != null)
            {
                if (_mainCharacter != null)
                {
                    var skinData = GetSkinById(_coreGameModel.characterId);
                    if (skinData != null)
                    {
                        _mainCharacter.SetModel(skinData.prefabModel);
                    }
                }

                if (_background != null)
                {
                    var skinData = GetSkinById(_coreGameModel.backgroundId);
                    if (skinData != null)
                    {
                        if (_background != null)
                        {
                            GameObject.Destroy(_background);
                        }

                        _background = GameObject.Instantiate(skinData.prefabModel);
                        _background.transform.localPosition = _backgroundPosition;
                    }
                }
            }

            RegisterEvent();
            InitPool();

            _currentScore = 0;
            _timeOutEffect.gameObject.SetActive(false);
            _hideGroup.SetActive(false);
            _gamePanel.SetActive(true);
            _endGamePanel.gameObject.SetActive(false);

            StartCoroutine(WaitAndStartGame());
        }

        private void OnDestroy()
        {
            UnRegisterEvent();
        }

        private void OnGameTimeUpdate(EventGameTimeUpdate e)
        {
            _labelCountdown.text = Mathf.RoundToInt(e.time).ToString();
            _countdownFillRing.fillAmount = e.time / _gameSetting.roundDuration;

            if (_isTimeoutEffectActive)
            {
                if (e.time > 5f)
                {
                    _isTimeoutEffectActive = false;
                    ShowTimeoutEffect(false, true);
                }
            }
            else
            {
                if (e.time <= 5f)
                {
                    _isTimeoutEffectActive = true;
                    ShowTimeoutEffect(true, false);
                }
            }
        }

        private void ShowTimeoutEffect(bool isShow , bool resetScale)
        {
            if (isShow)
            {
                _timeOutEffect.gameObject.SetActive(true);
                _timeoutSeq = DOTween.Sequence();

                var color = _timeOutEffect.color;
                color.a = 0;
                _timeOutEffect.color = color;
                
                _timeoutSeq.Append(_timeOutEffect.DOFade(0.65f, 0.5f).SetEase(Ease.Linear));
                _timeoutSeq.Append(_timeOutEffect.DOFade(0, 0.2f).SetEase(Ease.Linear));
                _timeoutSeq.SetLoops(-1, LoopType.Restart);
                _timeoutSeq.Play();

                _clockScaleAnchor.DOScale(1.5f, 5f);
            }
            else
            {
                if (_timeoutSeq != null)
                {
                    _timeoutSeq.Kill();
                    _timeoutSeq = null;
                }

                _clockScaleAnchor.DOKill();
                if (resetScale)
                {
                    _clockScaleAnchor.localScale = Vector3.one;
                }
                _timeOutEffect.gameObject.SetActive(false);
            }
        }

        private void InitPool()
        {
            foreach (var prefab in _fishEscapeEffectPrefabs)
            {
                _effectPool.RegisterPooledGO(prefab);
            }

            _effectPool.RegisterPooledGO(_destroyPedestalLayerPrefab);
        }

        /// <summary>
        /// Register event for update Views
        /// </summary>
        private void RegisterEvent()
        {
            EventHub.Bind<EventUpdateScore>(OnScoreUpdate);
            EventHub.Bind<EventGameTimeUpdate>(OnGameTimeUpdate);
            EventHub.Bind<EventEndGame>(OnEndGame);
            EventHub.Bind<EventCharacterPassLayer>(ShowFishesEscapeEffect);
            EventHub.Bind<EventPedestalLayerDestroy>(ShowPedestalLayerDestroyEffect);
            EventHub.Bind<EventRevive>(OnRevive);
        }

        /// <summary>
        /// Unregister events
        /// </summary>
        private void UnRegisterEvent()
        {
            EventHub.Unbind<EventUpdateScore>(OnScoreUpdate);
            EventHub.Unbind<EventGameTimeUpdate>(OnGameTimeUpdate);
            EventHub.Unbind<EventEndGame>(OnEndGame);
            EventHub.Unbind<EventCharacterPassLayer>(ShowFishesEscapeEffect);
            EventHub.Unbind<EventPedestalLayerDestroy>(ShowPedestalLayerDestroyEffect);
            EventHub.Unbind<EventRevive>(OnRevive);
        }

        private IEnumerator WaitAndStartGame()
        {
            _readyImage.SetActive(true);
            _labelCountdown.text = _gameSetting.roundDuration.ToString();
            _labelScore.text = "0<size=80>匹</size>";

            yield return new WaitForSeconds(2);

            _readyImage.SetActive(false);
            _goImage.SetActive(true);

            yield return new WaitForSeconds(1);

            _goImage.SetActive(false);
            _hideGroup.SetActive(true);

            EventHub.Emit<EventStartGame>();
        }

        /// <summary>
        /// Receiver from event hub to update label score
        /// </summary>
        /// <param name="eventData"></param>
        private void OnScoreUpdate(EventUpdateScore eventData)
        {
            _currentScore = eventData.score;
            _labelScore.text = ScoreUtil.FormatScore(eventData.score) + "<size=14>匹</size>";

            var spawnLabel = GameObject.Instantiate(_labelScoreIncrease, _labelScoreIncrease.transform.parent);
            spawnLabel.transform.localPosition = _labelScoreIncrease.transform.localPosition;
            spawnLabel.transform.localScale = Vector3.one;

            spawnLabel.gameObject.SetActive(true);
            spawnLabel.alpha = 1f;
            spawnLabel.text = "+" + ScoreUtil.FormatScore(eventData.increase);

            var seq = DOTween.Sequence();
            seq.Append(spawnLabel.transform.DOLocalMoveY(spawnLabel.transform.position.y + 250, 1.5f));
            seq.Join(spawnLabel.DOFade(0, 1.5f));

            seq.OnComplete(() =>
            {
                if (spawnLabel != null && spawnLabel.gameObject != null)
                {
                    Destroy(spawnLabel.gameObject);
                }
            });
        }

        private void OnEndGame(EventEndGame eventData)
        {
            StartCoroutine(DelayAndShowEndGamePanel(!eventData.isCharacterDeath));
        }

        private IEnumerator DelayAndShowEndGamePanel(bool isTimeout)
        {
            bool hasWatchAd = true;
            if (isTimeout)
            {
                ShowTimeoutEffect(false, false);
            }

            yield return new WaitForSeconds(0.5f);
            float delay = 0.5f;
            if (isTimeout)
            {
                _timeupImage.SetActive(true);
                delay = 2f;
                var image = _timeupImage.GetComponent<Image>();
                var sequence = DOTween.Sequence();
                sequence.Append(image.DOColor(new Color(1,1,1,0),0.5f ));
                sequence.Append(image.DOColor(new Color(1,1,1,1),0.5f ));
                sequence.Append(image.DOColor(new Color(1,1,1,0),0.5f ));
                sequence.Append(image.DOColor(new Color(1,1,1,1),0.5f ));
            }
            else
            {
                SoundManager.PlayOneShot(SoundConfig.FishMoveEndGame);
                _endGameParticle.gameObject.SetActive(true);
                _endGameParticle.Play(true);
            }
            yield return new WaitForSeconds(delay);
            _timeupImage.gameObject.SetActive(false);

            _gamePanel.SetActive(false);
            _endGamePanel.gameObject.SetActive(true);
            _endGamePanel.SetScore(_currentScore);

            var highScore = PlayerPrefs.GetInt(PlayerPrefsKeys.HIGH_SCORE);

            if (_currentScore > highScore)
            {
                PlayerPrefs.SetInt(PlayerPrefsKeys.HIGH_SCORE, (int)_currentScore);
            }
            

            if (hasWatchAd)
            {
                _endGamePanel.ShowWithWatchAd();
            }
            else
            {
                _endGamePanel.ShowAsNormal();
            }
        }

        private void OnRevive(EventRevive eventData)
        {
            _gamePanel.SetActive(true);
            ShowTimeoutEffect(false, true);
            _endGamePanel.gameObject.SetActive(false);
            _endGameParticle.gameObject.SetActive(false);
        }

        public void OnRestartGame()
        {
            EventHub.ClearAll();
            SceneManager.LoadScene("PlatformTestScene");
        }

        private void ShowFishesEscapeEffect(EventCharacterPassLayer e)
        {
            if (e.hasPowerup || e.hasLayerDestroyed)
                return;

            PedestalType layerType = PedestalType.None;
            foreach (var item in e.layer.pedestalInfos)
            {
                if (item.type == PedestalType.Pedestal_01 ||
                    item.type == PedestalType.Pedestal_01_1_Fish ||
                    item.type == PedestalType.Pedestal_01_3_Fish)
                {
                    layerType = item.type;
                    break;
                }
            }

            if (layerType == PedestalType.Pedestal_01 || layerType == PedestalType.Pedestal_01_1_Fish || layerType == PedestalType.Pedestal_01_3_Fish)
            {
                Vector3 effectPosition = new Vector3(0f, e.layer.height + 0.3f, _mainCharacter.transform.position.z - 1);
                var effect = _effectPool.Instantiate(layerType, effectPosition, 0);
                effect.transform.SetParent(_camera, true);
            }
        }

        private void ShowPedestalLayerDestroyEffect(EventPedestalLayerDestroy e)
        {
            SoundManager.PlayOneShot(SoundConfig.BreakFloor);
            Vector3 effectPosition = new Vector3(0f, e.layer.height - 0.5f, 0);
            _effectPool.Instantiate(_destroyPedestalLayerPrefab.ID, effectPosition, 0);
        }

        private SkinSetting.SkinData GetSkinById(string skinId)
        {
            return _skinSetting != null ? _skinSetting.GetSkinById(skinId) : null;
        }
    }
}
