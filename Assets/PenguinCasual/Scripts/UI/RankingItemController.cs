using System;
using Penguin.Utilities;
using UnityEngine;
using UnityEngine.UI;

namespace Penguin.UI
{
    public class RankingItemController : MonoBehaviour
    {
        [HideInInspector]
        public int Rank;
        [HideInInspector]
        public string Name;
        [HideInInspector]
        public int Score;

        [SerializeField]
        private Text _rank;
        
        [SerializeField]
        private Text _name;
        
        [SerializeField]
        private Text _score;

        private void Start()
        {
            _rank.text = Rank.ToString();
            _name.text = Name;
            _score.text = $"{ScoreUtil.FormatScore(Score)}匹";
        }
    }
}