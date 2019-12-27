using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace Penguin
{
    public class FishEscapeEffect : IdentifiedMonoBehaviour
    {
        [SerializeField] PedestalType _effectType;
        [SerializeField] List<SpriteRenderer> _groupFishLeft;
        [SerializeField] List<SpriteRenderer> _groupFishRight;
        Dictionary<SpriteRenderer, Vector3> _defaultPositions = new Dictionary<SpriteRenderer, Vector3>(); 

        public override object ID => _effectType;

        const float totalAnimation = 0.5f;

        void Awake()
        {
            foreach (var fish in _groupFishLeft)
            {
                _defaultPositions[fish] = fish.transform.localPosition;
            }

            foreach (var fish in _groupFishRight)
            {
                _defaultPositions[fish] = fish.transform.localPosition;
            }
        }

        public override void OnObjectHasReused()
        {
            foreach (var fish in _groupFishLeft)
            {
                fish.color = Color.white;
                fish.transform.localPosition = _defaultPositions[fish];
                
                fish.transform.DOLocalMoveY(_defaultPositions[fish].y + 0.75f, totalAnimation / 2);
                fish.transform.DOLocalMoveY(_defaultPositions[fish].y, totalAnimation / 2).SetDelay(totalAnimation / 2);
                fish.transform.DOLocalMoveX(-1.5f, totalAnimation).SetRelative(true);
                fish.DOColor(new Color(1f, 1f, 1f, 0.0f), totalAnimation / 4).SetDelay(totalAnimation * 0.75f);
            }

            foreach (var fish in _groupFishRight)
            {
                fish.color = Color.white;
                fish.transform.localPosition = _defaultPositions[fish];

                fish.transform.DOLocalMoveY(_defaultPositions[fish].y + 0.75f, totalAnimation / 2);
                fish.transform.DOLocalMoveY(_defaultPositions[fish].y, totalAnimation / 2).SetDelay(totalAnimation / 2);
                fish.transform.DOLocalMoveX(1.5f, totalAnimation).SetRelative(true);
                fish.DOColor(new Color(1f, 1f, 1f, 0.0f), totalAnimation / 4).SetDelay(totalAnimation * 0.75f);
            }

            Sequence sequence = DOTween.Sequence();
            sequence.AppendInterval(totalAnimation);
            sequence.AppendCallback(() => ReturnToPool());
        }
    }
}
