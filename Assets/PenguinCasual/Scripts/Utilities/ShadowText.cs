using TMPro;
using UnityEngine;

namespace Penguin
{
    public class ShadowText : MonoBehaviour
    {
        public TextMeshProUGUI label;
        public TextMeshProUGUI shadow;

        public string text
        {
            get
            {
                return label.text;
            }
            set
            {
                label.text = shadow.text = value;
            }
        }
    }
}