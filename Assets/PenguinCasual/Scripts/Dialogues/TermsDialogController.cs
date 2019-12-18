using UnityEngine;
using UnityEngine.UI;

namespace Penguin.Dialogues
{
    public class TermsDialogController : MonoBehaviour
    {
        [SerializeField] private Text _text;
        [SerializeField] private string _androidTermsPath;
        [SerializeField] private string _iosTermsPath;
        private void Start()
        {
#if UNITY_ANDROID
            _text.text = Resources.Load<TextAsset>(_androidTermsPath).text;
#elif UNITY_IOS
            _text.text = Resources.Load<TextAsset>(_iosTermsPath).text;
#endif
        }

        public void Close()
        {
            this.gameObject.SetActive(false);
        }
    }
}