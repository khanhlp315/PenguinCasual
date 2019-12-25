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
        DeadZone_01,
        Wall_01
    }

    public class Pedestal : MonoBehaviour
    {
        public PedestalType type;
    }    
}

