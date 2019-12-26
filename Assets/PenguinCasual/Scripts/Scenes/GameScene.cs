using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
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
        private Character _mainCharacter;

        [Header("-----------Backgrounds-------------")]
        [SerializeField]
        private Vector3 _backgroundPosition;

        [SerializeField]
        private GameObject _background;

        [Header("-----------UI--------------")]
        [SerializeField]
        private TextMeshProUGUI _labelScore;
        [SerializeField]
        private TextMeshProUGUI _labelCountdown;
        [SerializeField]
        private TextMeshProUGUI _labelWarning;

        private CoreGameModel _coreGameModel;

        private bool _isGameStart;
        private float _countDownDuration;

        private void Start()
        {
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

            StartCoroutine(WaitAndStartGame());
        }

        private void OnDestroy()
        {
            UnRegisterEvent();
        }

        private void Update()
        {
            if (_isGameStart)
            {
                _countDownDuration -= Time.deltaTime;
                _labelCountdown.text = Mathf.RoundToInt(_countDownDuration).ToString();
                if (_countDownDuration <= 0)
                {
                    _isGameStart = false;
                    EventHub.Emit<EventTimeout>();
                }
            }
        }

        /// <summary>
        /// Register event for update Views
        /// </summary>
        private void RegisterEvent()
        {
            EventHub.Bind<EventUpdateScore>(OnScoreUpdate);
        }

        /// <summary>
        /// Unregister events
        /// </summary>
        private void UnRegisterEvent()
        {
            EventHub.Unbind<EventUpdateScore>(OnScoreUpdate);
        }

        private IEnumerator WaitAndStartGame()
        {
            _countDownDuration = _gameSetting != null ? _gameSetting.RoundDuration : 60;

            _labelWarning.gameObject.SetActive(true);
            _labelWarning.text = "Ready";
            _labelCountdown.text = _countDownDuration.ToString();

            yield return new WaitForSeconds(2);

            _labelWarning.text = "Go!";

            yield return new WaitForSeconds(1);

            _labelWarning.gameObject.SetActive(false);

            _isGameStart = true;
            EventHub.Emit<EventStartGame>();
        }

        /// <summary>
        /// Receiver from event hub to update label score
        /// </summary>
        /// <param name="eventData"></param>
        private void OnScoreUpdate(EventUpdateScore eventData)
        {
            _labelScore.text = eventData.score.ToString();
        }

        /// <summary>
        /// Receiver from event hub to update game duration
        /// </summary>
        /// <param name="increase"></param>
        private void OnTimeUpdate(float increase)
        {
            _countDownDuration += increase;
        }

        private SkinSetting.SkinData GetSkinById(string skinId)
        {
            return _skinSetting != null ? _skinSetting.GetSkinById(skinId) : null;
        }
    }
}