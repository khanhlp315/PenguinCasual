﻿using System;
using System.Collections;
using System.Collections.Generic;
using Penguin.AppConfigs;
using Penguin.Dialogues;
using Penguin.Network;
using Penguin.Sound;
using Penguin.UI;
using pingak9;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Penguin.Scenes
{
    public class SettingsScene : MonoBehaviour
    {
        [SerializeField]
        private InputField _nameInputField;

        [SerializeField] private SwitchButtonGroup _switchButtonGroup;

        [SerializeField] private GameObject _androidTermsDialog;
        [SerializeField] private GameObject _iosTermsDialog;
        [SerializeField] private GameObject _licenseDialog;

        private void Start()
        {
            Sound2DManager.Instance.StopBgm();
            _nameInputField.text = NetworkCaller.Instance.PlayerData.Nickname;
            _switchButtonGroup.SetButton(Sound2DManager.Instance.IsMuteBGM? "Off": "On");
            _switchButtonGroup.OnButtonSelected += (buttonName) =>
            {
                if (buttonName == "Off")
                {
                    Sound2DManager.Instance.SetMuteSound(true);
                    Sound2DManager.Instance.SetMuteBGM(true);
                }
                else if (buttonName == "On")
                {
                    Sound2DManager.Instance.SetMuteSound(false);
                    Sound2DManager.Instance.SetMuteBGM(false);                    
                }
            };
        }


        public void GoToHomeScene()
        {
            SceneManager.LoadScene("HomeScene");
        }
        public void OpenRequest()
        {
            Application.OpenURL("https://jp.research.net/r/L22BJJW");
        }
    
        public void OpenTerms()
        {
            #if UNITY_IOS
            _iosTermsDialog.gameObject.SetActive(true);
            #elif UNITY_ANDROID
            _androidTermsDialog.gameObject.SetActive(true);
#endif
        }
    
        public void OpenPolicy()
        {
            Application.OpenURL("https://my.cybird.ne.jp/sp-inq/agreement");
        }
    
        public void OpenLicense()
        {
            _licenseDialog.gameObject.SetActive(true);
        }
        
        private void Update()
        {
            if (AppConfigManager.Instance.IsMaintaining || AppConfigManager.Instance.NeedsUpdate)
            {
                SceneManager.LoadScene("MaintenanceOrUpdateScene");
            }
        }

        public void ChangeName()
        {
            NetworkCaller.Instance.ChangeName(_nameInputField.text, () =>
            {
                NativeDialogManager.Instance.ShowChangeNameSuccessDialog();
            }, (errorCode, message) =>
            {
                if (errorCode == 422)
                {
                    NativeDialogManager.Instance.ShowChangeNameValidationError(message);

                }
                else
                {
                    NativeDialogManager.Instance.ShowConnectionErrorDialog(ChangeName, () =>
                    {
                            
                    });
                }
            });
        }
        
        public void GoToLink(string link, Vector2 screenPoint)
        {
            Application.OpenURL(link);
        }
    }    
}
