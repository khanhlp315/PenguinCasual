using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Penguin
{
    public class DestroyPedestalLayerEffect : IdentifiedMonoBehaviour
    {
        public override object ID => "DestroyPedestalLayerEffect";

        void OnParticleSystemStopped()
        {
            ReturnToPool();
        }
    }
}