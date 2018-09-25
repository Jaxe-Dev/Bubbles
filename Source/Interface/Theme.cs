using UnityEngine;
using Verse;

namespace Bubbles.Interface
{
    internal static class Theme
    {
        private static readonly GUIStyle BaseFontStyle = new GUIStyle(Text.fontStyles[(int) GameFont.Medium]) { alignment = TextAnchor.MiddleCenter, clipping = TextClipping.Clip, padding = new RectOffset(0, 0, 0, 0) };

        public static bool Activated { get; set; } = true;
        public static bool DoAnimals { get; set; } = true;
        public static bool DoNonPlayer { get; set; } = true;
        public static bool DoInjuries { get; set; } = false;
        public static bool DoSound { get; set; } = false;

        public static int MaxZoom { get => MinScale; set => MinScale = value; } // Backcompat
        public static int MinScale { get; set; } = 35;
        public static int MaxWidth { get; set; } = 250;
        public static int Spacing { get; set; } = 2;
        public static int StartOffset { get; set; } = 15;
        public static int OffsetDirection { get; set; } = 0;
        public static int StartOpacity { get; set; } = 90;
        public static int MouseOverOpacity { get; set; } = 20;
        public static int MinTime { get; set; } = 0;
        public static int FadeStart { get; set; } = 500;
        public static int FadeLength { get; set; } = 250;
        public static int MaxPerPawn { get; set; } = 3;

        public static int FontSize { get; set; } = 12;
        public static int PaddingX { get; set; } = 4;
        public static int PaddingY { get; set; } = 2;

        public static Color BackColor { get; set; } = Color.white;
        public static Color ForeColor { get; set; } = Color.black;
        public static Color SelectedBackColor { get; set; } = new Color(1f, 1f, 0.75f);
        public static Color SelectedForeColor { get; set; } = Color.black;

        public static GUIStyle GetFont(float scale) => new GUIStyle(BaseFontStyle) { fontSize = Mathf.CeilToInt(FontSize * scale) };

        public static Rot4 GetOffsetDirection() => new Rot4(OffsetDirection);
    }
}
