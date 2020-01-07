﻿using System;
using System.Collections;
using System.Collections.Generic;
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

        [SerializeField] private GameObject _termsDialog;
        [SerializeField] private GameObject _licenseDialog;

        private void Start()
        {
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
            _termsDialog.gameObject.SetActive(true);
        }
    
        public void OpenPolicy()
        {
            Application.OpenURL("https://my.cybird.ne.jp/sp-inq/agreement");
        }
    
        public void OpenLicense()
        {
            _licenseDialog.gameObject.SetActive(true);
        }

        public void ChangeName()
        {
            if (_nameInputField.text != "" && _nameInputField.text.Length <= 6)
            {
                NetworkCaller.Instance.ChangeName(_nameInputField.text, () =>
                {
                    NativeDialog.OpenDialog("Success",
                        "Name changes successfully", "Ok",
                        () => { });
                }, (errorCode) =>
                {
                    NativeDialog.OpenDialog("Error",
                        errorCode == 422
                            ? "Cannot change into this name. Please select another name"
                            : "Cannot connect to server", "Ok",
                        () => { });
                });
            }
        }
    }    
}
