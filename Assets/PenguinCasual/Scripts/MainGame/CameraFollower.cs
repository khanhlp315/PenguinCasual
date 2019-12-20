using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Penguin
{
    public class CameraFollower : MonoBehaviour
    {
        [SerializeField] TestCharacter _player;
        [SerializeField] float _playerOffset;
        float _lowestPosition = float.MaxValue;

        void Update()
        {
            float newLowestPosition = _player.transform.position.y + _playerOffset;
            if (_lowestPosition > newLowestPosition)
            {
                _lowestPosition = newLowestPosition;
                Vector3 position = transform.position;
                position.y = _lowestPosition;
                transform.position = position;
            }
        }
    }
}
