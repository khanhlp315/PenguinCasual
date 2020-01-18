using UnityEngine;

namespace Penguin.AppConfigs
{
    [CreateAssetMenu(fileName = "AppConfig.asset", menuName = "Penguin/Settings/App Setting", order = 10)]
    public class AppConfig: ScriptableObject
    {
        public string IsMaintainingKeyForIOS;
        public string MaintenanceTitleKeyForIOS;
        public string MaintenanceMessageKeyForIOS;
        public string VersionKeyForIOS;
        public string IsMaintainingKeyForAndroid;
        public string MaintenanceTitleKeyForAndroid;
        public string MaintenanceMessageKeyForAndroid;
        public string VersionKeyForAndroid;

        public string IosAppId;
        public string AndroidAppId;

    }
}