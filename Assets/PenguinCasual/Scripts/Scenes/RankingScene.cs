using System;
using System.Collections;
using Penguin;
using Penguin.Dialogues;
using Penguin.Network;
using Penguin.Sound;
using Penguin.UI;
using Penguin.Utilities;
using PenguinCasual.Scripts.Utilities;
using pingak9;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Penguin.Scenes
{
    public class RankingScene : MonoBehaviour
    {
        [SerializeField] 
        private SkinSetting _skinSetting;
        [SerializeField]
        private RankingItemController _topPlayerItem;
        [SerializeField]
        private RectTransform _topPlayerList;

        [SerializeField] private Text _nameText;
        [SerializeField] private Text _scoreText;
        [SerializeField] private Text _rankText;

        [SerializeField]
        private GameObject _loadingLayer;

        private void Start()
        {
            var localHighscore = PlayerPrefsHelper.GetHighScore();
            var serverHighscore = NetworkCaller.Instance.PlayerData.HighestScore;
            var totalScore = PlayerPrefsHelper.GetTotalScore();
            var serverTotalScore = NetworkCaller.Instance.PlayerData.TotalScore;
            if (localHighscore > serverHighscore || totalScore > serverTotalScore)
            {
                CheckScore(localHighscore, totalScore);
            }
            else
            {
                var playerData = NetworkCaller.Instance.PlayerData;
                _nameText.text = playerData.Nickname;
                _scoreText.text = $"{ScoreUtil.FormatScore(playerData.TotalScore)}匹";
                _rankText.text = $"{ScoreUtil.FormatScore(playerData.Rank)}位";
                UpdateTopPlayers();
            }
        }

        private void CheckScore(int localHighscore, int totalScore)
        {
            NetworkCaller.Instance.UpdateHighScore(localHighscore, totalScore, () =>
                {
                    var responsePlayerData = NetworkCaller.Instance.PlayerData;
                    _nameText.text = responsePlayerData.Nickname;
                    _scoreText.text = $"{ScoreUtil.FormatScore(responsePlayerData.TotalScore)}匹";
                    _rankText.text = $"{ScoreUtil.FormatScore(responsePlayerData.Rank)}位";
                    UpdateTopPlayers();
                },
                () =>
                {
                    NativeDialogManager.Instance.ShowConnectionErrorDialog(
                        () => { CheckScore(localHighscore, totalScore); },
                        () => { SceneManager.LoadScene("HomeScene"); });
                });
        }

        private void UpdateTopPlayers()
        {
            NetworkCaller.Instance.GetTopPlayers((players) =>
            {
                int rank = 0;
                foreach (var player in players)
                {
                    var playerItem = Instantiate(_topPlayerItem, _topPlayerList, false);
                    playerItem.Rank = ++rank;
                    playerItem.Score = player.TotalScore;
                    playerItem.Name = player.Nickname;
                    playerItem.Avatar = _skinSetting.GetSkinById(player.SkinId).skinAvatar;
                }
                _loadingLayer.SetActive(false);
            }, () =>
            {
                NativeDialogManager.Instance.ShowConnectionErrorDialog(UpdateTopPlayers, () =>
                {
                    SceneManager.LoadScene("HomeScene"); 
                });
            });
        }
    

        public void GoToHomeScene()
        {
            SceneManager.LoadScene("HomeScene");
        }

        public void GoToSettingsScene()
        {
            SceneManager.LoadScene("SettingsScene");

        }
    }

}