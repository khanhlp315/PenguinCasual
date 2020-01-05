using System.Collections;
using System.Collections.Generic;
using Penguin.Ads.Data;
using Penguin.Ads.Settings;
using UnityEngine;
using AdPosition = Penguin.Ads.Data.AdPosition;

namespace Penguin.Ads
{
    public interface IAdvertisement
    {
        void LoadNormalBanner (string adsId, Data.AdPosition pos);
        bool IsNormalBannerReady { get; }
        bool IsNormalBannerShowing { get; }
        void ShowNormalBanner( );
        void HideNormalBanner( );
        
        void LoadEndGameBanner (string adsId, Data.AdPosition pos);
        bool IsEndGameBannerReady { get; }
        bool IsEndGameBannerShowing { get; }
        void ShowEndGameBanner( );
        void HideEndGameBanner( );

        
        void LoadDieRewardAds (string adsId);
        bool IsDieRewardAdsReady{ get; }
        bool IsDieRewardAdShowing { get; }
        void ShowDieRewardAds (string adsId);
        
        void LoadTimeUpRewardAds (string adsId);
        bool IsTimeUpRewardAdsReady{ get; }
        bool IsTimeUpRewardAdShowing { get; }
        void ShowTimeUpRewardAds (string adsId);

        // Call this method will close and dispose all availabe ads in our system
        void RemoveAds( );

        string HandleProvider { get; }

        IAdvertisementDelegate Callback { get; set; }
    }
    
    public interface IAdvertisementDelegate
    {
        void OnNormalBannerAdLoaded ();
        void OnNormalBannerAdDismiss();
        void OnNormalBannerAdShow();
        void OnNormalBannerAdFailedToLoad();
        
        void OnEndGameBannerAdLoaded ();
        void OnEndGameBannerAdDismiss();
        void OnEndGameBannerAdShow();
        void OnEndGameBannerAdFailedToLoad();

        void OnDieRewardLoaded();
        void OnDieRewardShow();
        void OnDieRewardSkipped();
        void OnDieRewardDismiss();
        void OnDieRewardDidReward();
        void OnDieRewardFailedToLoad();
        
        void OnTimeUpRewardLoaded();
        void OnTimeUpRewardShow();
        void OnTimeUpRewardSkipped();
        void OnTimeUpRewardDismiss();
        void OnTimeUpRewardDidReward();
        void OnTimeUpRewardFailedToLoad();
    }

    public class AdvertisementSystem: IAdvertisementDelegate
    {
        private Dictionary<string, IAdvertisement> _managedSystem = new Dictionary<string, IAdvertisement>();

        private bool _enableAdvertise;
        public bool EnableAdvertise{
            set{
                _enableAdvertise = value;
            }
        }

        private List<AdUnit> _adUnitList = new List<AdUnit> ();
        private List<AdPlacementData> _adPlacement = new List<AdPlacementData> ();
        private List<string> _disablePlacement = new List<string> ();

        private IAdvertisementDelegate _delegate;
        public void SetDelegate(IAdvertisementDelegate callback)
        {
            _delegate = callback;
        }

        public void LoadConfig( AdvertisementConfig config )
        {
            if (config == null)
                return;
            if (config.listAds != null && config.listAds.Count > 0) {
                _adUnitList = new List<AdUnit> (config.listAds);
            }
            if (config.listPlacement != null && config.listPlacement.Count > 0) {
                _adPlacement = new List<AdPlacementData> (config.listPlacement);
            }
        }

        public void Run( )
        {
            if (!_enableAdvertise)
                return;

            foreach (var item in _managedSystem) {
                item.Value.Callback = this;
            }
            
            foreach (var item in _adUnitList) {
                var system = GetSystemWithProvider (item.Provider);
                if (system != null) {
                    if (!string.IsNullOrEmpty (item.AdId.NormalBannerAdId)) {
                        system.LoadNormalBanner(item.AdId.NormalBannerAdId, item.AdMeta.BannerAdPosition);	
                    }
                    if (!string.IsNullOrEmpty (item.AdId.EndGameBannerAdId)) {
                        system.LoadEndGameBanner(item.AdId.EndGameBannerAdId, item.AdMeta.BannerAdPosition);	
                    }
                    if (!string.IsNullOrEmpty (item.AdId.RewardTimeUpId)) {
                        system.LoadTimeUpRewardAds(item.AdId.RewardDieId);
                    }
                    if (!string.IsNullOrEmpty (item.AdId.RewardDieId)) {
                        system.LoadDieRewardAds(item.AdId.RewardDieId);
                    }
                }
            }
        }

        public void SetActivePlacement(string placement , bool isActive)
        {
            if (isActive) {
                _disablePlacement.Remove (placement);
            } else {
                if (!_disablePlacement.Contains (placement)) {
                    _disablePlacement.Add (placement);
                }
            }
        }

        public IAdvertisement GetSystemWithProvider(string provider)
        {
            foreach (var item in _managedSystem) {
                if (item.Value.HandleProvider == provider) {
                    return item.Value;
                }
            }

            return null;
        }

        public void ShowNormalBanner( string provider = "" )
        {
            if (!string.IsNullOrEmpty (provider)) {
                var adSys = GetSystemWithProvider (provider);
                adSys?.ShowNormalBanner();
            } else {
                foreach (var item in _managedSystem) {
                    item.Value.ShowNormalBanner();
                }
            }
        }

        public void HideNormalBanner( )
        {
            foreach (var item in _managedSystem) {
                item.Value.HideNormalBanner();
            }
        }
        
        public void ShowEndGameBanner( string provider = "" )
        {
            if (!string.IsNullOrEmpty (provider)) {
                var adSys = GetSystemWithProvider (provider);
                adSys?.ShowEndGameBanner();
            } else {
                foreach (var item in _managedSystem) {
                    item.Value.ShowEndGameBanner ();
                }
            }
        }

        public void HideEndGameBanner( )
        {
            foreach (var item in _managedSystem) {
                item.Value.HideEndGameBanner ();
            }
        }
        
        public bool IsDieRewardAdsReady{ 
            get{
                foreach (var item in _managedSystem) {
                    if (item.Value.IsDieRewardAdsReady) {
                        return true;
                    }
                }

                return false;
            }
        }

        public void ShowDieRewardAds ( string placement , bool forcedShow = false , string provider = "" )
        {
            if (!forcedShow) {
                if (!_enableAdvertise)
                    return;
            }
            if (_disablePlacement.Contains (placement))
                return;

            bool willShow = false;

            foreach (var place in _adPlacement) {
                if (place.PlacementName == placement) {
                    if (place.EnableShowByPercentage) {
                        place.CurrentFrequency = UnityEngine.Random.Range (0, 100);
                        if (place.CurrentFrequency <= place.Frequency) {
                            place.CurrentFrequency = 0;
                            willShow = true;
                            break;
                        }
                    } else {
                        place.CurrentFrequency += 1;
                        if (place.CurrentFrequency >= place.Frequency) {
                            place.CurrentFrequency = 0;
                            willShow = true;
                            break;
                        }
                    }
                    break;
                }
            }

            if (willShow) {
                bool isForcedProvider = !string.IsNullOrEmpty (provider);
                foreach (var item in _adUnitList) {
                    if (!string.IsNullOrEmpty (item.AdId.RewardDieId)) {
                        string adProvider = item.Provider;
                        if (isForcedProvider) {
                            if (item.Provider != provider) {
                                continue;
                            }
                        }

                        var system = GetSystemWithProvider (adProvider);
                        if (system != null && system.IsDieRewardAdsReady && !system.IsDieRewardAdShowing) {
                            //TODO: analytics
                            system.ShowDieRewardAds(item.AdId.RewardDieId);
                            break;
                        }
                    }
                }
            }
        }
        
        
        public bool IsTimeUpRewardAdsReady{ 
            get{
                foreach (var item in _managedSystem) {
                    if (item.Value.IsTimeUpRewardAdsReady) {
                        return true;
                    }
                }

                return false;
            }
        }

        public void ShowTimeUpRewardAds ( string placement , bool forcedShow = false , string provider = "" )
        {
            if (!forcedShow) {
                if (!_enableAdvertise)
                    return;
            }
            if (_disablePlacement.Contains (placement))
                return;

            bool willShow = false;

            foreach (var place in _adPlacement) {
                if (place.PlacementName == placement) {
                    if (place.EnableShowByPercentage) {
                        place.CurrentFrequency = UnityEngine.Random.Range (0, 100);
                        if (place.CurrentFrequency <= place.Frequency) {
                            place.CurrentFrequency = 0;
                            willShow = true;
                            break;
                        }
                    } else {
                        place.CurrentFrequency += 1;
                        if (place.CurrentFrequency >= place.Frequency) {
                            place.CurrentFrequency = 0;
                            willShow = true;
                            break;
                        }
                    }
                    break;
                }
            }

            if (willShow) {
                bool isForcedProvider = !string.IsNullOrEmpty (provider);
                foreach (var item in _adUnitList) {
                    if (!string.IsNullOrEmpty (item.AdId.RewardDieId)) {
                        string adProvider = item.Provider;
                        if (isForcedProvider) {
                            if (item.Provider != provider) {
                                continue;
                            }
                        }

                        var system = GetSystemWithProvider (adProvider);
                        if (system != null && system.IsTimeUpRewardAdsReady && !system.IsTimeUpRewardAdShowing) {
                            //TODO: analytics
                            system.ShowTimeUpRewardAds(item.AdId.RewardDieId);
                            break;
                        }
                    }
                }
            }
        }
        
        

        public void RemoveAds( )
        {
            foreach (var item in _managedSystem) {
                item.Value.RemoveAds ();
            }

            _enableAdvertise = false;
        }

        #region IAdvertisementDelegate implementation

        public void OnNormalBannerAdLoaded ()
        {
            if (!_enableAdvertise)
                return;
            foreach (var item in _adUnitList) {
                if (item.AdMeta.AutoShowBanner) {
                    var sys = GetSystemWithProvider (item.Provider);
                    if (sys.IsNormalBannerReady) {
                        sys.ShowNormalBanner ();
                    }
                }
            }

            _delegate?.OnNormalBannerAdLoaded ();
        }

        public void OnNormalBannerAdDismiss ()
        {
            _delegate?.OnNormalBannerAdDismiss ();
        }

        public void OnNormalBannerAdShow ()
        {
            _delegate?.OnNormalBannerAdShow ();
        }

        public void OnNormalBannerAdFailedToLoad ()
        {
            _delegate?.OnNormalBannerAdFailedToLoad ();
        }
        
        public void OnEndGameBannerAdLoaded ()
        {
            if (!_enableAdvertise)
                return;
            foreach (var item in _adUnitList) {
                if (item.AdMeta.AutoShowBanner) {
                    var sys = GetSystemWithProvider (item.Provider);
                    if (sys.IsEndGameBannerReady && !sys.IsEndGameBannerShowing) {
                        sys.ShowEndGameBanner ();
                    }
                }
            }

            _delegate?.OnEndGameBannerAdLoaded ();
        }

        public void OnEndGameBannerAdDismiss ()
        {
            _delegate?.OnEndGameBannerAdDismiss ();
        }

        public void OnEndGameBannerAdShow ()
        {
            _delegate?.OnEndGameBannerAdShow ();
        }

        public void OnEndGameBannerAdFailedToLoad ()
        {
            _delegate?.OnEndGameBannerAdFailedToLoad ();
        }

        public void OnDieRewardLoaded ()
        {
            _delegate?.OnDieRewardLoaded ();
        }

        public void OnDieRewardShow ()
        {
            _delegate?.OnDieRewardShow ();
        }

        public void OnDieRewardSkipped ()
        {
            //TODO: analytics 
            _delegate?.OnDieRewardSkipped ();
        }

        public void OnDieRewardDismiss ()
        {
            //TODO: analytics 
            _delegate?.OnDieRewardDismiss();
        }

        public void OnDieRewardDidReward ()
        {
            //TODO: analytics 
            _delegate?.OnDieRewardDidReward ();
        }

        public void OnDieRewardFailedToLoad ()
        {
            _delegate?.OnDieRewardFailedToLoad ();
        }
        
        public void OnTimeUpRewardLoaded ()
        {
            _delegate?.OnTimeUpRewardLoaded ();
        }

        public void OnTimeUpRewardShow ()
        {
            _delegate?.OnTimeUpRewardShow ();
        }

        public void OnTimeUpRewardSkipped ()
        {
            //TODO: analytics 
            _delegate?.OnTimeUpRewardSkipped ();
        }

        public void OnTimeUpRewardDismiss ()
        {
            //TODO: analytics 
            _delegate?.OnTimeUpRewardDismiss ();
        }

        public void OnTimeUpRewardDidReward ()
        {
            //TODO: analytics 
            _delegate?.OnTimeUpRewardDidReward ();
        }

        public void OnTimeUpRewardFailedToLoad ()
        {
            _delegate?.OnTimeUpRewardFailedToLoad ();
        }

        #endregion

        public AdvertisementSystem Add<TValue>()  where TValue : IAdvertisement , new()
        {    
            var system = new TValue ();
            string key = system.GetType().ToString();
            if (_managedSystem.ContainsKey (key)){
                Debug.LogErrorFormat ("System {0} already exist", key);
                return this;
            }
            _managedSystem.Add (key, system);
            return this;
        }
        
        public void Remove<TValue>()
        {
            string key = typeof(TValue).ToString ();
            _managedSystem.Remove (key);
        }
        
        public IAdvertisement Get<TValue>()
            where TValue : IAdvertisement
        {
            string key = typeof(TValue).ToString();
            if (_managedSystem.ContainsKey (key)) {
                return _managedSystem [key];
            }

            Debug.LogErrorFormat ("System {0} did not exits", key);
            return default(IAdvertisement);
        }
        
    }
}