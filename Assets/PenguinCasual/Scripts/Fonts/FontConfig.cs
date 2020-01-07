using TMPro;
using UnityEngine;

namespace Penguin.Fonts
{
    [CreateAssetMenu(fileName = "FontConfig.asset", menuName = "Penguin/Settings/Font Setting", order = 10)]

    public class FontConfig : ScriptableObject
    {
        public string AndroidFont = "Noto sans CJK JP";
        public string IosFont = "ヒラギノ角ゴ Pro N";
        public TMP_FontAsset EditorFont;

        public TMP_FontAsset EnglishFont;
        public TMP_FontAsset CounterFont;
        public TMP_FontAsset RankingFont;
        public TMP_FontAsset OthersFont;
    }
}