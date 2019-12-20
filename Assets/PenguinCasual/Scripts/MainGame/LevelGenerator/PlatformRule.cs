using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Penguin
{
    public interface PlatformRule
    {
        List<PedestalInfo> GetPedestalInfos(int level);
    }

    public class TestPlarformRule : PlatformRule
    {
        public List<PedestalInfo> GetPedestalInfos(int level)
        {
            List<PedestalInfo> _pedestalInfos = new List<PedestalInfo>();

            int randomNone = Random.Range(0, 6);
            for (int i = 0; i < 7; i++)
            {
                if (i == randomNone)
                    continue;
                else
                    _pedestalInfos.Add(new PedestalInfo(PedestalType.Pedestal_01, i));
            }

            return _pedestalInfos;
        }
    }
}
