using UnityEngine;

namespace Penguin.Network.Data
{
    [System.Serializable]
    public class TokenInfo
    {
        [SerializeField]
        private string token;
        [SerializeField]
        private string token_type;

        public string Token => token;

        public static TokenInfo FromJson(string jsonString)
        {
            return JsonUtility.FromJson<TokenInfo>(jsonString);
        }
    }
    
}