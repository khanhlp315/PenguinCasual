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
            if (level == 0)
                return GetFirstLevelPedestalInfo();

            List<PedestalInfo> pedestalInfos = new List<PedestalInfo>();
            List<int> slots = new List<int> {0, 1, 2, 3, 4, 5, 6};

            int numHole = Random.Range(0, int.MaxValue) % 3 + 1;
            int numDeadZone = Random.Range(0, int.MaxValue) % 2;
            int numWall = Random.Range(0, int.MaxValue) % 2;
            
            for (int i = 0; i < numHole; i++)
            {
                int randomSlot = Random.Range(0, slots.Count - 1);
                slots.RemoveAt(randomSlot);
            }

            for (int i = 0; i < numDeadZone; i++)
            {
                int randomSlot = Random.Range(0, slots.Count - 1);
                pedestalInfos.Add(new PedestalInfo(PedestalType.DeadZone_01, slots[randomSlot]));
                
                slots.RemoveAt(randomSlot);
            }

            for (int i = 0; i < slots.Count; i++)
            {
                pedestalInfos.Add(new PedestalInfo(PedestalType.Pedestal_01, slots[i]));
            }

            if (numWall > 0)
            {
                pedestalInfos.Add(new PedestalInfo(PedestalType.Wall_01, Random.Range(0, 6)));
            }

            return pedestalInfos;
        }

        private List<PedestalInfo> GetFirstLevelPedestalInfo()
        {
            List<PedestalInfo> pedestalInfos = new List<PedestalInfo>();
            List<int> slots = new List<int> { 0, 1, 2, 3, 4, 5, 6 };

            int randomSlot = Random.Range(0, slots.Count - 1);
            slots.RemoveAt(randomSlot);

            for (int i = 0; i < slots.Count; i++)
            {
                pedestalInfos.Add(new PedestalInfo(PedestalType.Pedestal_01, slots[i]));
            }

            return pedestalInfos;
        }
    }

    public class FinalPlatformRule : PlatformRule
    {
        private readonly Dictionary<PedestalType, float> _floorPercentages = new Dictionary<PedestalType, float>()
        {
            { PedestalType.Pedestal_01 , 0.6f },
            { PedestalType.Pedestal_01_1_Fish , 0.2f },
            { PedestalType.Pedestal_01_3_Fish , 0.2f }
        };

        public List<PedestalInfo> GetPedestalInfos(int level)
        {
            if (level == 0)
                return GetFirstLevelPedestalInfo();

            List<PedestalInfo> pedestalInfos = new List<PedestalInfo>();
            List<int> slots = new List<int> { 0, 1, 2, 3, 4, 5, 6 };

            PedestalType pedestalType = PedestalType.Pedestal_01;
            float totalPercentage = 0;

            foreach (var item in _floorPercentages)
            {
                totalPercentage += item.Value;
            }

            foreach (var item in _floorPercentages)
            {
                float chance = Random.value * totalPercentage;
                if (chance < item.Value)
                {
                    pedestalType = item.Key;
                    break;
                }
                else
                {
                    totalPercentage -= item.Value;
                }
            }

            int numHole = Random.Range(0, int.MaxValue) % 3 + 1;
            int numDeadZone = Random.Range(0, int.MaxValue) % 2;
            int numWall = Random.Range(0, int.MaxValue) % 2;

            for (int i = 0; i < numHole; i++)
            {
                int randomSlot = Random.Range(0, slots.Count - 1);
                slots.RemoveAt(randomSlot);
            }

            for (int i = 0; i < numDeadZone; i++)
            {
                int randomSlot = Random.Range(0, slots.Count - 1);
                pedestalInfos.Add(new PedestalInfo(PedestalType.DeadZone_01, slots[randomSlot]));

                slots.RemoveAt(randomSlot);
            }

            for (int i = 0; i < slots.Count; i++)
            {
                pedestalInfos.Add(new PedestalInfo(pedestalType, slots[i]));
            }

            if (numWall > 0)
            {
                pedestalInfos.Add(new PedestalInfo(PedestalType.Wall_01, Random.Range(0, 6)));
            }

            return pedestalInfos;
        }

        private List<PedestalInfo> GetFirstLevelPedestalInfo()
        {
            List<PedestalInfo> pedestalInfos = new List<PedestalInfo>();
            List<int> slots = new List<int> { 0, 1, 2, 3, 4, 5, 6 };

            int randomSlot = Random.Range(0, slots.Count - 1);
            slots.RemoveAt(randomSlot);

            for (int i = 0; i < slots.Count; i++)
            {
                pedestalInfos.Add(new PedestalInfo(PedestalType.Pedestal_01, slots[i]));
            }

            return pedestalInfos;
        }
    }
}
