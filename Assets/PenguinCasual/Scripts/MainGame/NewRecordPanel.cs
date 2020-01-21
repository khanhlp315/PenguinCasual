using System;
using DG.Tweening;
using Penguin.Utilities;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Penguin
{
    public class NewRecordPanel: MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI _labelScore;
        
        [SerializeField] private Animation _animation;

        private long _score;
        
        private void OnEnable()
        {
            _labelScore.text = ScoreUtil.FormatScore(_score);
            _animation.Play();
        }

        public void SetScore(long score)
        {
            _score = score;
        }
    }
}