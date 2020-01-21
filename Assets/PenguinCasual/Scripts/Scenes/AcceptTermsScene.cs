using Penguin.Ads;
using Penguin.AppConfigs;
using PenguinCasual.Scripts.Utilities;
using UGUITagActionText;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Penguin.Scenes
{
    public class AcceptTermsScene : MonoBehaviour
    {
        private void Start()
        {
            Advertiser.AdvertisementSystem.HideNormalBanner();
            Advertiser.AdvertisementSystem.HideEndGameBanner();
        }

        private void Update()
        {
            if (AppConfigManager.Instance.IsMaintaining || AppConfigManager.Instance.NeedsUpdate)
            {
                SceneManager.LoadScene("MaintenanceOrUpdateScene");
            }
        }
        
        public void GoToHomeScreen()
        {
            PlayerPrefsHelper.SetFirstTimeUser();
            SceneManager.LoadScene("HomeScene");
        }

        public void GoToLink(string link, Vector2 screenPoint)
        {
            Application.OpenURL(link);
        }
    }
}
