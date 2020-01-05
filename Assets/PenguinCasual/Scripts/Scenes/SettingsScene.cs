using System;
using System.Collections;
using System.Collections.Generic;
using Penguin.Network;
using Penguin.Sound;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Penguin.Scenes
{
    public class SettingsScene : MonoBehaviour
    {
        [SerializeField]
        private InputField _nameInputField;

        [SerializeField] private GameObject _termsDialog;
        [SerializeField] private GameObject _licenseDialog;

        private void Start()
        {
            _nameInputField.text = NetworkCaller.Instance.PlayerData.Nickname;
        }


        public void GoToHomeScene()
        {
            SceneManager.LoadScene("HomeScene");
        }

        public void TurnOnSound()
        {
            //TODO: Turn on sound
        }
        public void TurnOffSound()
        {
            //TODO: Turn off sound
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
                    Debug.LogError("Change nickname successful");
                }, () =>
                {
                    Debug.LogError("Change nickname failed");
                });
            }
        }
    }    
}
