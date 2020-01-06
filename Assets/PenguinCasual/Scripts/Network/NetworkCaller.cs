using System.Collections;
using System.Collections.Generic;
using System.Text;
using Penguin.Network.Data;
using Penguin.Utilities;
using PenguinCasual.Scripts.Utilities;
using pingak9;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;

namespace Penguin.Network
{
    public class NetworkCaller : MonoSingleton<NetworkCaller>
    {
        private const string _baseUrl = "http://18.179.159.55";
        private const string _getTokenPath = "oauth/token";
        private const string _currentPlayerPath = "api/get/player";
        private const string _topPlayersPath = "api/get/player_top";
        private const string _putPlayerPath = "api/put/player";
        private const string _updateScorePath = "api/post/player_score";
        private const string _getAllSkinsPath = "api/get/skin";
        private const string _updateSkinPath = "api/put/player";
        private const string _getAllUnlocksPath = "api/get/unlock";



        private PlayerData _playerData;

        private string _token;

        public PlayerData PlayerData
        {
            get => _playerData;
            private set => _playerData = value;
        }

        public override void Initialize()
        {
            if (PlayerPrefsHelper.HasToken())
            {
                _token = PlayerPrefsHelper.GetToken();
            }
            else
            {
                StartCoroutine(SendPostRequest(_getTokenPath, (json, responseCode) =>
                    {
                        if (responseCode == 200)
                        {
                            _token = TokenInfo.FromJson(json).Token;
                            PlayerPrefsHelper.SetToken(_token);
                        }
                        else
                        {
                            NativeDialog.OpenDialog("Cannot connect to server", "Do you want to retry?", "Yes", "No",
                                Initialize,
                                Application.Quit);
                        }
                        Initialize();
                    }));
                return;
            }

            StartCoroutine(SendPostRequest(_currentPlayerPath, (json, responseCode) =>
                {
                    Debug.Log(responseCode);
                    if (responseCode == 200)
                    {
                        _playerData = PlayerDataResponse.FromJson(json).Get();
                        var localHighScore = PlayerPrefsHelper.GetHighScore();
                        if (localHighScore > _playerData.HighestScore)
                        {
                            UpdateHighScore(localHighScore);
                        }
                    }
                    else
                    {
                        NativeDialog.OpenDialog("Cannot connect to server", "Do you want to retry?", "Yes", "No",
                            Initialize,
                            Application.Quit);
                    }

                    OnInitializeDone();
                }));
        }

        private IEnumerator SendPostRequest(string path, UnityAction<string, long> onResponseReceived = null, string requestBody = null)
        {
            var downloadHandler = new DownloadHandlerBuffer();
            var request = new UnityWebRequest($"{_baseUrl}/{path}", "POST")
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

        public void ChangeName(string nickname, UnityAction onDone = null, UnityAction<int> onError = null)
        {
            StartCoroutine(SendPostRequest(_putPlayerPath, (json, responseCode) =>
                {
                    if (responseCode == 200)
                    {
                        var playerData = PlayerDataResponse.FromJson(json).Get();
                        _playerData.Nickname = playerData.Nickname;
                        _playerData.SkinId = playerData.SkinId;
                        onDone?.Invoke();
                    }
                    else 
                    {
                        onError?.Invoke((int)responseCode);
                    }

                }, $"{{\"nickname\": \"{nickname}\" }}"));
        }

        public void GetTopPlayers(UnityAction<List<TopPlayerData>> onDone, UnityAction onError)
        {
            StartCoroutine(SendPostRequest(_topPlayersPath, (json, responseCode) =>
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

        public void UpdateHighScore(int highScore, UnityAction onDone = null, UnityAction onError = null)
        {
            StartCoroutine(SendPostRequest(_updateScorePath, (json, responseCode) =>
            {
                if (responseCode == 200)
                {
                    var playerData = PlayerDataResponse.FromJson(json).Get();
                    _playerData.HighestScore = playerData.HighestScore;
                    _playerData.Rank = playerData.Rank;
                    onDone?.Invoke();
                }
                else
                {
                    Debug.LogError("Cannot connect to server");
                    onError?.Invoke();
                }

            }, $"{{\"score\": \"{highScore}\" }}"));
        }

        public void GetAllSkins(UnityAction<List<SkinData>, List<UnlockData>> onDone, UnityAction onError)
        {
            List<SkinData> skinsData = null;
            List<UnlockData> unlocksData = null;
            StartCoroutine(SendPostRequest(_getAllSkinsPath, (json, responseCode) =>
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
            
            StartCoroutine(SendPostRequest(_getAllUnlocksPath, (json, responseCode) =>
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
            StartCoroutine(SendPostRequest(_updateSkinPath, (json, responseCode) =>
            {
                if (responseCode == 200)
                {
                    var playerData = PlayerDataResponse.FromJson(json).Get();
                    _playerData.HighestScore = playerData.HighestScore;
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