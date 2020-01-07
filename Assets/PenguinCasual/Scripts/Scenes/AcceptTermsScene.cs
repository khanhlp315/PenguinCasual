using Penguin.Ads;
using PenguinCasual.Scripts.Utilities;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Penguin.Scenes
{
    public class AcceptTermsScene : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _text;
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
            PlayerPrefsHelper.SetFirstTimeUser();
            SceneManager.LoadScene("HomeScene");
        }
    }
}
