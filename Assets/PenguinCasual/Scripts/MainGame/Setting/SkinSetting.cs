using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Penguin
{
    /// <summary>
    /// ScriptableObject use to hold settings for penguin skin data
    /// </summary>
    [CreateAssetMenu(fileName = "SkinSetting", menuName = "Penguin/Settings/Skin Setting", order = 10)]
    public class SkinSetting : ScriptableObject
    {
        /// <summary>
        /// Define how our skin can unlock
        /// </summary>
        [System.Serializable]
        [Flags]
        public enum UnlockType
        {
            FREE = 0, // Free item unlock immediatelly at game boots
            IAP = 1 << 1, // In-app purchasing 
            CURRENCY = 1 << 2, // Using somekind of currency to unlock
            LEVEL = 1 << 3, // Reach specific level to unlock
            SCORE = 1 << 4 // Reach specific score to unlock
        }

        [System.Serializable]
        public class SkinData
        {
            public string id;
            public GameObject prefabModel;
            public UnlockType unlockType;
            public string iapProductId;
            public string currencyId;
            public float currencyAmount;
            public int levelReqire;
            public int scoreRequire;
        }

        public List<SkinData> skinDataList = new List<SkinData>();

        public SkinData GetSkinById(string id)
        {
            return skinDataList.Find(x => x.id == id);
        }
    }
}