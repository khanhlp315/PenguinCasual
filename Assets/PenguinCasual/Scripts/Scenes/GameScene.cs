using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Penguin.Ads;
using Penguin.Analytics;
using Penguin.Dialogues;
using Penguin.Network;
using Penguin.Sound;
using Penguin.Utilities;
using PenguinCasual.Scripts.Utilities;
using pingak9;
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
        private BackgroundSetting _backgroundSetting;
        [SerializeField]
        private GameSetting _gameSetting;

        [Header("------------Character---------------")]
        [SerializeField]
        private Transform _camera;
        [SerializeField]
        private Canvas _mainCanvas;

        [Header("------------Character---------------")]
        [SerializeField]
        private Character _mainCharacter;

        [Header("-----------Backgrounds-------------")]
        [SerializeField]
        private Vector3 _backgroundPosition;

        [SerializeField]
        private GameObject _background;

        [SerializeField] private Transform _backgroundCanvas;

        [Header("-----------UI--------------")]
        [SerializeField]
        private GameObject _hideGroup;
        [SerializeField]
        private TextMeshProUGUI _labelScore;
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

        [Header("-------Effect----------")]

        [SerializeField]
        private List<FishEscapeEffect> _fishEscapeEffectPrefabs;
        [SerializeField]
        private DestroyPedestalLayerEffect _destroyPedestalLayerPrefab;
        [SerializeField]
        private BonusTimeEffect _bonusTimeEffectPrefab;

        [Header("-------Timeout----------")] 
        [SerializeField]
        private Image _timeOutEffect;
        [SerializeField]
        private Transform _clockScaleAnchor;
        [SerializeField]
        private Image _clockImage;
        [SerializeField]
        private Color _clockNormalColor;
        [SerializeField]
        private Color _clockTimeoutColor;

        [Header("-----------UI Menu-----------")]
        [SerializeField]
        private GameObject _gamePanel;
        [SerializeField]
        private NewRecordPanel _newRecordPanel;
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

        private bool _hasAlreadyRevived;

        private void Start()
        {
            Advertiser.AdvertisementSystem.HideNormalBanner();
            Advertiser.AdvertisementSystem.HideEndGameBanner();

            ChangeBackgroundAndSkin();
            PlayerPrefsHelper.CountGamesPlayed();

            RegisterEvent();
            InitPool();

            _currentScore = 0;
            _timeOutEffect.gameObject.SetActive(false);
            _hideGroup.SetActive(false);
            _gamePanel.SetActive(true);
            _endGamePanel.gameObject.SetActive(false);

            StartCoroutine(WaitAndStartGame());
        }

        private void ChangeBackgroundAndSkin()
        {
            var skinData = GetSkinById(NetworkCaller.Instance.PlayerData.SkinId);
            var unlockedBackgrounds = _backgroundSetting.GetUnlockedBackgrounds();
            Debug.Log("Unlocked backgrounds: " + unlockedBackgrounds.Count);
            var unlockedBackgroundsCount = unlockedBackgrounds.Count;
            var backgroundData = unlockedBackgrounds[Random.Range(0, unlockedBackgroundsCount)];
            Debug.Log(_mainCharacter);
            Debug.Log(skinData);
            _mainCharacter.SetModel(skinData.prefabModel);

            PlayerPrefsHelper.CountCharacterPlayTimes(skinData.id);
            
            StandardEvent.GameProgress.StartGame(skinData.id, backgroundData.id);

            _background = GameObject.Instantiate(backgroundData.prefabModel, _backgroundCanvas);
            _background.transform.localPosition = _backgroundPosition;
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
                _clockImage.color = _clockTimeoutColor;

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
                _clockImage.color = _clockNormalColor;
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
            _effectPool.RegisterPooledGO(_bonusTimeEffectPrefab);
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
            EventHub.Bind<EventGetBonusTime>(ShowBonusTimeEffect);
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
            EventHub.Unbind<EventGetBonusTime>(ShowBonusTimeEffect);
            EventHub.Unbind<EventRevive>(OnRevive);
        }

        private IEnumerator WaitAndStartGame()
        {
            _readyImage.SetActive(true);
            _labelCountdown.text = _gameSetting.roundDuration.ToString();
            _labelScore.text = "0";

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
            _labelScore.text = ScoreUtil.FormatScore(eventData.score);

            var spawnLabel = GameObject.Instantiate(_labelScoreIncrease, _labelScoreIncrease.transform.parent);
            spawnLabel.transform.localPosition = _labelScoreIncrease.transform.localPosition;
            spawnLabel.transform.localScale = Vector3.one;

            spawnLabel.gameObject.SetActive(true);
            spawnLabel.alpha = 1f;
            spawnLabel.text = "+" + ScoreUtil.FormatScore(eventData.increase);

            var seq = DOTween.Sequence();
            seq.Append(spawnLabel.transform.DOLocalMoveY(spawnLabel.transform.position.y, 3.0f));
            seq.Join(spawnLabel.DOFade(0, 3.0f));

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
            #if UNITY_EDITOR
            bool hasWatchAd = PlayerPrefsHelper.CanWatchAds();
#elif UNITY_IOS || UNITY_ANDROID
            bool hasWatchAd = PlayerPrefsHelper.CanWatchAds();
            if (hasWatchAd)
            {
                if (isTimeout)
                {
                    if (!Advertiser.AdvertisementSystem.IsTimeUpRewardAdsReady)
                    {
                        hasWatchAd = false;
                    }
                }
                else
                {
                    if (!Advertiser.AdvertisementSystem.IsDieRewardAdsReady)
                    {
                        hasWatchAd = false;
                    }
                } 
            }
#endif
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
                Sound2DManager.Instance.PlayFishMoveEndGameSound();
                _endGameParticle.gameObject.SetActive(true);
                _endGameParticle.Play(true);
            }
            yield return new WaitForSeconds(delay);
            _timeupImage.gameObject.SetActive(false);

            var highScore = PlayerPrefsHelper.GetHighScore();

            if (_currentScore > highScore)
            {
                PlayerPrefsHelper.UpdateHighScore((int)_currentScore);
                var totalScore = PlayerPrefsHelper.GetTotalScore();
                NetworkCaller.Instance.UpdateHighScore((int)_currentScore, totalScore, () =>
                {
                    
                }, () =>
                {
                    NativeDialogManager.Instance.ShowScoreUpdateErrorDialog();
                });
                _newRecordPanel.SetScore(_currentScore);
                _newRecordPanel.gameObject.SetActive(true);
                yield return new WaitForSeconds(2.0f);
                _newRecordPanel.gameObject.SetActive(false);
            }
            
            _gamePanel.SetActive(false);
            _endGamePanel.gameObject.SetActive(true);
            _endGamePanel.SetScore(_currentScore);
            _endGamePanel.SetIsGameEndedByDie(!isTimeout);

            if (hasWatchAd && !_hasAlreadyRevived)
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
            _hasAlreadyRevived = true;
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
            Sound2DManager.Instance.PlayBreakFloorSound();
            Vector3 effectPosition = new Vector3(0f, e.layer.height - 0.5f, 0);
            _effectPool.Instantiate(_destroyPedestalLayerPrefab.ID, effectPosition, 0);
        }

        private void ShowBonusTimeEffect(EventGetBonusTime e)
        {
            Vector3 effectPosition = new Vector3(e.squid.transform.position.x - 2, e.squid.transform.position.y, e.squid.transform.position.z);
            var effect = _effectPool.Instantiate(_bonusTimeEffectPrefab.ID, Vector3.zero, 0);
            var bonusTimeEffect = effect.GetComponent<BonusTimeEffect>();

            bonusTimeEffect.SetTime(e.bonusTime);
            bonusTimeEffect.transform.SetParent(_mainCanvas.transform);
            bonusTimeEffect.transform.position = _bonusTimeEffectPrefab.transform.position;
            bonusTimeEffect.transform.localScale = Vector3.one;
        }

        private SkinSetting.SkinData GetSkinById(int skinId)
        {
            return _skinSetting != null ? _skinSetting.GetSkinById(skinId) : null;
        }
    }
}
