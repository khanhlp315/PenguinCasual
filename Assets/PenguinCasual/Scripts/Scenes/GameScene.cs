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

        private CoreGameModel _coreGameModel;

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
        }

        private void OnDestroy()
        {
            UnRegisterEvent();
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

        /// <summary>
        /// Receiver from event hub to update label score
        /// </summary>
        /// <param name="eventData"></param>
        private void OnScoreUpdate(EventUpdateScore eventData)
        {
            _labelScore.text = eventData.score.ToString();
        }

        private SkinSetting.SkinData GetSkinById(string skinId)
        {
            return _skinSetting != null ? _skinSetting.GetSkinById(skinId) : null;
        }
    }
}