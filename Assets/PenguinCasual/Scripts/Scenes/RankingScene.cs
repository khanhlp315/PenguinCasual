using System;
using System.Collections;
using Penguin.Network;
using Penguin.Sound;
using Penguin.UI;
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

    private IEnumerator Start()
    {
        yield return NetworkCaller.Instance.GetTopPlayers();
        int rank = 0;
        foreach (var player in NetworkCaller.Instance.TopPlayers)
        {
            var item = Instantiate(_topPlayerItem, _topPlayerList, false);
            item.Rank = ++rank;
            item.Name = player.Nickname;
            item.Score = player.HighestScore;
        }

        _nameText.text = NetworkCaller.Instance.PlayerData.Nickname;
        _scoreText.text = $"{NetworkCaller.Instance.PlayerData.HighestScore}点";
        _rankText.text = $"{NetworkCaller.Instance.PlayerData.Rank}位";
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
