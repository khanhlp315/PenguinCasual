using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace Penguin
{
    public enum PedestalType
    {
        None,
        Pedestal_01,
        Pedestal_01_1_Fish,
        Pedestal_01_3_Fish,
        DeadZone_01,
        Wall_01
    }

    public struct EventPedestalDestroy : IEvent
    {
        public EventPedestalDestroy(Pedestal pedestal)
        {
            this.pedestal = pedestal;
        }

        public Pedestal pedestal;
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

        private IEnumerator _Fall()
        {
            float animationTime = 0.75f;
            Vector3 downward = -transform.forward;
            downward = Quaternion.Euler(0f, 360f/14f, 0) * downward;
            Vector3 deltaPos = downward.normalized * 6;
            deltaPos.y = -10;

            deltaPos = deltaPos / animationTime;
            // Quaternion.
            float time = 0f;
            while (time <= animationTime)
            {
                time += Time.deltaTime;

                Vector3 position = transform.position;
                position += deltaPos * Time.deltaTime;
                transform.position = position;
                yield return new WaitForEndOfFrame();
            }

            EventHub.Emit(new EventPedestalDestroy(this));
        }
    }    
}

