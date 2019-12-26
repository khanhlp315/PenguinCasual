using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Penguin.Network.Data;
using UnityEngine;
using UnityEngine.Networking;

namespace Penguin.Network
{
    public class NetworkCaller : MonoBehaviour
    {
        public static NetworkCaller instance;
        public static NetworkCaller Instance
        {
            get
            {
                if (instance == null) instance = FindObjectOfType(typeof(NetworkCaller)) as NetworkCaller;

                return instance;
            }
        }

        [SerializeField]
        private string _baseUrl = "http://18.179.159.55";

        private PlayerData _playerData;
        private List<TopPlayerData> _topPlayers;
        
        private string _token;

        void Awake( )
        {
            DontDestroyOnLoad(this.gameObject);
        }

        private IEnumerator SendPostRequest(string path, DownloadHandler downloadHandler, string requestBody = null)
        {
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
                Debug.Log(requestBody);
                var bodyRaw = Encoding.UTF8.GetBytes(requestBody);
                request.uploadHandler = (UploadHandler) new UploadHandlerRaw(bodyRaw);
                request.SetRequestHeader("Content-Type", "application/json");
            }
            yield return request.SendWebRequest();

            Debug.Log(request.responseCode);
        }
        
        private IEnumerator GetToken()
        {
            var downloadHandler = new DownloadHandlerBuffer();
            yield return SendPostRequest("oauth/token", downloadHandler);
            var tokenInfo = TokenInfo.FromJson(downloadHandler.text);
            _token = tokenInfo.Token;
            PlayerPrefs.SetString("token", _token);
        }

        private IEnumerator GetPlayerData()
        {
            var downloadHandler = new DownloadHandlerBuffer();
            yield return SendPostRequest("api/get/player", downloadHandler);
            var playerData = PlayerDataResponse.FromJson(downloadHandler.text).Get();
            _playerData = playerData;
        }
        
        private IEnumerator Start()
        {
            if(Instance != this)
            {
                Destroy(this.gameObject);
            }
            else if(instance == null)
            {
                instance = this;
            }
            
            if (PlayerPrefs.HasKey("token"))
            {
                _token = PlayerPrefs.GetString("token");
                yield return null;
            }
            else
            {
                yield return GetToken();
            }
            yield return GetPlayerData();
        }

        public PlayerData PlayerData
        {
            get => _playerData;
            private set => _playerData = value;
        }
        
        public List<TopPlayerData> TopPlayers
        {
            get => _topPlayers;
            private set => _topPlayers = value;
        }

        public void ChangeNickname(string nickname)
        {
            StartCoroutine(ChangeNicknameCoroutine(nickname));
        }

        private IEnumerator ChangeNicknameCoroutine(string nickname)
        {
            var downloadHandler = new DownloadHandlerBuffer();
            yield return SendPostRequest("api/put/player", downloadHandler, $"{{\"nickname\": \"{nickname}\" }}");
        }

        private IEnumerator GetTopPlayers()
        {
            var downloadHandler = new DownloadHandlerBuffer();
            yield return SendPostRequest("api/get/player_top", downloadHandler);
            _topPlayers = JsonUtility.FromJson<TopPlayersResponse>(downloadHandler.text).GetAll();
        }
    }
}