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
        public Action<Pedestal> OnStuckInPedestal;
        [SerializeField] private GameObject _model;
        [SerializeField] private BoxCollider _collider;

        private GameSetting _gameSetting;
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
        public void Setup(GameSetting gameSetting)
        {
            _gameSetting = gameSetting;
            _defaultLayerMask = LayerMask.GetMask("Default");
            
            Vector3 startPos = transform.position;
            startPos.y = gameSetting.characterStartPosition;
            transform.position = startPos;
        }

        public void CustomUpdate()
        {
            if (State == CharacterState.Dead)
                return;

            _velocity -= _gameSetting.characterGravity * Time.deltaTime;
            _velocity = Mathf.Max(_velocity, _gameSetting.characterMaxDroppingVelocity);
            float moveDistance = _velocity * Time.deltaTime;
            int totalHit = Physics.BoxCastNonAlloc(transform.position + _collider.center, 
                                                    _collider.size / 2, 
                                                    Vector3.down,
                                                    _rayCastHits, 
                                                    Quaternion.identity, 
                                                    moveDistance < 0 ? Mathf.Abs(moveDistance) : 0,
                                                    _defaultLayerMask);
            Pedestal stuckInPedestal = null;

            Vector3 position = transform.position;
            position.y += moveDistance;

            for (int i = 0; i < totalHit; i++)
            {
                Pedestal pedestal = _rayCastHits[i].collider.GetComponent<Pedestal>();
                if (pedestal == null)
                    pedestal = _rayCastHits[i].collider.GetComponentInParent<Pedestal>();
                if (pedestal == null || !pedestal.Active)
                    continue;

                OnCollideWithPedestal?.Invoke(pedestal);

                if (!pedestal.CanGoThrough)
                {
                    if (_rayCastHits[i].distance > 0 && position.y < _rayCastHits[i].point.y)
                    {
                        position.y = _rayCastHits[i].point.y;
                    }
                    else if (_rayCastHits[i].distance <= 0)
                    {
                        stuckInPedestal = pedestal;
                    }
                }
            }

            transform.position = position;

            if (stuckInPedestal != null)
            {
                OnStuckInPedestal?.Invoke(stuckInPedestal);
            }
        }

        public bool DryCheckCollision()
        {
            float moveDistance = 0;
            int totalHit = Physics.BoxCastNonAlloc(transform.position + _collider.center,
                                                    _collider.size / 2,
                                                    Vector3.down,
                                                    _rayCastHits,
                                                    Quaternion.identity,
                                                    moveDistance < 0 ? Mathf.Abs(moveDistance) : 0,
                                                    _defaultLayerMask);
            
            for (int i = 0; i < totalHit; i++)
            {
                Pedestal pedestal = _rayCastHits[i].collider.GetComponent<Pedestal>();
                if (pedestal == null)
                    pedestal = _rayCastHits[i].collider.GetComponentInParent<Pedestal>();
                if (pedestal == null || !pedestal.Active)
                    continue;

                if (!pedestal.CanGoThrough)
                {
                    return true;
                }
            }

            return false;
        }

        public void Jump()
        {
            _velocity = _gameSetting.characterJumpVelocity;
        }

        public void Revive()
        {
            State = CharacterState.Normal;
        }

        public void OnDie()
        {
            State = CharacterState.Dead;
        }

        public void ActivePowerup()
        {
            _velocity = _gameSetting.characterMaxDroppingVelocity;
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