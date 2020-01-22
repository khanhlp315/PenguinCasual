using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Penguin.Dialogues;
using Penguin.Network;
using Penguin.Network.Data;
using Penguin.Utilities;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace Penguin.AppConfigs
{
    public class AppConfigManager: MonoSingleton<AppConfigManager, AppConfig>
    {
        private RemoteConfig _remoteConfig;

        public bool NeedsUpdate => _remoteConfig.NeedsUpdate;
        public bool IsMaintaining => _remoteConfig.IsMaintaining;


        public override void Initialize()
        {
            FetchConfig();
        }

        private void CheckConfig(bool isInitializing = true)
        {
            
            var isMaintaining = _remoteConfig.IsMaintaining;
            var needsUpdate = _remoteConfig.NeedsUpdate;
            if (isMaintaining || needsUpdate)
            {
                SceneManager.LoadScene("MaintenanceOrUpdateScene");
                return;
            }
            if (isInitializing)
            {
                StartCoroutine(FetchConfigContinuosly());
                OnInitializeDone?.Invoke();
            }
        }

        private IEnumerator FetchConfigContinuosly()
        {
            yield return null;
            bool fetchCompleted = false;
            while (true)
            {
                fetchCompleted = false;
                NetworkCaller.Instance.GetConfig((config) =>
                {
                    _remoteConfig = config;
                    fetchCompleted = true;
                }, (responseCode) => { fetchCompleted = true; });
                while (!fetchCompleted)
                {
                    yield return null;
                }
            }
        }

        public void ShowUpdateDialog(UnityAction onClose)
        {
            NativeDialogManager.Instance.ShowUpdateRequestDialog(Regex.Unescape(_remoteConfig.UpdateRequiredTitle), Regex.Unescape(_remoteConfig.UpdateRequiredMessage),() =>
            {
                Application.OpenURL(_config.AppLink);
                onClose?.Invoke();
                Application.Quit();
            });
        }
        
        public void ShowMaintenanceDialog(bool willQuit, UnityAction onClose)
        {
            NativeDialogManager.Instance.ShowMaintenanceDialog(Regex.Unescape(_remoteConfig.MaintainingTitle), Regex.Unescape(_remoteConfig.MaintainingMessage),() =>
            {
                onClose?.Invoke();
                if (willQuit)
                {
                    Application.Quit();
                }
            });
        }
        
        


        private void FetchConfig()
        {
            NetworkCaller.Instance.GetConfig((config) =>
            {
                _remoteConfig = config;
                CheckConfig();
            }, (responseCode) =>
            {
                NativeDialogManager.Instance.ShowInitialConnectionErrorDialog(FetchConfig);
            });
        }
    }
}