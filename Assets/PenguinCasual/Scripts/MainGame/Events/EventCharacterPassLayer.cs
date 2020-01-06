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
            this.hasPowerup = false;
            this.hasCombo = false;
            this.hasLayerDestroyed = false;
        }

        public PedestalLayer layer;
        public bool hasPowerup;
        public bool hasCombo;
        public bool hasLayerDestroyed;
    }
}