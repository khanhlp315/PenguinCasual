using System;
using DG.Tweening;
using Penguin.Utilities;
using UnityEngine;
using UnityEngine.UI;

namespace Penguin
{
    public class NewRecordPanel: MonoBehaviour
    {
        [SerializeField]
        private Text _labelScore;
        
        [SerializeField]
        private Image _starEffect;

        [SerializeField] 
        private RectTransform _bannerImage;

        private long _score;
        
        private void OnEnable()
        {
            _labelScore.text = ScoreUtil.FormatScore(_score);
            _labelScore.color = Color.white;
            _starEffect.color = Color.white;
            _bannerImage.anchoredPosition = new Vector2(_bannerImage.sizeDelta.x + 100, 0);

            var starBlinkEffect = DOTween.Sequence();
            starBlinkEffect.Append(_starEffect.DOColor(new Color(1, 1, 1, 0), 0.5f));
            starBlinkEffect.Append(_starEffect.DOColor(new Color(1, 1, 1, 1), 0.5f));
            starBlinkEffect.Append(_starEffect.DOColor(new Color(1, 1, 1, 0), 0.5f));
            starBlinkEffect.Append(_starEffect.DOColor(new Color(1, 1, 1, 1), 0.5f));

            var textBlinkEffect = DOTween.Sequence();
            textBlinkEffect.Append(_labelScore.DOColor(new Color(1, 1, 1, 0), 0.5f));
            textBlinkEffect.Append(_labelScore.DOColor(new Color(1, 1, 1, 1), 0.5f));
            textBlinkEffect.Append(_labelScore.DOColor(new Color(1, 1, 1, 0), 0.5f));
            textBlinkEffect.Append(_labelScore.DOColor(new Color(1, 1, 1, 1), 0.5f));

            var imageMoveEffect = DOTween.Sequence();
            imageMoveEffect.Append(_bannerImage.DOAnchorPosX(_bannerImage.sizeDelta.x/2, 0.2f));
            imageMoveEffect.Append(_bannerImage.DOAnchorPosX(0, 0.6f));
            imageMoveEffect.Append(_bannerImage.DOAnchorPosX(-_bannerImage.sizeDelta.x/2, 1.0f));
            imageMoveEffect.Append(_bannerImage.DOAnchorPosX(-_bannerImage.sizeDelta.x - 100, 0.2f));


            var sequence = DOTween.Sequence();
            
            sequence.Insert(0, starBlinkEffect);
            sequence.Insert(0, imageMoveEffect);
            sequence.Insert(0, textBlinkEffect);

        }

        public void SetScore(long score)
        {
            _score = score;
        }
    }
}