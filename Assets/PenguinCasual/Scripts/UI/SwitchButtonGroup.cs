using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Penguin.UI
{
    public class SwitchButtonGroup: MonoBehaviour
    {
        [SerializeField]
        public List<SwitchButton> _switchButtons;

        [HideInInspector]
        public UnityAction<string> OnButtonSelected;

        private void Start()
        {
            _switchButtons.ForEach((switchButton) => { switchButton.OnSelect += (buttonName) =>
            {
                foreach(var button in _switchButtons)
                {
                    button.IsActive = button.Name == buttonName;
                }
                OnButtonSelected?.Invoke(buttonName);
            }; });
        }

        public void SetButton(string selectedButtonName)
        {
            foreach (var button in _switchButtons)
            {
                button.IsActive = button.Name == selectedButtonName;
            }
        }
        
    }
}