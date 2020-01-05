using System;
using System.Collections.Generic;
using System.Linq;
using PenguinCasual.Scripts.Utilities;
using UnityEngine;

namespace Penguin
{
    [CreateAssetMenu(fileName = "BackgroundSetting", menuName = "Penguin/Settings/Background Setting", order = 10)]
    public class BackgroundSetting: ScriptableObject
    {
        [Serializable]
        public class BackgroundData
        {
            public int id;
            public GameObject prefabModel;
            public Sprite skinAvatar;
            public int characterIdToUnlock;
            public int characterPlayTimesToUnlock;

            public bool IsUnlocked()
            {
                if (characterIdToUnlock <= 0)
                {
                    return true;
                }

                var playTimes = PlayerPrefsHelper.GetCharacterPlayTimes(characterIdToUnlock);
                return playTimes >= characterPlayTimesToUnlock;
            }
        }
        
        public List<BackgroundData> backgroundDataList = new List<BackgroundData>();

        public List<BackgroundData> GetUnlockedBackgrounds()
        {
            return backgroundDataList.Where(x => x.IsUnlocked()).ToList();
        }

    }
}