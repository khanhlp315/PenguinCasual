using System.Collections;
using System.Collections.Generic;
using Penguin.Ads;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Penguin.Scenes
{
    public class AcceptTermsScene : MonoBehaviour
    {
        [SerializeField] private Text _text;
        [SerializeField] private string _androidTermsPath;
        [SerializeField] private string _iosTermsPath;

        private void Start()
        {
            Advertiser.AdvertisementSystem.HideNormalBanner();
            Advertiser.AdvertisementSystem.HideEndGameBanner();
#if UNITY_ANDROID
            _text.text = Resources.Load<TextAsset>(_androidTermsPath).text;
#elif UNITY_IOS
            _text.text = Resources.Load<TextAsset>(_iosTermsPath).text;
#endif
        }
        
        // Update is called once per frame
        public  void GoToHomeScreen()
        {
            PlayerPrefs.SetInt("HasAcceptTerms", 0);
            SceneManager.LoadScene("HomeScene");
        }
    }
}
