using Fluct;
using UnityEngine;
using AdPosition = Ads.Data.AdPosition;

namespace Ads
{
    public class FluctAdvertisementSystem: IAdvertisement
    {
        private RewardedVideo _dieRewardAd;
        private RewardedVideo _timeUpRewardAd;
        private bool _isDieRewardShow = false;
        private bool _isTimeUpRewardShow = false;

        public FluctAdvertisementSystem()
        {
        }

        public void LoadNormalBanner(string adsId, AdPosition pos)
        {
            
        }

        public bool IsNormalBannerReady => false;
        public bool IsNormalBannerShowing => false;
        public void ShowNormalBanner()
        {
        }

        public void HideNormalBanner()
        {
        }

        public void LoadEndGameBanner(string adsId, AdPosition pos)
        {
            
        }

        public bool IsEndGameBannerReady => false;
        public bool IsEndGameBannerShowing => false;
        public void ShowEndGameBanner()
        {
        }

        public void HideEndGameBanner()
        {
        }

        public void LoadDieRewardAds(string adsId)
        {
            var groupId = adsId.Split('_')[0];
            var unitId = adsId.Split('_')[1];
            Debug.Log(groupId);
            Debug.Log(unitId);
            _dieRewardAd = new RewardedVideo(groupId, unitId);
            _dieRewardAd.OnDidLoad += (sender, e) =>
            {
                Callback?.OnDieRewardLoaded();
            };
            _dieRewardAd.OnDidFailToLoad += (sender, e) => {
                LoadDieRewardAds(adsId);
                Callback?.OnDieRewardFailedToLoad();
            };
            _dieRewardAd.OnDidOpen += (sender, e) => {
                _isDieRewardShow = true;
                Callback?.OnDieRewardShow();
            };
            _dieRewardAd.OnDidClose += (sender, e) => {
                _isDieRewardShow = false;
                LoadDieRewardAds(adsId);
                Callback?.OnDieRewardDismiss();
            };
            _dieRewardAd.OnShouldReward += (sender, e) =>
            {
                Callback?.OnDieRewardDidReward();
            };
            var targeting = new AdRequestTargeting();
            _dieRewardAd.Load(targeting);
        }

        public bool IsDieRewardAdsReady => _dieRewardAd.HasAdAvailable();
        public bool IsDieRewardAdShowing => _isDieRewardShow;
        public void ShowDieRewardAds(string adsId)
        {
            if (_dieRewardAd.HasAdAvailable())
            {
                _dieRewardAd.Present();
            }     
        }

        public void LoadTimeUpRewardAds(string adsId)
        {
            var groupId = adsId.Split('_')[0];
            var unitId = adsId.Split('_')[1];
            
            _timeUpRewardAd = new RewardedVideo(groupId, unitId);
            _timeUpRewardAd.OnDidLoad += (sender, e) =>
            {
                Callback?.OnTimeUpRewardLoaded();
            };
            _timeUpRewardAd.OnDidFailToLoad += (sender, e) => {
                LoadTimeUpRewardAds(adsId);
                Callback?.OnTimeUpRewardFailedToLoad();
            };
            _timeUpRewardAd.OnDidOpen += (sender, e) => {
                _isTimeUpRewardShow = true;
                Callback?.OnTimeUpRewardShow();
            };
            _timeUpRewardAd.OnDidClose += (sender, e) => {
                _isTimeUpRewardShow = false;
                LoadTimeUpRewardAds(adsId);
                Callback?.OnTimeUpRewardDismiss();
            };
            _timeUpRewardAd.OnShouldReward += (sender, e) =>
            {
                Callback?.OnTimeUpRewardDidReward();
            };
            var targeting = new AdRequestTargeting();
            _timeUpRewardAd.Load(targeting);
        }

        public bool IsTimeUpRewardAdsReady => _timeUpRewardAd.HasAdAvailable();
        public bool IsTimeUpRewardAdShowing => _isTimeUpRewardShow;
        public void ShowTimeUpRewardAds(string adsId)
        {
            if (_timeUpRewardAd.HasAdAvailable())
            {
                _timeUpRewardAd.Present();
            }
        }

        public void RemoveAds()
        {
            _isDieRewardShow = false;
            _isTimeUpRewardShow = false;
        }

        public string HandleProvider => "Fluct";
        public IAdvertisementDelegate Callback { get; set; }
    }
}