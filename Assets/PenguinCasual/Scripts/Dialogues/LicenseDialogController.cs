using UnityEngine;
using UnityEngine.UI;

namespace Penguin.Dialogues
{
    public class LicenseDialogController : MonoBehaviour
    {
        [SerializeField] private Text _text;
        [SerializeField] private string _licensePath;
        private void Start()
        {
            _text.text = Resources.Load<TextAsset>(_licensePath).text;
        }

        public void Close()
        {
            this.gameObject.SetActive(false);
        }
    }
}