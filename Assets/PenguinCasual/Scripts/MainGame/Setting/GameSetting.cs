using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Penguin
{
    [CreateAssetMenu(fileName = "GameSetting", menuName = "Penguin/Settings/Game Setting", order = 10)]
    public class GameSetting : ScriptableObject
    {
        [Header("General")]
		public int roundDuration;
        public int reviveRoundDuration;
        public int squidBonusDuration;
        public int watchAdBonusDuration;
        public int powerUpBreakFloors;
        public int comboToBreakFloor;

        [Header("Platform")]
        public float distancePerPedestalLayer;
        public float unitToAngleRotation;

        [Header("Start setting")]
        public float pedestalStartPosition;
        public float characterStartPosition;
        public float cameraStartPosition;

        [Header("Character")]
        public float characterMaxDroppingVelocity;
        public float characterJumpVelocity;
        public float characterGravity;

        [Header("Camera")]
        public float characterOffsetWithCamera;
    }
}
