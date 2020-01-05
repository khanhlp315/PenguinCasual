using System;
using Penguin.Network.Data;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Penguin.UI
{
    public class CharacterInfoPanel: MonoBehaviour
    {
        [SerializeField]
        private Image _avatar;
        
        [SerializeField]
        private Text _nameText;
        
        [SerializeField]
        private Text _descriptionText;

        [HideInInspector]
        public SkinData SkinData;

        public UnityAction<int> OnCharacterSelect;

        
        private void OnEnable()
        {
            _nameText.text = SkinData.Name;
            _descriptionText.text = SkinData.Introduction;
        }

        public void SetAvatar(Sprite sprite)
        {
            _avatar.sprite = sprite;
        }

        public void Show()
        {
            this.gameObject.SetActive(true);
        }

        public void Hide()
        {
            this.gameObject.SetActive(false);
        }

        public void OnSelect()
        {
            OnCharacterSelect?.Invoke(SkinData.Id);
        }

        public void OnCancel()
        {
            this.gameObject.SetActive(false);
        }
    }
}