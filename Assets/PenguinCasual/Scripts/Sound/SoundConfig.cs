using UnityEngine;

namespace Penguin.Sound
{
    [CreateAssetMenu(fileName = "SoundConfig.asset", menuName = "Penguin/Settings/Sound Setting", order = 10)]
    public class SoundConfig: ScriptableObject
    {
        public string BGM = "Sounds/BGM";
        public string PenguinHitAndDie = "Sounds/OUT加工済み";
        public string FishMoveEndGame = "Sounds/魚演出加工済み";
        public string BreakFloor = "Sounds/台座破壊音加工済み";
        public string Jump = "Sounds/ペンギン音加工済み";
    }
}