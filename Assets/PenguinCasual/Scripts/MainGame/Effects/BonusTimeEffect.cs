using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Penguin
{
    public class BonusTimeEffect : IdentifiedMonoBehaviour
    {
        public override object ID => "BonusTimeEffect";

        [SerializeField]
        Image _image;

        [SerializeField]
        TextMeshProUGUI _text;

        public void SetTime(float time)
        {
            _text.text = $"+{Mathf.Round(time)}";
        }

        public override void OnObjectHasReused()
        {
            Color color = _image.color;
            color.a = 0f;
            _image.color = color;

            transform.DOLocalMoveY(60, 1f).SetRelative(true).SetEase(Ease.OutSine);

            Sequence imageSeq = DOTween.Sequence();
            imageSeq.Append(_image.DOFade(1, 0.2f));
            imageSeq.AppendInterval(0.6f);
            imageSeq.Append(_image.DOFade(0, 0.2f));

            Sequence textSeq = DOTween.Sequence();
            textSeq.Append(_text.DOFade(1, 0.2f));
            textSeq.AppendInterval(0.6f);
            textSeq.Append(_text.DOFade(0, 0.2f));
            textSeq.AppendCallback(() =>
            {
                ReturnToPool();
            });
        }
    }
}