using System;
using Penguin.AppConfigs;
using Penguin.Dialogues;
using PenguinCasual.Scripts.Utilities;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Penguin.Scenes
{
    public class MaintenanceOrUpdateScene: MonoBehaviour
    {
        private bool _isUpdateDialogShowing = false;
        private bool _isMaintenanceDialogShowing = false;
        private void Start()
        {
            Check();
        }

#if UNITY_IOS || UNITY_EDITOR
        private void OnApplicationFocus(bool hasFocus)
        {
            if (hasFocus)
            {
                Check();
            }
        }
#endif

#if UNITY_ANDROID || UNITY_EDITOR
        private void OnApplicationPause(bool pauseStatus)
        {
            Debug.Log(pauseStatus);
            if (!pauseStatus)
            {
                Check();
            }
        }
#endif

        private void Check()
        {
            if (_isUpdateDialogShowing || _isMaintenanceDialogShowing)
            {
                return;
            }
            var needsUpdate = AppConfigManager.Instance.NeedsUpdate;
            var isMaintaining = AppConfigManager.Instance.IsMaintaining;

            if (isMaintaining)
            {
                _isMaintenanceDialogShowing = true;
                AppConfigManager.Instance.ShowMaintenanceDialog(!needsUpdate, () =>
                {
                    _isMaintenanceDialogShowing = false;
                    if (!needsUpdate) return;
                    _isUpdateDialogShowing = true;
                    AppConfigManager.Instance.ShowUpdateDialog(() => { _isUpdateDialogShowing = false; });
                });
            }
            else if (needsUpdate)
            {
                _isUpdateDialogShowing = true;
                AppConfigManager.Instance.ShowUpdateDialog(() => { _isUpdateDialogShowing = false; });
            }
            else
            {
                var hasAcceptTerms = !PlayerPrefsHelper.IsFirstTimeUser();
                SceneManager.LoadScene(hasAcceptTerms ? "HomeScene" : 
#if UNITY_IOS
                        "AcceptTermsSceneForIOS"
#else
                "AcceptTermsSceneForAndroid"
#endif
                );
            }
        }
    }
}