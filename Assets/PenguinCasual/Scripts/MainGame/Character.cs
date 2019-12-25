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

        void Update()
        {
            if (State == CharacterState.Dead)
                return;

            _velocity -= _gravity * Time.deltaTime;
            _velocity = Mathf.Max(_velocity, _maxDroppingVelocity);
            float moveDistance = _velocity * Time.deltaTime;

            int totalHit = Physics.BoxCastNonAlloc(transform.position + _collider.center, _collider.size / 2, Vector3.down, _rayCastHits, Quaternion.identity, Mathf.Abs(moveDistance));

            Vector3 position = transform.position;
            position.y += moveDistance;
            transform.position = position;


            for (int i = 0; i < totalHit; i++)
            {
                Pedestal pedestal = _rayCastHits[i].collider.GetComponent<Pedestal>();
                if (pedestal != null)
                {
                    OnCollideWithPedestal?.Invoke(pedestal);
                }
            }
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