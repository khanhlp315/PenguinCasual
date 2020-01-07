using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Penguin.Fonts
{
    public enum TextType
    {
        SYSTEM,
        ENGLISH,
        COUNTER,
        RANKING,
        OTHERS
    }
    [RequireComponent(typeof(TextMeshProUGUI))]
    public class FontSelector : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI _text;

        [SerializeField] private TextType _textType;
        void Start()
        {
            switch (_textType)
            {
                case TextType.SYSTEM:
                    _text.font = FontManager.Instance.SystemFont;
                    return;
                case TextType.ENGLISH:
                    _text.font = FontManager.Instance.EnglishFont;
                    return;
                case TextType.COUNTER:
                    _text.font = FontManager.Instance.CounterFont;
                    return;
                case TextType.RANKING:
                    _text.font = FontManager.Instance.RankingFont;
                    return;
                case TextType.OTHERS:
                    _text.font = FontManager.Instance.OthersFont;
                    return;
            }
            
        }

        #if UNITY_EDITOR
        [ContextMenu("Update font")]
        private void OnValidate()
        {
            var fontConfig = Resources.Load<FontConfig>("Databases/FontConfig");
            switch (_textType)
            {
                case TextType.SYSTEM:
                    _text.font = fontConfig.EditorFont;
                    return;
                case TextType.ENGLISH:
                    _text.font = fontConfig.EnglishFont;
                    return;
                case TextType.COUNTER:
                    _text.font = fontConfig.CounterFont;
                    return;
                case TextType.OTHERS:
                    _text.font = fontConfig.OthersFont;
                    return;
                case TextType.RANKING:
                    _text.font = fontConfig.RankingFont;
                    return;
            }
        }
        #endif

        private void Reset()
        {
            _text = GetComponent<TextMeshProUGUI>();

        }
    }   
}
