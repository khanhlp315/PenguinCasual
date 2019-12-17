namespace Ads.Interface
{
    public interface IPlatformAdId 
    {
        string NormalBannerAdId { get; }
        string EndGameBannerAdId { get; }
        string RewardTimeUpId { get; }
        string RewardDieId { get; }
    }
}