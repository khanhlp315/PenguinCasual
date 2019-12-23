using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Penguin
{
    public class TestCharacter : MonoBehaviour
    {
        public Action<Pedestal> OnCollideWithPedestal;
        [SerializeField] private float _maxDroppingVelocity;
        [SerializeField] private float _jumpVelocity;
        [SerializeField] private float _gravity;
        [SerializeField] private BoxCollider _collider;
        RaycastHit[] _rayCastHits = new RaycastHit[5];
        private float _velocity;

        private bool _isDead;

        void Update()
        {
            if (_isDead)
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

        public void Die()
        {
            // TODO:
            _isDead = true;
        }
    }
}
