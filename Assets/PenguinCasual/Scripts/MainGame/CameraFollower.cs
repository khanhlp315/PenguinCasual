using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Penguin
{
    public class CameraFollower : MonoBehaviour
    {
        [SerializeField] private Character _player;
        
        private GameSetting _gameSetting;
        private float _lowestPosition = float.MaxValue;

        public void Setup(GameSetting gameSetting)
        {
            _gameSetting = gameSetting;
            SetPositionY(gameSetting.cameraStartPosition);
        }

        public void CustomUpdate()
        {
            float newLowestPosition = _player.transform.position.y + _gameSetting.characterOffsetWithCamera;
            SetPositionY(newLowestPosition);
        }

        private void SetPositionY(float posY)
        {
            if (_lowestPosition > posY)
            {
                _lowestPosition = posY;
                Vector3 position = transform.position;
                position.y = _lowestPosition;
                transform.position = position;
            }
        }
    }
}
