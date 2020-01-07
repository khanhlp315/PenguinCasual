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
        
    }
}