using UnityEngine;

namespace Bubbles.Interface
{
    // Legacy for old RimHUD. Should be removed next version
    internal static class Theme
    {
        public static bool Activated { get => Mod.Settings.Activated; set => Mod.Settings.Activated = value; }

        public static bool DoNonPlayer { get => Mod.Settings.DoNonPlayer; set => Mod.Settings.DoNonPlayer = value; }
        public static bool DoAnimals { get => Mod.Settings.DoAnimals; set => Mod.Settings.DoAnimals = value; }
        public static bool DoDrafted { get => Mod.Settings.DoDrafted; set => Mod.Settings.DoDrafted = value; }
        public static bool DoCombat { get => Mod.Settings.DoCombat; set => Mod.Settings.DoCombat = value; }

        public static int FontSize { get => Mod.Settings.FontSize; set => Mod.Settings.FontSize = value; }
        public static int PaddingX { get => Mod.Settings.PaddingX; set => Mod.Settings.PaddingX = value; }
        public static int PaddingY { get => Mod.Settings.PaddingY; set => Mod.Settings.PaddingY = value; }

        public static int ScaleStart { get => Mod.Settings.ScaleStart; set => Mod.Settings.ScaleStart = value; }
        public static int MinScale { get => Mod.Settings.ScaleMin; set => Mod.Settings.ScaleMin = value; }
        public static int MaxWidth { get => Mod.Settings.WidthMax; set => Mod.Settings.WidthMax = value; }
        public static int Spacing { get => Mod.Settings.Spacing; set => Mod.Settings.Spacing = value; }
        public static int StartOffset { get => Mod.Settings.OffsetStart; set => Mod.Settings.OffsetStart = value; }
        public static int OffsetDirection { get => Mod.Settings.OffsetDirection; set => Mod.Settings.OffsetDirection = value; }
        public static int StartOpacity { get => Mod.Settings.OpacityStart; set => Mod.Settings.OpacityStart = value; }
        public static int MouseOverOpacity { get => Mod.Settings.OpacityMouseOver; set => Mod.Settings.OpacityMouseOver = value; }
        public static int MinTime { get => Mod.Settings.TimeMin; set => Mod.Settings.TimeMin = value; }
        public static int FadeStart { get => Mod.Settings.FadeStart; set => Mod.Settings.FadeStart = value; }
        public static int FadeLength { get => Mod.Settings.FadeLength; set => Mod.Settings.FadeLength = value; }
        public static int MaxPerPawn { get => Mod.Settings.PawnMax; set => Mod.Settings.PawnMax = value; }

        public static Color BackColor { get => Mod.Settings.BackColor; set => Mod.Settings.BackColor = value; }
        public static Color ForeColor { get => Mod.Settings.ForeColor; set => Mod.Settings.ForeColor = value; }
        public static Color SelectedForeColor { get => Mod.Settings.ForeColorSelected; set => Mod.Settings.ForeColorSelected = value; }
        public static Color SelectedBackColor { get => Mod.Settings.BackColorSelected; set => Mod.Settings.BackColorSelected = value; }

        public static Color CombatForeColor { get => Mod.Settings.ForeColorCombat; set => Mod.Settings.ForeColorCombat = value; }
        public static Color CombatBackColor { get => Mod.Settings.BackColorCombat; set => Mod.Settings.BackColorCombat = value; }
        public static Color CombatSelectedForeColor { get => Mod.Settings.ForeColorCombatSelected; set => Mod.Settings.ForeColorCombatSelected = value; }
        public static Color CombatSelectedBackColor { get => Mod.Settings.BackColorCombatSelected; set => Mod.Settings.BackColorCombatSelected = value; }
    }
}
