using UnityEngine;
using UnityEngine.UI;

namespace Penguin.Dialogues
{
    public class TermsDialogController : MonoBehaviour
    {
        public void Close()
        {
            this.gameObject.SetActive(false);
        }
    }
}