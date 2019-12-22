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
        private float _velocity;

        private bool _isDead;

        private void OnTriggerEnter(Collider other)
        {
            Pedestal pedestal = other.GetComponent<Pedestal>();
            if (pedestal != null)
            {
                OnCollideWithPedestal?.Invoke(pedestal);
            }
        }

        void Update()
        {
            if (_isDead)
                return;

            _velocity -= _gravity * Time.deltaTime;
            _velocity = Mathf.Max(_velocity, _maxDroppingVelocity);

            Vector3 position = transform.position;
            position.y += _velocity * Time.deltaTime;
            transform.position = position;
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
