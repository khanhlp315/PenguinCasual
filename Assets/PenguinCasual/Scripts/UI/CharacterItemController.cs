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
        public UnityAction OnSelected;

        [SerializeField]
        private Image _avatar;

        [SerializeField] private GameObject _selectLayer;
        [SerializeField] private GameObject _unSelectLayer;
        [SerializeField] private GameObject _lockLayer;
        
        private void Start()
        {
            Debug.Log(IsLocked);
            _avatar.sprite = Avatar;
            _lockLayer.SetActive(false);
            _selectLayer.SetActive(false);
            _unSelectLayer.SetActive(false);
            if (IsLocked)
            {
                _lockLayer.SetActive(true);
            }
            else if (IsSelected)
            {
                _selectLayer.SetActive(true);
            }
            else
            {
                _unSelectLayer.SetActive(true);
            }
            _lockLayer.SetActive(IsLocked);
        }

        public void OnTap()
        {
            OnSelected?.Invoke();
        }

        public void Reload()
        {
            _avatar.sprite = Avatar;
            _lockLayer.SetActive(false);
            _selectLayer.SetActive(false);
            _unSelectLayer.SetActive(false);
            if (IsLocked)
            {
                _lockLayer.SetActive(true);
            }
            else if (IsSelected)
            {
                _selectLayer.SetActive(true);
            }
            else
            {
                _unSelectLayer.SetActive(true);
            }            
            _lockLayer.SetActive(IsLocked);            
        }
    }
}