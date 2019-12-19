using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Penguin
{
    public enum PedestalType
    {
        None,
        Pedestal_01,
        Wall
    }

    public class Pedestal : MonoBehaviour
    {
        public PedestalType type;
    }    
}

