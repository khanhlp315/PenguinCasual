using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Penguin
{
    [CreateAssetMenu(fileName = "ScoreSetting", menuName = "Penguin/Settings/Score Setting", order = 10)]
    public class ScoreSetting : ScriptableObject
    {
        public int basicScore;

        [Header("-------Multiply------")]

        [Tooltip("The Multiply value in score calculation formular : BasicScore * Multiply + Increase")]
        public int floorTypeNoFishMultiply;

        [Tooltip("The Multiply value in score calculation formular : BasicScore * Multiply + Increase")]
        public int floorTypeOneFishMultiply;

        [Tooltip("The Multiply value in score calculation formular : BasicScore * Multiply + Increase")]
        public int floorTypeThreeFishMultiply;

        [Tooltip("The multiply value in combo calculation formular : BasicScore * Multiply * PassingFloor")]
        public int comboMultiply;

        [Header("------Increasement------")]
        [Tooltip("The Increase value in score calculation formular : BasicScore * Multiply + Increase")]
        public int floorTypeNoFishIncrease;

        [Tooltip("The Increase value in score calculation formular : BasicScore * Multiply + Increase")]
        public int floorTypeOneFishIncrease;

        [Tooltip("The Increase value in score calculation formular : BasicScore * Multiply + Increase")]
        public int floorTypeThreeFishIncrease;


        [Header("---------Booster-----------")]
        public int basicBoosterScore;

        public List<int> passingFloorMultiplies = new List<int>();
    }
}