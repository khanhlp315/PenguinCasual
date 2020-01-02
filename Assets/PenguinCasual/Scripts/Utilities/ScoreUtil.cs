using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Penguin.Utilities
{
    public static class ScoreUtil
    {
        public static string FormatScore(long score)
        {
            return string.Format("{0:n0}", score);
        }
    }
}