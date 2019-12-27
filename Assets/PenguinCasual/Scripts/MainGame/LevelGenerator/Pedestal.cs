using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Penguin
{
    public enum PedestalType
    {
        None,
        Pedestal_01,
        Pedestal_01_1_Fish,
        Pedestal_01_3_Fish,
        Pedestal_04_Powerup,
        DeadZone_01,
        Wall_01
    }

    public class Pedestal : MonoBehaviour
    {
        public PedestalType type;
        private bool _active = true;

        public bool Active
        {
            get
            {
                return _active;
            }
            set
            {
                _active = value;
            }
        }

        public void Fall()
        {
            _active = false;
            transform.SetParent(null, true);
            StartCoroutine(_Fall());
        }


        // TODO: need to clean up
        private IEnumerator _Fall()
        {
            float animationTime = 0.75f;
            Vector3 downward = -transform.forward;
            downward = Quaternion.Euler(0f, 360f/14f, 0) * downward;
            Vector3 deltaPos = downward.normalized * 7;
            float velocityY = 2;
            float gravity = 20;
            // floatdeltaPos.y = -8;

            Vector3 perpendicularAxis = Quaternion.Euler(0f, 90f, 0) * downward;

            deltaPos = deltaPos / animationTime;

            float time = 0f;
            while (time <= animationTime)
            {
                time += Time.deltaTime;
                velocityY -= gravity * Time.deltaTime;

                // Update position
                Vector3 position = transform.position;
                position += deltaPos * Time.deltaTime;
                position.y += velocityY * Time.deltaTime;
                
                transform.position = position;

                // Update rotation
                Vector3 centerPoint = GetComponent<MeshRenderer>().bounds.center;
                transform.RotateAround(centerPoint, perpendicularAxis, -245 * Time.deltaTime);
                yield return new WaitForEndOfFrame();
            }

            EventHub.Emit(new EventPedestalDestroy(this));
        }
    }    
}

