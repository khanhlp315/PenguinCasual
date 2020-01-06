using Penguin.Ads.Interface;
using UnityEngine;

namespace Penguin.Ads.Data
{
    [System.Serializable]
    public class AdPlacementData : IAdPlacement 
    {
        #region IAdPlacement implementation
        [SerializeField]
        string placementName;
        public string PlacementName => placementName;

        [SerializeField]
        int frequency;
        public int Frequency {
            get {
                return frequency;
            }
        }

        [SerializeField]
        bool enableShowByPercentage;
        public bool EnableShowByPercentage => enableShowByPercentage;

        int currentFrequency;
        public int CurrentFrequency {
            get => currentFrequency;
            set => currentFrequency = value;
        }

        #endregion
		
    }
}