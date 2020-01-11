using UnityEngine;

namespace Penguin.AppConfigs
{
    [CreateAssetMenu(fileName = "AppConfig.asset", menuName = "Penguin/Settings/App Setting", order = 10)]
    public class AppConfig: ScriptableObject
    {
        public string IsMaintainingKeyForIOS;
        public string MaintenanceMessageKeyForIOS;
        public string IsMaintainingKeyForAndroid;
        public string MaintenanceMessageKeyForAndroid;

    }
}