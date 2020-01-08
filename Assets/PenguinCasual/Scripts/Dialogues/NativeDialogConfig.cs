using UnityEngine;

namespace Penguin.Dialogues
{
    [CreateAssetMenu(fileName = "NativeDialogConfig.asset", menuName = "Penguin/Settings/Native Dialog Setting", order = 10)]

    public class NativeDialogConfig: ScriptableObject
    {
        public string ConnectionErrorTitle = "通信エラー";
        public string ConnectionErrorBody = "通信に失敗しました。\nインタネット接続を確認して、\n再度お試しください。\n※上手く動作しない場合は、\nアプリを再起動してください。";
        public string RetryText = "リトライ";
        public string CancelText = "不要";
        public string OkText = "Ok";
        public string ScoreUpdateErrorTitle = "警告";
        public string ScoreUpdateErrorBody = "SCOREを更新できません。";
        public string ChangeNameSuccessTitle = "通知";
        public string ChangeNameSuccessBody = "ニックネームの変更が成功でした。";
        public string ChangeNameValidationErrorTitle = "警告";

    }
}