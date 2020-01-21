
using Penguin.Ads;
using Penguin.AppConfigs;
using Penguin.Sound;
using Penguin.Utilities;
using PenguinCasual.Scripts.Utilities;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Penguin.Scenes
{
    public class HomeScene : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI _highscoreText;
        private void Start()
        {
            Sound2DManager.Instance.PlayBgm();
            _highscoreText.text = ScoreUtil.FormatScore(PlayerPrefsHelper.GetHighScore());
            Advertiser.AdvertisementSystem.ShowNormalBanner();
        }
        
        private void Update()
        {
            if (AppConfigManager.Instance.IsMaintaining || AppConfigManager.Instance.NeedsUpdate)
            {
                SceneManager.LoadScene("MaintenanceOrUpdateScene");
            }
        }

        public void GoToGameScene()
        {
            SceneManager.LoadScene("PlatformTestScene");
        }

        public void GoToSettingsScene()
        {
            SceneManager.LoadScene("SettingsScene");

        }

        public void GoToLeaderBoardScene()
        {
            SceneManager.LoadScene("RankingScene");
        }

        public void GoToCharacterScene()
        {
            SceneManager.LoadScene("CharacterScene");
        }
    }
}
