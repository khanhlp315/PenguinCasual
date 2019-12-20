using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Penguin
{
    public class TestMainGame : MonoBehaviour
    {
        [SerializeField] TestCharacter _character;
        [SerializeField] Platform _platform;

        void Awake()
        {
            _character.OnCollideWithPedestal += OnPlayerCollideWithPedestal;
        }

        void Update()
        {
            _platform.UpdatePenguinPosition(_character.transform.position);
        }

        void OnPlayerCollideWithPedestal(Pedestal pedestal)
        {
            // If character is in middle air, and player rotate the platform and causes collision, this threshold will prevent the character bounce back
            if (_character.transform.position.y - pedestal.transform.position.y > -0.3f)
            {
                _character.Jump();
                Debug.Log(_character.transform.position.y - pedestal.transform.position.y);
            }
                
        }
    }
}

