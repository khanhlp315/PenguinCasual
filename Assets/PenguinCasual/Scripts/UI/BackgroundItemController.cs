using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Penguin.UI
{
    public class BackgroundItemController: MonoBehaviour
    {
        [HideInInspector]
        public Sprite Avatar;
        
        [HideInInspector] 
        public bool IsLocked;
        
        [SerializeField]
        private Image _avatar;
        
        [SerializeField] 
        private GameObject _lockLayer;

        private void Start()
        {
            _avatar.sprite = Avatar;
            _lockLayer.SetActive(IsLocked);
        }
    }
}