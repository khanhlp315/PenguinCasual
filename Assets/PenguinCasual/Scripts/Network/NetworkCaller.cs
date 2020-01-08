using System.Collections;
using System.Collections.Generic;
using System.Text;
using Penguin.Dialogues;
using Penguin.Network.Data;
using Penguin.Utilities;
using PenguinCasual.Scripts.Utilities;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;

namespace Penguin.Network
{
    public class NetworkCaller : MonoSingleton<NetworkCaller, NetworkConfig>
    {

        private PlayerData _playerData;

        private string _token;

        public PlayerData PlayerData
        {
            get => _playerData;
            private set => _playerData = value;
        }

        public override void Initialize()
        {
            Debug.Log(this.GetType());
            if (PlayerPrefsHelper.HasToken())
            {
                _token = PlayerPrefsHelper.GetToken();
            }
            else
            {
                StartCoroutine(SendPostRequest(_config.GetTokenPath, (json, responseCode) =>
                    {
                        if (responseCode == 200)
                        {
                            _token = TokenInfo.FromJson(json).Token;
                            PlayerPrefsHelper.SetToken(_token);
                        }
                        else
                        {
                            NativeDialogManager.Instance.ShowConnectionErrorDialog(Initialize, Application.Quit);
                            return;
                        }
                        Initialize();
                    }));
                return;
            }

            StartCoroutine(SendPostRequest(_config.CurrentPlayerPath, (json, responseCode) =>
                {
                    Debug.Log(responseCode);
                    if (responseCode == 200)
                    {
                        _playerData = PlayerDataResponse.FromJson(json).Get();
                        var localHighScore = PlayerPrefsHelper.GetHighScore();
                        var totalScore = PlayerPrefsHelper.GetTotalScore();
                        if (localHighScore > _playerData.HighestScore || totalScore > _playerData.TotalScore)
                        {
                            UpdateHighScore(localHighScore, totalScore);
                        }
                    }
                    else
                    {
                        NativeDialogManager.Instance.ShowConnectionErrorDialog(Initialize, Application.Quit);
                        return;
                    }

                    OnInitializeDone?.Invoke();
                }));
        }

        private IEnumerator SendPostRequest(string path, UnityAction<string, long> onResponseReceived = null, string requestBody = null)
        {
            var downloadHandler = new DownloadHandlerBuffer();
            var request = new UnityWebRequest($"{_config.BaseUrl}/{path}", "POST")
            {
                downloadHandler = downloadHandler
            };
            if (_token != null)
            {
                request.SetRequestHeader("Authorization", $"Bearer {_token}");
            }
            if (requestBody != null)
            {
                var bodyRaw = Encoding.UTF8.GetBytes(requestBody);
                request.uploadHandler = new UploadHandlerRaw(bodyRaw);
                request.SetRequestHeader("Content-Type", "application/json");
            }
            yield return request.SendWebRequest();

            onResponseReceived?.Invoke(downloadHandler.text, request.responseCode);
        }

        public void ChangeName(string nickname, UnityAction onDone = null, UnityAction<int, string> onError = null)
        {
            StartCoroutine(SendPostRequest(_config.PutPlayerPath, (json, responseCode) =>
                {
                    if (responseCode == 200)
                    {
                        Debug.Log("Change ok");
                        var playerData = PlayerDataResponse.FromJson(json).Get();
                        _playerData.Nickname = playerData.Nickname;
                        _playerData.SkinId = playerData.SkinId;
                        onDone?.Invoke();
                    }
                    else 
                    {
                        Debug.Log(responseCode);
                        Debug.Log(json);
                        onError?.Invoke((int)responseCode, responseCode ==0? null: ErrorInfo.FromJson(json).Message);
                    }

                }, $"{{\"nickname\": \"{nickname}\" }}"));
        }

        public void GetTopPlayers(UnityAction<List<TopPlayerData>> onDone, UnityAction onError)
        {
            StartCoroutine(SendPostRequest(_config.TopPlayersPath, (json, responseCode) =>
            {
                if (responseCode == 200)
                {
                    var playersData = TopPlayersResponse.FromJson(json).GetAll();
                    onDone?.Invoke(playersData);
                }
                else
                {
                    onError?.Invoke();
                }
            }));
        }

        public void UpdateHighScore(int highScore, int totalScore, UnityAction onDone = null, UnityAction onError = null)
        {
            StartCoroutine(SendPostRequest(_config.UpdateScorePath, (json, responseCode) =>
            {
                if (responseCode == 200)
                {
                    var playerData = PlayerDataResponse.FromJson(json).Get();
                    _playerData.HighestScore = playerData.HighestScore;
                    _playerData.TotalScore = playerData.TotalScore;
                    _playerData.Rank = playerData.Rank;
                    onDone?.Invoke();
                }
                else
                {
                    Debug.LogError("Cannot connect to server");
                    onError?.Invoke();
                }

            }, $"{{\"total_score\": \"{totalScore}\",\"highest_score\": \"{highScore}\" }}"));
        }

        public void GetAllSkins(UnityAction<List<SkinData>, List<UnlockData>> onDone, UnityAction onError)
        {
            List<SkinData> skinsData = null;
            List<UnlockData> unlocksData = null;
            StartCoroutine(SendPostRequest(_config.GetAllSkinsPath, (json, responseCode) =>
            {
                if (responseCode == 200)
                {
                    Debug.Log(json);
                    skinsData = SkinDataResponse.FromJson(json).GetAll();
                    if (unlocksData != null)
                    {
                        onDone?.Invoke(skinsData, unlocksData);
                    }
                }
                else
                {
                    Debug.LogError("Cannot connect to server");
                    onError?.Invoke();
                }
            }));
            
            StartCoroutine(SendPostRequest(_config.GetAllUnlocksPath, (json, responseCode) =>
            {
                if (responseCode == 200)
                {
                    unlocksData = UnlockDataResponse.FromJson(json).GetAll();
                    if (skinsData != null)
                    {
                        onDone?.Invoke(skinsData, unlocksData);
                    }
                }
                else
                {
                    Debug.LogError("Cannot connect to server");
                    onError?.Invoke();
                }
            }));
        }

        public void SelectSkin(int skinId, UnityAction onDone, UnityAction onError)
        { 
            StartCoroutine(SendPostRequest(_config.UpdateSkinPath, (json, responseCode) =>
            {
                if (responseCode == 200)
                {
                    var playerData = PlayerDataResponse.FromJson(json).Get();
                    _playerData.SkinId = playerData.SkinId;
                    onDone?.Invoke();
                }
                else
                {
                    onError?.Invoke();
                }

            }, $"{{\"skin_id\": \"{skinId}\" }}"));        
        }
    }
}