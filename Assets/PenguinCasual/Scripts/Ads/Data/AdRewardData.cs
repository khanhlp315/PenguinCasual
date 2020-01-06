namespace Penguin.Ads.Data
{
    [System.Serializable]
    public enum AdRewardType
    {
        NONE = 0,
        GEM,
        LIVES
    }
	
    public class AdRewardData
    {
        public AdRewardType Type;
        public float Value;
    }
}