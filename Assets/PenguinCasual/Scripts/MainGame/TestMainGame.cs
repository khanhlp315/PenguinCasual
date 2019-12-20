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
            if (_character.transform.position.y - pedestal.transform.position.y < -0.3f)
                return;

            if (pedestal.type == PedestalType.Pedestal_01)
            {
                _character.Jump();
            }
            else if (pedestal.type == PedestalType.DeadZone_01)
            {
                Debug.Log("Dead by touching deadzone");
            }
            else if (pedestal.type == PedestalType.Wall_01)
            {
                Debug.Log("Dead by touching wall");
            }
        }
    }
}

