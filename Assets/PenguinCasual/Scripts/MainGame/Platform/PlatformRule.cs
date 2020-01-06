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

    public class SimplePlatformRule : PlatformRule
    {
        private readonly Dictionary<PedestalType, float> _floorPercentages = new Dictionary<PedestalType, float>()
        {
            { PedestalType.Pedestal_01 , 0.6f },
            { PedestalType.Pedestal_01_1_Fish , 0.2f },
            { PedestalType.Pedestal_01_3_Fish , 0.2f }
        };

        private readonly float _powerupPercentage = 0.3f;
        private readonly float _squidPercentage = 0.2f;

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
            int numPowerUp = Random.value < _powerupPercentage ? 1 : 0;
            int numSquid = Random.value < _squidPercentage ? 1 : 0;

            for (int i = 0; i < numHole; i++)
            {
                int randomSlot = Random.Range(0, slots.Count);
                slots.RemoveAt(randomSlot);
            }

            for (int i = 0; i < numDeadZone; i++)
            {
                int randomSlot = Random.Range(0, slots.Count);
                pedestalInfos.Add(new PedestalInfo(PedestalType.DeadZone_01, slots[randomSlot]));

                slots.RemoveAt(randomSlot);
            }

            for (int i = 0; i < numPowerUp; i++)
            {
                int randomSlot = Random.Range(0, slots.Count);
                pedestalInfos.Add(new PedestalInfo(PedestalType.Pedestal_04_Powerup, slots[randomSlot]));

                slots.RemoveAt(randomSlot);
            }

            for (int i = 0; i < numSquid; i++)
            {
                int randomSlot = Random.Range(0, slots.Count);
                pedestalInfos.Add(new PedestalInfo(PedestalType.Squid_01, slots[randomSlot]));
            }

            for (int i = 0; i < slots.Count; i++)
            {
                pedestalInfos.Add(new PedestalInfo(pedestalType, slots[i]));
            }

            var wallSlot = new List<int>(slots);
            for (int i = 0; i < numWall; i++)
            {
                int randomSlot = Random.Range(0, wallSlot.Count);
                pedestalInfos.Add(new PedestalInfo(PedestalType.Wall_01, wallSlot[randomSlot]));

                wallSlot.Remove(randomSlot);
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
