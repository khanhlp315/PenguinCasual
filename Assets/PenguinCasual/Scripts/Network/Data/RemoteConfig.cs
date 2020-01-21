using System;
using UnityEngine;

namespace Penguin.Network.Data
{
    [Serializable]
    public class RemoteConfig
    {
#if UNITY_ANDROID
        [SerializeField]
        private string AndroidMaintainingMessage;
        [SerializeField]
        private string AndroidMaintainingTitle;
        [SerializeField]
        private string AndroidRequiredVersion;
        [SerializeField]
        private string AndroidRequiredTitle;
        [SerializeField]
        private string AndroidRequiredMessage;
        [SerializeField]
        private int IsAndroidMaintaining;
#elif UNITY_IOS
        [SerializeField]
        private string IOSMaintainingMessage;
        [SerializeField]
        private string IOSMaintainingTitle;
        [SerializeField]
        private string IOSRequiredVersion;
        [SerializeField]
        private string IOSRequiredTitle;
        [SerializeField]
        private string IOSRequiredMessage;
        [SerializeField]
        private int IsIOSMaintaining;
#endif

        public string MaintainingMessage
        {
            get
            {
                #if UNITY_ANDROID
                return AndroidMaintainingMessage;
                #elif UNITY_IOS
                return IOSMaintainingMessage;
                #endif
            }
        }
        
        public string MaintainingTitle
        {
            get
            {
#if UNITY_ANDROID
                return AndroidMaintainingTitle;
#elif UNITY_IOS
                return IOSMaintainingTitle;
#endif
            }
        }
        
        public string RequiredVersion
        {
            get
            {
#if UNITY_ANDROID
                return AndroidRequiredVersion;
#elif UNITY_IOS
                return IOSRequiredVersion;
#endif
            }
        }
        
        public string UpdateRequiredTitle
        {
            get
            {
#if UNITY_ANDROID
                return AndroidRequiredTitle;
#elif UNITY_IOS
                return IOSRequiredTitle;
#endif
            }
        }
        
        public string UpdateRequiredMessage
        {
            get
            {
#if UNITY_ANDROID
                return AndroidRequiredMessage;
#elif UNITY_IOS
                return IOSRequiredMessage;
#endif
            }
        }
        
        public bool IsMaintaining
        {
            get
            {
#if UNITY_ANDROID
                return IsAndroidMaintaining != 0;
#elif UNITY_IOS
                return IsIOSMaintaining != 0;
#endif
            }
        }

        public bool NeedsUpdate
        {
            get
            {
                var currentVersion = Application.version.Split('.');
                var requiredVersion = RequiredVersion.Split('.');
                for (int i = 0; i < currentVersion.Length; ++i)
                {
                    if (int.Parse(currentVersion[i]) == int.Parse(requiredVersion[i])) continue;
                    if (int.Parse(currentVersion[i]) > int.Parse(requiredVersion[i])) break;
                    return true;
                }

                return false;
            }
        }
    }
    
    [Serializable]
    public class RemoteConfigResponse : ResponseBodyWithMultipleEntities<RemoteConfig>
    {
        public static RemoteConfigResponse FromJson(string jsonString)
        {
            return JsonUtility.FromJson<RemoteConfigResponse>(jsonString);
        }
    }
}