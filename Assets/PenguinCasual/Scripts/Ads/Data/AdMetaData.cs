namespace Penguin.Ads.Data
{
    [System.Serializable]
    public enum AdPosition
    {
        Top = 0,
        Bot = 1
    }
	
    [System.Serializable]
    public class AdMetaData
    {
        public AdPosition BannerAdPosition;
        public bool AutoShowBanner;
    }
}