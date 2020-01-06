using System.Collections.Generic;
using Penguin.Ads.Interface;
using UnityEngine;

namespace Penguin.Ads.Data
{
    [System.Serializable]
    public class AdUnit : IAdUnit 
    {
        #region IAdUnit implementation
        [SerializeField]
        string provider;
        public string Provider => provider;

        [SerializeField]
        PlatformAdId adId;
        public IPlatformAdId AdId => adId;

        [SerializeField]
        AdMetaData adMeta;
        public AdMetaData AdMeta => adMeta;

        [SerializeField]
        List<AdRewardData> rewardItems = new List<AdRewardData>();
        public List<AdRewardData> RewardItems => rewardItems;

        #endregion
    }
}