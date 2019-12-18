using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Penguin
{
    public class DroppingCameraTest : MonoBehaviour
    {
        [SerializeField] Platform _platform;
        [SerializeField] float _droppingSpeed;

        void Update()
        {
            Vector3 pos = transform.position;
            pos.y -= _droppingSpeed * Time.deltaTime;
            transform.position = pos;
            _platform.UpdatePenguinPosition(pos);
        }
    }

}
