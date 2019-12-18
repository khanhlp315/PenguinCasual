namespace Penguin.Ads
{
    public static class Advertiser
    {
        static AdvertisementSystem _advertisementSystem = null;
        public static AdvertisementSystem AdvertisementSystem{
            get{
                if (_advertisementSystem == null) {
                    _advertisementSystem = new AdvertisementSystem ();
                    _advertisementSystem.Add<AdmobAdvertisementSystem> ();
                    _advertisementSystem.Add<FluctAdvertisementSystem> ();

                }
                return _advertisementSystem;
            }
        }

        public static void SetAdvertisementSystem (AdvertisementSystem sys)
        {
            _advertisementSystem = sys;
        }
    }
}