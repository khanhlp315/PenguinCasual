using UnityEngine;
using UnityEngine.UI;

namespace Penguin
{
    public class ShadowTextUGUI : MonoBehaviour
    {
        public Text label;
        public Text shadow;

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