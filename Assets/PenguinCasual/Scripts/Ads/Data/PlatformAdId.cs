using Penguin.Ads.Interface;
using UnityEngine;

namespace Penguin.Ads.Data
{
    [System.Serializable]
    public class PlatformAdId : IPlatformAdId
    {
        [Header("IOS")]
        [SerializeField]
        private string _iosNormalBannerAdId;
        [SerializeField]
        private string _iosEndGameBannerAdId;
        [SerializeField]
        private string _iosRewardTimeUpId;
        [SerializeField]
        private string _iosRewardDieId;

        [Header("Android")]
        [SerializeField]
        private string _androidNormalBannerAdId;
        [SerializeField]
        private string _androidEndGameBannerAdId;
        [SerializeField]
        private string _androidRewardTimeUpId;
        [SerializeField]
        private string _androidRewardDieId;
		
        #region IPlatformAdId implementation
        public string NormalBannerAdId {
            get{
                if (Application.platform == RuntimePlatform.IPhonePlayer) {
                    return _iosNormalBannerAdId;
                } else if (Application.platform == RuntimePlatform.Android) {
                    return _androidNormalBannerAdId;
                }

                return _iosNormalBannerAdId;
            }
        }
        public string EndGameBannerAdId {
            get{
                if (Application.platform == RuntimePlatform.IPhonePlayer) {
                    return _iosEndGameBannerAdId;
                } else if (Application.platform == RuntimePlatform.Android) {
                    return _androidEndGameBannerAdId;
                }

                return _iosEndGameBannerAdId;
            }
        }
        public string RewardTimeUpId {
            get{
                if (Application.platform == RuntimePlatform.IPhonePlayer) {
                    return _iosRewardTimeUpId;
                } else if (Application.platform == RuntimePlatform.Android) {
                    return _androidRewardTimeUpId;
                }

                return _iosRewardTimeUpId;
            }
        }
        public string RewardDieId {
            get{
                if (Application.platform == RuntimePlatform.IPhonePlayer) {
                    return _iosRewardDieId;
                } else if (Application.platform == RuntimePlatform.Android) {
                    return _androidRewardDieId;
                }

                return _iosRewardDieId;
            }
        }
        #endregion
    }
}