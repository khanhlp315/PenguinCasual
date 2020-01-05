using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Penguin.UI
{
    [RequireComponent(typeof(Image))]
    public class SwitchButton: MonoBehaviour
    {
        [SerializeField] private Sprite _activeSprite;

        [SerializeField] private Sprite _inactiveSprite;

        [SerializeField] private Color _activeTextColor;
        [SerializeField] private Color _inactiveTextColor;

        [SerializeField] private Image _image;
        [SerializeField] private Text _text;
        [SerializeField] private string _name;

        public bool IsActive
        {
            get { return _image.sprite == _activeSprite; }
            set
            {
                _image.sprite = value ? _activeSprite : _inactiveSprite;
                _text.color = value ? _activeTextColor : _inactiveTextColor;
            }
        }

        public string Name => _name;

        [HideInInspector]
        public UnityAction<string> OnSelect;

        public void OnTap()
        {
            if (IsActive)
            {
                return;
            }
            OnSelect?.Invoke(Name);
        }
    }
}