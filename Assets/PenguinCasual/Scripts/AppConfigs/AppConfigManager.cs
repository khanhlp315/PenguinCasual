using System;
using System.Collections.Generic;
using Penguin.Dialogues;
using Penguin.Utilities;
using UnityEngine;
using RemoteConfig = Firebase.RemoteConfig.FirebaseRemoteConfig;

namespace Penguin.AppConfigs
{
    public class AppConfigManager: MonoSingleton<AppConfigManager, AppConfig>
    {
        public override void Initialize()
        {
#if UNITY_IOS
            Dictionary<string, object> defaults = new Dictionary<string, object> {{_config.IsMaintainingKeyForIOS, false}};
#elif UNITY_ANDROID
            Dictionary<string, object> defaults = new Dictionary<string, object> {{_config.IsMaintainingKeyForAndroid, false}};
#endif
            RemoteConfig.SetDefaults(defaults);
            FetchConfig();
        }

        private void FetchConfig()
        {
            RemoteConfig.FetchAsync(TimeSpan.Zero).ContinueWith((value) =>
            {
                if (value.IsFaulted || value.IsCanceled)
                {
                    NativeDialogManager.Instance.ShowInitialConnectionErrorDialog(FetchConfig);
                    return;
                }
                RemoteConfig.ActivateFetched();
#if UNITY_IOS
                var isMaintaining = RemoteConfig.GetValue(_config.IsMaintainingKeyForIOS).BooleanValue;
#elif  UNITY_ANDROID
                                var isMaintaining = RemoteConfig.GetValue(_config.IsMaintainingKeyForAndroid).BooleanValue;

#endif
                if (isMaintaining)
                {
#if UNITY_IOS
                    var message = RemoteConfig.GetValue(_config.MaintenanceMessageKeyForIOS).StringValue;
#elif  UNITY_ANDROID
                    var message = RemoteConfig.GetValue(_config.MaintenanceMessageKeyForAndroid).StringValue;
#endif
                    NativeDialogManager.Instance.ShowMaintenanceDialog(message, Application.Quit);
                    return;
                }

                OnInitializeDone();
            });

        }
    }
}