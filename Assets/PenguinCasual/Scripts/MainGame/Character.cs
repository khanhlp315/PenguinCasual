using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Penguin
{
    public enum CharacterState
    {
        Normal,
        Dead,
        Powerup
    }

    /// <summary>
    /// Handle logic for in-game main character
    /// </summary>
    public class Character : MonoBehaviour
    {
        #region [ Fields ]
        public Action<Pedestal> OnCollideWithPedestal;

        [SerializeField] private GameObject _model;

        [Header("-------Actions---------")]
        [SerializeField] private float _maxDroppingVelocity;
        [SerializeField] private float _jumpVelocity;
        [SerializeField] private float _gravity;
        [SerializeField] private BoxCollider _collider;


        RaycastHit[] _rayCastHits = new RaycastHit[5];
        private float _velocity;
        private LayerMask _defaultLayerMask;

        private CharacterState _state;

        public CharacterState State
        {
            get
            {
                return _state;
            }
            set
            {
                _state = value;
            }
        }

        #endregion

        #region [ Methods ]

        void Awake()
        {
            _defaultLayerMask = LayerMask.GetMask("Default");
        }

        void Update()
        {
            if (State == CharacterState.Dead)
                return;

            _velocity -= _gravity * Time.deltaTime;
            _velocity = Mathf.Max(_velocity, _maxDroppingVelocity);
            float moveDistance = _velocity * Time.deltaTime;
            int totalHit = Physics.BoxCastNonAlloc(transform.position + _collider.center, 
                                                    _collider.size / 2, 
                                                    Vector3.down,
                                                    _rayCastHits, 
                                                    Quaternion.identity, 
                                                    moveDistance < 0 ? Mathf.Abs(moveDistance) : 0,
                                                    _defaultLayerMask);

            Vector3 position = transform.position;
            position.y += moveDistance;
            
            if (totalHit > 0)
            {
                for (int i = 0; i < totalHit; i++)
                {
                    Pedestal pedestal = _rayCastHits[i].collider.GetComponent<Pedestal>();
                    if (pedestal == null || !pedestal.Active)
                        continue;

                    OnCollideWithPedestal?.Invoke(pedestal);

                    if (_rayCastHits[i].distance > 0 && position.y < _rayCastHits[i].point.y)
                    {
                        position.y = _rayCastHits[i].point.y;
                    }
                }
            }

            transform.position = position;
        }

        public void Jump()
        {
            _velocity = _jumpVelocity;
        }

        public void OnDie()
        {
            State = CharacterState.Dead;
        }

        public void SetModel(GameObject characterModel)
        {
            if (_model != null)
            {
                GameObject.Destroy(_model);
            }

            _model = GameObject.Instantiate(characterModel, transform);
            _model.transform.localPosition = Vector3.zero;
            _model.transform.rotation = Quaternion.identity;
        }

        #endregion
    }
}