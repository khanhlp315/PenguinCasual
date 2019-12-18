using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Penguin
{
    public enum Obstacle
    {
        None,
        Pedestal,
        Wall
    }

    public class Platform : MonoBehaviour
    {
        //TODO: temporary
        [SerializeField] float _unitToAngleRotation;

        [SerializeField] Transform _propInstance;
        List<Transform> _props;
        float _propHeight;
        float _propLength;
        float _platformAngle;

        void Awake()
        {
            EventHub.Bind<EventTouchMoved>(OnTouchMoved, true);
            
            MeshRenderer propMeshRenderer = _propInstance.GetComponent<MeshRenderer>();
            _propHeight = propMeshRenderer.bounds.size.y;
            _propLength = _propHeight * 2;

            float propHeight = _propInstance.GetComponent<MeshRenderer>().bounds.size.y;
            Transform propBackBuffer = Instantiate(_propInstance, new Vector3(0, -propHeight * 2f, 0), Quaternion.identity, transform);
            propBackBuffer.localScale = new Vector3(1f, -_propInstance.localScale.y, 1f);

            _props = new List<Transform>();
            _props.Add(_propInstance);
            _props.Add(propBackBuffer);

            _platformAngle = 0;
        }

        void OnTouchMoved(EventTouchMoved e)
        {
            Camera cam = Camera.main;
            float xMove = e.deltaPos.x / Camera.main.pixelWidth;
            float rotateAngle = xMove * _unitToAngleRotation;
            _platformAngle -= rotateAngle;
            transform.rotation = Quaternion.Euler(0f, _platformAngle, 0f);
        }

        public Obstacle UpdatePenguinPosition(Vector3 position)
        {
            CheckAndRecycleObstacles(position);
            return Obstacle.None;
        }

        void CheckAndRecycleObstacles(Vector3 position)
        {
            float centerBotPropY = transform.position.y - _propLength + _propHeight / 2;
            if (position.y < centerBotPropY)
            {
                _propLength += _propHeight;
                
                Vector3 propPosition = _props[0].position;
                propPosition.y -= _propHeight * 2;
                _props[0].position = propPosition;

                _props.Reverse();
            }
        }
    }
}

