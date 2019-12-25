using System;
using System.Collections;
using Penguin.Network.Data;
using UnityEngine;
using UnityEngine.Networking;

namespace Penguin.Network
{
    public class NetworkCaller : MonoBehaviour
    {
        [SerializeField]
        private string _baseUrl = "http://18.179.159.55";
        
        
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
            if (requestBody == null)
            {
                yield return request.SendWebRequest();
            }
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
            var playerData = ReponseData<PlayerData>.FromJson(downloadHandler.text).GetSingle();
            Debug.Log(playerData.Nickname);
        }
        
        private IEnumerator Start()
        {
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


    }
}