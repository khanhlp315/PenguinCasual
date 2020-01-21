using UnityEngine;

namespace Penguin.AppConfigs
{
    [CreateAssetMenu(fileName = "AppConfig.asset", menuName = "Penguin/Settings/App Setting", order = 10)]
    public class AppConfig: ScriptableObject
    {
        [SerializeField]
        private string _isMaintainingKeyForIOS;
        [SerializeField]
        private string _maintenanceTitleKeyForIOS;
        [SerializeField]
        private string _maintenanceMessageKeyForIOS;
        [SerializeField]
        private string _versionKeyForIOS;
        [SerializeField]
        private string _isMaintainingKeyForAndroid;
        [SerializeField]
        private string _maintenanceTitleKeyForAndroid;
        [SerializeField]
        private string _maintenanceMessageKeyForAndroid;
        [SerializeField]
        private string _versionKeyForAndroid;
        [SerializeField]
        private string _iosAppId;
        [SerializeField]
        private string _androidAppId;

        public string IsMaintainingKey
        {
            get
            {
#if UNITY_IOS
                    return _isMaintainingKeyForIOS;
#else 
                return _isMaintainingKeyForAndroid;
#endif
            }
        }
        
        public string MaintenanceTitleKey
        {
            get
            {
#if UNITY_IOS
                return _maintenanceTitleKeyForIOS;
#else 
                return _maintenanceTitleKeyForAndroid;
#endif
            }
        }
        
        public string MaintenanceMessageKey
        {
            get
            {
#if UNITY_IOS
                    return _maintenanceMessageKeyForIOS;
#else 
                return _maintenanceMessageKeyForAndroid;
#endif
            }
        }
        
        public string VersionKey
        {
            get
            {
#if UNITY_IOS
                    return _versionKeyForIOS;
#else 
                return _versionKeyForAndroid;
#endif
            }
        }

        public string AppLink
        {
            get
            {
#if UNITY_IOS
                return $"itms-apps://itunes.apple.com/app/id{_iosAppId}";
#else 
                return $"market://details?id={_androidAppId}";
#endif
            }
        }
    }
}