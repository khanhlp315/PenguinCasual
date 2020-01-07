using System.Linq;
using Penguin.Utilities;
using TMPro;
using UnityEngine;

namespace Penguin.Fonts
{
    public class FontManager: MonoSingleton<FontManager, FontConfig>
    {
        private TMP_FontAsset _fontAsset;

        public TMP_FontAsset SystemFont => _fontAsset;

        public TMP_FontAsset EnglishFont => _config.EnglishFont;
        public TMP_FontAsset CounterFont => _config.CounterFont;
        public TMP_FontAsset OthersFont => _config.OthersFont;
        public TMP_FontAsset RankingFont => _config.RankingFont;

        
        public override void Initialize()
        {
            string[] fontPaths = Font.GetPathsToOSFonts();
            string fontName =
#if UNITY_EDITOR
                    null
                    #else
#if UNITY_IOS
                    _config.IosFont
#endif
#if UNITY_ANDROID
                    _config.AndroidFont
#endif
#endif
                ;
            if (fontName == null)
            {
                _fontAsset = _config.EditorFont;
                OnInitializeDone?.Invoke();
                return;
            }
            foreach(var font in fontPaths)
            {
                Debug.Log(font);
            }
            var osFont = new Font(fontPaths.FirstOrDefault(f => f.Contains(fontName)));
            _fontAsset = TMP_FontAsset.CreateFontAsset(osFont);
            OnInitializeDone?.Invoke();

        }
    }
}