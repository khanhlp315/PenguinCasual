using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
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
            var currentVersion = Application.version.Split('.');
            RemoteConfig.FetchAsync(TimeSpan.Zero).ContinueWith((value) =>
            {
                MainThreadDispatcher.MainThreadDispatcher.Instance.Enqueue(() =>
                {
                    if (value.IsFaulted || value.IsCanceled)
                    {
                        OnInitializeDone?.Invoke();
                        // NativeDialogManager.Instance.ShowInitialConnectionErrorDialog(FetchConfig);
                        return;
                    }
                    RemoteConfig.ActivateFetched();
#if UNITY_IOS
                    var isMaintaining = RemoteConfig.GetValue(_config.IsMaintainingKeyForIOS).BooleanValue;
#elif  UNITY_ANDROID
                    var isMaintaining = RemoteConfig.GetValue(_config.IsMaintainingKeyForAndroid).BooleanValue;

#endif
                    Debug.Log(isMaintaining);
#if UNITY_IOS
                        var requiredVersion = RemoteConfig.GetValue(_config.VersionKeyForIOS).StringValue.Split('.');
#elif  UNITY_ANDROID
                        var requiredVersion = RemoteConfig.GetValue(_config.VersionKeyForAndroid).StringValue.Split('.');
#endif
                    if (isMaintaining)
                    {
#if UNITY_IOS
                        var title = RemoteConfig.GetValue(_config.MaintenanceTitleKeyForIOS).StringValue; 
                        var message = RemoteConfig.GetValue(_config.MaintenanceMessageKeyForIOS).StringValue;
#elif  UNITY_ANDROID
                        var title = RemoteConfig.GetValue(_config.MaintenanceTitleKeyForAndroid).StringValue;
                        var message = RemoteConfig.GetValue(_config.MaintenanceMessageKeyForAndroid).StringValue;
#endif
                        NativeDialogManager.Instance.ShowMaintenanceDialog(title,Regex.Unescape(message), () =>
                        {
                            for (int i = 0; i < currentVersion.Length; ++i)
                            {
                                Debug.Log(currentVersion[i] + ' ' + requiredVersion[i]);
                                if (int.Parse(currentVersion[i]) == int.Parse(requiredVersion[i])) continue;
                                if (int.Parse(currentVersion[i]) > int.Parse(requiredVersion[i])) break;
                                NativeDialogManager.Instance.ShowUpdateRequestDialog(() =>
                                {
#if UNITY_ANDROID
                                    Application.OpenURL($"market://details?id={_config.AndroidAppId}");
#elif UNITY_IOS
                                    Application.OpenURL($"itms-apps://itunes.apple.com/app/id{_config.IosAppId}");
#endif
                                    Application.Quit();
                                });
                                return;
                            }
                            Application.Quit();
                        });
                        return;
                    }
                    
                    for (int i = 0; i < currentVersion.Length; ++i)
                    {
                        Debug.Log(currentVersion[i] + ' ' + requiredVersion[i]);
                        if (int.Parse(currentVersion[i]) == int.Parse(requiredVersion[i])) continue;
                        if (int.Parse(currentVersion[i]) > int.Parse(requiredVersion[i])) break;
                        NativeDialogManager.Instance.ShowUpdateRequestDialog(() =>
                        {
#if UNITY_ANDROID
                            Application.OpenURL($"market://details?id={_config.AndroidAppId}");
#elif UNITY_IOS
                            Application.OpenURL($"itms-apps://itunes.apple.com/app/id{_config.IosAppId}");
#endif
                            Application.Quit();
                        });
                        return;
                    }
                

                    OnInitializeDone();
                });
            });

        }
    }
}