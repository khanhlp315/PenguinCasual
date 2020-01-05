using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Penguin.UI
{
    public class CharacterItemController: MonoBehaviour
    {
        [HideInInspector]
        public Sprite Avatar;

        [HideInInspector]
        public bool IsSelected;

        [HideInInspector] 
        public bool IsLocked;

        [HideInInspector] 
        public int Id;

        [HideInInspector] 
        public UnityAction<int> OnSelected;

        [SerializeField]
        private Image _avatar;
        
        [SerializeField]
        private Image _background;

        [SerializeField]
        private Sprite _notSelectedBackground;

        [SerializeField]
        private Sprite _selectedBackground;

        [SerializeField] 
        private GameObject _lockLayer;

        private void Start()
        {
            _avatar.sprite = Avatar;
            _background.sprite = IsSelected ? _selectedBackground : _notSelectedBackground;
            _lockLayer.SetActive(IsLocked);
        }

        public void OnTap()
        {
            if (!IsLocked && !IsSelected)
            {
                OnSelected?.Invoke(Id);
            }
        }

        public void Reload()
        {
            _avatar.sprite = Avatar;
            _background.sprite = IsSelected ? _selectedBackground : _notSelectedBackground;
            _lockLayer.SetActive(IsLocked);            
        }
    }
}