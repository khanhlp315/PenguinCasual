using UnityEngine;

namespace Penguin.Dialogues
{
    [CreateAssetMenu(fileName = "NativeDialogConfig.asset", menuName = "Penguin/Settings/Native Dialog Setting", order = 10)]

    public class NativeDialogConfig: ScriptableObject
    {
        [TextArea]
        public string ConnectionErrorTitle = "通信エラー";
        [TextArea]
        public string ConnectionErrorBody = "通信に失敗しました。\nインタネット接続を確認して、\n再度お試しください。\n※上手く動作しない場合は、\nアプリを再起動してください。";
        [TextArea]
        public string RetryText = "リトライ";
        [TextArea]
        public string CancelText = "不要";
        [TextArea]
        public string OkText = "Ok";
        [TextArea]
        public string ScoreUpdateErrorTitle = "警告";
        [TextArea]
        public string ScoreUpdateErrorBody = "SCOREを更新できません。";
        [TextArea]
        public string ChangeNameSuccessTitle = "通知";
        [TextArea]
        public string ChangeNameSuccessBody = "ニックネームの変更が成功でした。";
        [TextArea]
        public string ChangeNameValidationErrorTitle = "警告";
        [TextArea]
        public string UpdateRequestBody = "Please Update app to latest version";
    }
}