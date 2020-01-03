
using Penguin.Ads;
using Penguin.Utilities;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Penguin.Scenes
{
    public class HomeScene : MonoBehaviour
    {
        [SerializeField]
        private Text _highscoreText;
        private void Start()
        {
            _highscoreText.text = ScoreUtil.FormatScore(PlayerPrefs.GetInt(PlayerPrefsKeys.HIGH_SCORE)) + "<size=18>匹</size>";
            Advertiser.AdvertisementSystem.ShowNormalBanner();
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
