using GoogleMobileAds.Api;
using UnityEngine;
using AdPosition = Penguin.Ads.Data.AdPosition;

namespace Penguin.Ads
{
    public class AdmobAdvertisementSystem : IAdvertisement 
    {
        private BannerView _normalBannerAd;
        private BannerView _endGameBannerAd;
        private RewardedAd _dieRewardAd;
        private RewardedAd _timeUpRewardAd;
        private bool _isNormalBannerAdLoaded = false;
        private bool _isEndGameBannerAdLoaded = false;
        private bool _isNormalBannerAdShow = false;
        private bool _isEndGameBannerAdShow = false;
        private bool _isDieRewardShow = false;
        private bool _isTimeUpRewardShow = false;

        public AdmobAdvertisementSystem()
        {
            MobileAds.Initialize(initStatus => { });
        }

        public void LoadNormalBanner(string adsId, Data.AdPosition pos)
        {
            _normalBannerAd?.Destroy();
            
            var size = AdSize.Banner;
            
            _normalBannerAd = new BannerView(adsId, size, pos == Data.AdPosition.Top? GoogleMobileAds.Api.AdPosition.Top: GoogleMobileAds.Api.AdPosition.Bottom);
            _normalBannerAd.OnAdLoaded += (sender, e) => {
                _isNormalBannerAdLoaded = true;
                _isNormalBannerAdShow = true;
                Callback?.OnNormalBannerAdLoaded();
            };
            _normalBannerAd.OnAdFailedToLoad += (sender, e) => {
                _isNormalBannerAdLoaded = false;
                Callback?.OnNormalBannerAdFailedToLoad();
            };
            _normalBannerAd.OnAdOpening += (sender, e) => {
                Callback?.OnNormalBannerAdShow();
            };
            _normalBannerAd.OnAdClosed += (sender, e) => {
                _isNormalBannerAdShow = false;
                Callback?.OnNormalBannerAdDismiss();
            };
            _normalBannerAd.LoadAd (new AdRequest.Builder ().Build ());
        }

        public bool IsNormalBannerReady => _isNormalBannerAdLoaded;
        public bool IsNormalBannerShowing => _isNormalBannerAdShow;
        public void ShowNormalBanner()
        {
            _isNormalBannerAdShow = true;
            _normalBannerAd?.Show();
        }

        public void HideNormalBanner()
        {
            _isNormalBannerAdShow = false;
            _normalBannerAd?.Hide();
        }

        public void LoadEndGameBanner(string adsId, Data.AdPosition pos)
        {
            _endGameBannerAd?.Destroy();

            var size = AdSize.Banner;
            
            _endGameBannerAd = new BannerView(adsId, size, pos == Data.AdPosition.Top? GoogleMobileAds.Api.AdPosition.Top: GoogleMobileAds.Api.AdPosition.Bottom);
            _endGameBannerAd.OnAdLoaded += (sender, e) => {
                _isEndGameBannerAdLoaded = true;
                Callback?.OnEndGameBannerAdLoaded();
            };
            _endGameBannerAd.OnAdFailedToLoad += (sender, e) => {
                _isEndGameBannerAdLoaded = false;
                Callback?.OnEndGameBannerAdFailedToLoad();
            };
            _endGameBannerAd.OnAdOpening += (sender, e) => {
                _isEndGameBannerAdShow = true;
                Callback?.OnEndGameBannerAdShow();
            };
            _endGameBannerAd.OnAdClosed += (sender, e) => {
                _isEndGameBannerAdShow = false;
                Callback?.OnEndGameBannerAdDismiss();
            }; 
            _endGameBannerAd.LoadAd (new AdRequest.Builder ().Build ());
        }

        public bool IsEndGameBannerReady => _isEndGameBannerAdLoaded;
        public bool IsEndGameBannerShowing => _isEndGameBannerAdShow;
        public void ShowEndGameBanner()
        {
            _isEndGameBannerAdShow = true;
            _endGameBannerAd?.Show();
        }

        public void HideEndGameBanner()
        {
            _isEndGameBannerAdShow = false;
            _endGameBannerAd?.Hide();
        }

        public void LoadDieRewardAds(string adsId)
        {
            _dieRewardAd = new RewardedAd(adsId);
            _dieRewardAd.OnAdLoaded += (sender, e) =>
            {
                Callback?.OnDieRewardLoaded();
            };
            _dieRewardAd.OnAdFailedToLoad += (sender, e) => {
                LoadDieRewardAds(adsId);
                Callback?.OnDieRewardFailedToLoad();
            };
            _dieRewardAd.OnAdOpening += (sender, e) => {
                _isDieRewardShow = true;
                Callback?.OnDieRewardShow();
            };
            _dieRewardAd.OnAdClosed += (sender, e) => {
                _isDieRewardShow = false;
                LoadDieRewardAds(adsId);
                Callback?.OnDieRewardDismiss();
            };
            _dieRewardAd.OnUserEarnedReward += (sender, e) =>
            {
                Callback?.OnDieRewardDidReward();
            };
            AdRequest request = new AdRequest.Builder().Build();
            _dieRewardAd.LoadAd(request);
        }

        public bool IsDieRewardAdsReady => _dieRewardAd.IsLoaded();
        public bool IsDieRewardAdShowing => _isDieRewardShow;
        public void ShowDieRewardAds(string adsId)
        {
            if (_dieRewardAd.IsLoaded())
            {
                _dieRewardAd.Show();
            }     
        }

        public void LoadTimeUpRewardAds(string adsId)
        {
            _timeUpRewardAd = new RewardedAd(adsId);
            _timeUpRewardAd.OnAdLoaded += (sender, e) =>
            {
                Callback?.OnTimeUpRewardLoaded();
            };
            _timeUpRewardAd.OnAdFailedToLoad += (sender, e) => {
                LoadTimeUpRewardAds(adsId);
                Callback?.OnTimeUpRewardFailedToLoad();
            };
            _timeUpRewardAd.OnAdOpening += (sender, e) => {
                _isTimeUpRewardShow = true;
                Callback?.OnTimeUpRewardShow();
            };
            _timeUpRewardAd.OnAdClosed += (sender, e) => {
                _isTimeUpRewardShow = false;
                LoadTimeUpRewardAds(adsId);
                Callback?.OnTimeUpRewardDismiss();
            };
            _timeUpRewardAd.OnUserEarnedReward += (sender, e) =>
            {
                Callback?.OnTimeUpRewardDidReward();
            };
            AdRequest request = new AdRequest.Builder().Build();
            _timeUpRewardAd.LoadAd(request);
        }

        public bool IsTimeUpRewardAdsReady => _timeUpRewardAd.IsLoaded();
        public bool IsTimeUpRewardAdShowing => _isTimeUpRewardShow;
        public void ShowTimeUpRewardAds(string adsId)
        {
            if (_timeUpRewardAd.IsLoaded())
            {
                _timeUpRewardAd.Show();
            }
        }

        public void RemoveAds()
        {
            _normalBannerAd.Destroy();
            _endGameBannerAd.Destroy();

            _isNormalBannerAdLoaded = false;
            _isEndGameBannerAdLoaded = false;
            _isNormalBannerAdShow = false;
            _isEndGameBannerAdShow = false;
            _isDieRewardShow = false;
            _isTimeUpRewardShow = false;
        }

        public string HandleProvider => "Admob";
        public IAdvertisementDelegate Callback { get; set; }
    }
}