using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Penguin
{
    public struct EventCharacterPassLayer : IEvent
    {
        public EventCharacterPassLayer(PedestalLayer layer)
        {
            this.layer = layer;
        }

        public PedestalLayer layer;
    }
}