using System.Collections.Generic;
using Ads.Data;

namespace Ads.Interface
{
    public interface IAdUnit {
        string Provider { get; }
        IPlatformAdId AdId { get; }
        AdMetaData AdMeta { get; }
        List<AdRewardData> RewardItems { get; }
    }

}