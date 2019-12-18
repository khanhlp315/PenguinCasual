using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Penguin.Scenes
{
    public class HomeScene : MonoBehaviour
    {
        public void GoToGameScene()
        {
            SceneManager.LoadScene("GameScene");
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
