using Penguin.Utilities;
using pingak9;
using UnityEngine;
using UnityEngine.Events;

namespace Penguin.Dialogues
{
    public class NativeDialogManager: MonoSingleton<NativeDialogManager, NativeDialogConfig>
    {
        public override void Initialize()
        {
            OnInitializeDone?.Invoke();
        }

        public void ShowConnectionErrorDialog(UnityAction onRetry, UnityAction onCancel)
        {
            #if UNITY_EDITOR
            Debug.Log("Show connection error dialog");
#else
            NativeDialog.OpenDialog(_config.ConnectionErrorTitle, _config.ConnectionErrorBody, _config.RetryText, _config.CancelText,
                () => { onRetry?.Invoke(); },
                () => { onCancel?.Invoke(); });
#endif
        }

        public void ShowMaintenanceDialog(string message, UnityAction onOk)
        {
#if UNITY_EDITOR
            Debug.Log("Show maintenance dialog: " +message);
#else
            NativeDialog.OpenDialog("", message, _config.OkText,
                () => { onOk?.Invoke(); });
#endif
        }
        
        public void ShowInitialConnectionErrorDialog(UnityAction onRetry)
        {
#if UNITY_EDITOR

            Debug.Log("Show connection error dialog");
            Debug.Log(_config.ConnectionErrorTitle);
            Debug.Log(_config.ConnectionErrorBody);
#else
            NativeDialog.OpenDialog(_config.ConnectionErrorTitle, _config.ConnectionErrorBody, _config.RetryText,
                () => { onRetry?.Invoke(); });
#endif
        }
        
        public void ShowScoreUpdateErrorDialog()
        {
#if UNITY_EDITOR
            Debug.Log("Show score update error dialog");
#else
                    NativeDialog.OpenDialog(_config.ScoreUpdateErrorTitle,
                        _config.ScoreUpdateErrorBody, _config.OkText,
                        () => { });

#endif
        }
        
        public void ShowChangeNameSuccessDialog()
        {
#if UNITY_EDITOR
            Debug.Log("Show change name success dialog");
#else
                    NativeDialog.OpenDialog(_config.ChangeNameSuccessTitle,
                        _config.ChangeNameSuccessBody, _config.OkText,
                        () => { });
#endif
        }

        public void ShowChangeNameValidationError(string message)
        {
#if UNITY_EDITOR
            Debug.Log("Show change name validation error dialog");
#else
                    NativeDialog.OpenDialog(_config.ChangeNameValidationErrorTitle,
                        message, _config.OkText,
                        () => { });
#endif
        }
    }
}