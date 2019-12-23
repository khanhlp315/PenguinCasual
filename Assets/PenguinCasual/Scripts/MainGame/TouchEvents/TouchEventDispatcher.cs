using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Penguin
{
    public class TouchEventDispatcher : MonoBehaviour
    {
        bool _isMouseDown = false;
        Vector2 _startPosition;
        Vector2 _lastPosition;

        void Update()
        {
            #if UNITY_EDITOR
            HandleTouchOnEditor();
            #else
            HandleTouchOnMobile();
            #endif
        }
        void HandleTouchOnEditor()
        {
            if (Input.GetMouseButtonDown(0))
            {
                _isMouseDown = true;
                _lastPosition = _startPosition = Input.mousePosition;
                EventHub.Emit(new EventTouchBegan(_startPosition));
                return;
            }

            if (Input.GetMouseButtonUp(0))
            {
                _isMouseDown = false;
                Vector2 touchPos = Input.mousePosition;
                EventHub.Emit(new EventTouchEnded(touchPos, _startPosition));
                return;
            }

            if (_isMouseDown)
            {
                Vector2 touchPos = Input.mousePosition;
                EventHub.Emit(new EventTouchMoved(touchPos, touchPos - _lastPosition, _startPosition));
                _lastPosition = touchPos;
                return;
            }
        }

        void HandleTouchOnMobile()
        {
            if (Input.touchCount <= 0)
                return;

            Touch touch = Input.GetTouch(0);
            Vector2 touchPos = touch.position;

            if (touch.phase == TouchPhase.Began)
            {
                _lastPosition = _startPosition = touchPos;
                EventHub.Emit(new EventTouchBegan(_startPosition));
            }
            else if (touch.phase == TouchPhase.Moved)
            {
                EventHub.Emit(new EventTouchMoved(touchPos, touchPos - _lastPosition, _startPosition));
                _lastPosition = touchPos;
            }
            else if (touch.phase == TouchPhase.Ended)
            {
                EventHub.Emit(new EventTouchEnded(touchPos, _startPosition));
            }
            else if (touch.phase == TouchPhase.Canceled) 
            {
                EventHub.Emit(new EventTouchCanceled(touchPos));
            }
        }
    }
}
