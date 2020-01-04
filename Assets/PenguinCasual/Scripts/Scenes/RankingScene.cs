using System;
using System.Collections;
using Penguin.Network;
using Penguin.Sound;
using Penguin.UI;
using Penguin.Utilities;
using PenguinCasual.Scripts.Utilities;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class RankingScene : MonoBehaviour
{
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
        if (localHighscore < serverHighscore)
        {
            NetworkCaller.Instance.UpdateHighScore(localHighscore, () =>
            {
                var responsePlayerData = NetworkCaller.Instance.PlayerData;
                _nameText.text = responsePlayerData.Nickname;
                _scoreText.text = $"{ScoreUtil.FormatScore(responsePlayerData.HighestScore)}匹";
                _rankText.text = $"{ScoreUtil.FormatScore(responsePlayerData.Rank)}位";
            });
        }
        
        var playerData = NetworkCaller.Instance.PlayerData;
        _nameText.text = playerData.Nickname;
        _scoreText.text = $"{ScoreUtil.FormatScore(playerData.HighestScore)}匹";
        _rankText.text = $"{ScoreUtil.FormatScore(playerData.Rank)}位";
        
        NetworkCaller.Instance.GetTopPlayers((players) =>
        {
            int rank = 0;
            foreach (var player in players)
            {
                var playerItem = Instantiate(_topPlayerItem, _topPlayerList, false);
                playerItem.Rank = ++rank;
                playerItem.Score = player.HighestScore;
                playerItem.Name = player.Nickname;
            }
            _loadingLayer.SetActive(false);
        }, () =>
        {
            
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
