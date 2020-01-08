using UnityEngine;

namespace Penguin.Network.Data
{
    [System.Serializable]
    public class ErrorInfo
    {
        [SerializeField]
        private string message;

        public string Message => message;

        public static ErrorInfo FromJson(string jsonString)
        {
            return JsonUtility.FromJson<ErrorInfo>(jsonString);
        }
    }
    
}