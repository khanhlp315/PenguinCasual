using UnityEngine;

namespace Penguin.Network
{
    [CreateAssetMenu(fileName = "NetworkConfig.asset", menuName = "Penguin/Settings/Network Setting", order = 10)]
    public class NetworkConfig: ScriptableObject
    {
        public string BaseUrl = "http://18.179.159.55";
        public string GetTokenPath = "oauth/token";
        public string CurrentPlayerPath = "api/get/player";
        public string TopPlayersPath = "api/get/player_top";
        public string PutPlayerPath = "api/put/player";
        public string UpdateScorePath = "api/post/player_score";
        public string GetAllSkinsPath = "api/get/skin";
        public string UpdateSkinPath = "api/put/player";
        public string GetAllUnlocksPath = "api/get/unlock";
    }
}