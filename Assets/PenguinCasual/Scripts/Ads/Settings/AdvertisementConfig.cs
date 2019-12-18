using System.Collections.Generic;
using Penguin.Ads.Data;
using UnityEngine;

namespace Penguin.Ads.Settings
{
    [System.Serializable]
    [CreateAssetMenu(menuName="Database/Advertisement Config" , fileName="AdConfig")]
    public class AdvertisementConfig : ScriptableObject
    {
        public List<AdUnit> listAds = new List<AdUnit>();
        public List<AdPlacementData> listPlacement = new List<AdPlacementData>();
    }
}