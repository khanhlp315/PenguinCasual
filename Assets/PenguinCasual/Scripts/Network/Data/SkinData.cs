using System;
using UnityEngine;

namespace Penguin.Network.Data
{
    [Serializable]
    public class SkinData
    {
        [SerializeField]
        private int id;

        [SerializeField] 
        private int unlock_id;

        [SerializeField] 
        private string name;

        [SerializeField] 
        private string introdution;

        public int Id => id;
        public int UnlockId => unlock_id;
        public string Name => name;
        public string Introduction => introdution;
    }
    
    [Serializable]
    public class SkinDataResponse : ResponseBodyWithMultipleEntities<SkinData>
    {
        public static SkinDataResponse FromJson(string jsonString)
        {
            return JsonUtility.FromJson<SkinDataResponse>(jsonString);
        }
    }
}