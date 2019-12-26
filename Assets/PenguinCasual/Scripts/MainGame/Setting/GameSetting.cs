using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Penguin
{
    [CreateAssetMenu(fileName = "GameSetting", menuName = "Penguin/Settings/Game Setting", order = 10)]
    public class GameSetting : ScriptableObject
    {
        public int RoundDuration;

        public int SquidBonusDuration;

        public int WatchAdBonusDuration;
    }
}