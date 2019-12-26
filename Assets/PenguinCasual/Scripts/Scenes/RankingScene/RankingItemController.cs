using System;
using UnityEngine;
using UnityEngine.UI;

namespace Penguin.Scenes.RankingScene
{
    public class RankingItemController : MonoBehaviour
    {
        public int Rank;
        public string Name;
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
            _score.text = $"{Score}点";
        }
    }
}