using System.Collections.Generic;
using Penguin.Ads.Data;

namespace Penguin.Ads.Interface
{
    public interface IAdUnit {
        string Provider { get; }
        IPlatformAdId AdId { get; }
        AdMetaData AdMeta { get; }
        List<AdRewardData> RewardItems { get; }
    }

}