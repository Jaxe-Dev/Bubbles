using Bubbles.Interface;
using UnityEngine;
using Verse;

namespace Bubbles
{
    internal class Settings : ModSettings
    {
        private const bool DefaultActivated = true;

        private const bool DefaultDoNonPlayer = true;
        private const bool DefaultDoAnimals = true;
        private const bool DefaultDoDrafted = true;
        private const bool DefaultDoCombat = false;

        private const int DefaultScaleStart = 100;
        private const int DefaultScaleMin = 35;
        private const int DefaultWidthMax = 250;
        private const int DefaultSpacing = 2;
        private const int DefaultOffsetStart = 15;
        private const int DefaultOffsetDirection = 0;

        private const int DefaultOpacityStart = 90;
        private const int DefaultOpacityMouseOver = 20;
        private const int DefaultTimeMin = 0;
        private const int DefaultFadeStart = 500;
        private const int DefaultFadeLength = 250;
        private const int DefaultPawnMax = 3;

        private const int DefaultFontSize = 12;
        private const int DefaultPaddingX = 4;
        private const int DefaultPaddingY = 2;

        private static readonly Color DefaultBackColor = Color.white;
        private static readonly Color DefaultForeColor = Color.black;
        private static readonly Color DefaultForeColorSelected = Color.black;
        private static readonly Color DefaultBackColorSelected = new Color(1f, 1f, 0.75f);

        private static readonly Color DefaultForeColorCombat = Color.black;
        private static readonly Color DefaultBackColorCombat = new Color(1f, 0.3f, 0.3f);
        private static readonly Color DefaultForeColorCombatSelected = Color.black;
        private static readonly Color DefaultBackColorCombatSelected = new Color(1f, 0.5f, 0.5f);

//        private static readonly GUIStyle BaseFontStyle = new GUIStyle(Text.fontStyles[(int) GameFont.Medium]) { alignment = TextAnchor.MiddleCenter, clipping = TextClipping.Clip, padding = new RectOffset(0, 0, 0, 0) };

        public bool Activated = DefaultActivated;

        public bool DoNonPlayer = DefaultDoNonPlayer;
        public bool DoAnimals = DefaultDoAnimals;
        public bool DoDrafted = DefaultDoDrafted;
        public bool DoCombat = DefaultDoCombat;

        public int FontSize = DefaultFontSize;
        public int PaddingX = DefaultPaddingX;
        public int PaddingY = DefaultPaddingY;

        public int ScaleStart = DefaultScaleStart;
        public int ScaleMin = DefaultScaleMin;
        public int WidthMax = DefaultWidthMax;
        public int Spacing = DefaultSpacing;
        public int OffsetStart = DefaultOffsetStart;
        public int OffsetDirection = DefaultOffsetDirection;

        public int OpacityStart = DefaultOpacityStart;
        public int OpacityMouseOver = DefaultOpacityMouseOver;
        public int TimeMin = DefaultTimeMin;
        public int FadeStart = DefaultFadeStart;
        public int FadeLength = DefaultFadeLength;
        public int PawnMax = DefaultPawnMax;

        public Color BackColor = DefaultBackColor;
        public Color ForeColor = DefaultForeColor;
        public Color ForeColorSelected = DefaultForeColorSelected;
        public Color BackColorSelected = DefaultBackColorSelected;

        public Color ForeColorCombat = DefaultForeColorCombat;
        public Color BackColorCombat = DefaultBackColorCombat;
        public Color ForeColorCombatSelected = DefaultForeColorCombatSelected;
        public Color BackColorCombatSelected = DefaultBackColorCombatSelected;

        private string _bufferBackColor;
        private string _bufferForeColor;
        private string _bufferForeColorSelected;
        private string _bufferBackColorSelected;

        private string _bufferForeColorCombat;
        private string _bufferBackColorCombat;
        private string _bufferForeColorCombatSelected;
        private string _bufferBackColorCombatSelected;

        private Vector2 _scrollPosition = Vector2.zero;
        private Rect _viewRect;

        public static GUIStyle GetFont(float scale) => new GUIStyle(Text.CurFontStyle)
        {
            alignment = TextAnchor.MiddleCenter,
            clipping = TextClipping.Clip,
            padding = new RectOffset(0, 0, 0, 0),
            fontSize = Mathf.CeilToInt(Bubbles.Mod.Settings.FontSize * scale)
        };

        public static Rot4 GetOffsetDirection() => new Rot4(Bubbles.Mod.Settings.OffsetDirection);

        public override void ExposeData()
        {
            Scribe_Values.Look(ref Activated, nameof(Activated), DefaultActivated);
            Scribe_Values.Look(ref DoNonPlayer, nameof(DoNonPlayer), DefaultDoNonPlayer);
            Scribe_Values.Look(ref DoAnimals, nameof(DoAnimals), DefaultDoAnimals);
            Scribe_Values.Look(ref DoDrafted, nameof(DoDrafted), DefaultDoDrafted);
            Scribe_Values.Look(ref DoCombat, nameof(DoCombat));

            Scribe_Values.Look(ref FontSize, nameof(FontSize), DefaultFontSize);
            Scribe_Values.Look(ref PaddingX, nameof(PaddingX), DefaultPaddingX);
            Scribe_Values.Look(ref PaddingY, nameof(PaddingY), DefaultPaddingY);

            Scribe_Values.Look(ref ScaleStart, nameof(ScaleStart), DefaultScaleStart);
            Scribe_Values.Look(ref ScaleMin, nameof(ScaleMin), DefaultScaleMin);
            Scribe_Values.Look(ref WidthMax, nameof(WidthMax), DefaultWidthMax);
            Scribe_Values.Look(ref Spacing, nameof(Spacing), DefaultSpacing);
            Scribe_Values.Look(ref OffsetStart, nameof(OffsetStart), DefaultOffsetStart);
            Scribe_Values.Look(ref OffsetDirection, nameof(OffsetDirection));
            Scribe_Values.Look(ref OpacityStart, nameof(OpacityStart), DefaultOpacityStart);
            Scribe_Values.Look(ref OpacityMouseOver, nameof(OpacityMouseOver), DefaultOpacityMouseOver);
            Scribe_Values.Look(ref TimeMin, nameof(TimeMin));
            Scribe_Values.Look(ref FadeStart, nameof(FadeStart), DefaultFadeStart);
            Scribe_Values.Look(ref FadeLength, nameof(FadeLength), DefaultFadeLength);
            Scribe_Values.Look(ref PawnMax, nameof(PawnMax), DefaultPawnMax);

            Scribe_Values.Look(ref ForeColor, nameof(ForeColor), DefaultForeColor);
            Scribe_Values.Look(ref BackColor, nameof(BackColor), DefaultBackColor);
            Scribe_Values.Look(ref ForeColorSelected, nameof(ForeColorSelected), DefaultForeColorSelected);
            Scribe_Values.Look(ref BackColorSelected, nameof(BackColorSelected), DefaultBackColorSelected);

            Scribe_Values.Look(ref ForeColorCombat, nameof(ForeColorCombat), DefaultForeColorCombat);
            Scribe_Values.Look(ref BackColorCombat, nameof(BackColorCombat), DefaultBackColorCombat);
            Scribe_Values.Look(ref ForeColorCombatSelected, nameof(ForeColorCombatSelected), DefaultForeColorCombatSelected);
            Scribe_Values.Look(ref BackColorCombatSelected, nameof(BackColorCombatSelected), DefaultBackColorCombatSelected);

            base.ExposeData();
        }

        public void DrawSettings(Rect rect)
        {
            var listingRect = new Rect(rect.x, rect.y + 40f, rect.width, rect.height - 40f);
            var listing = new ListingPlus();
            listing.Begin(rect);

            if (listing.ButtonText("Bubbles.ResetToDefault".Translate())) { Bubbles.Mod.Settings.Reset(); }

            listing.End();
            listing.BeginScrollView(listingRect, ref _scrollPosition, ref _viewRect);

            listing.CheckboxLabeled("Bubbles.DoNonPlayer".Translate(), ref DoNonPlayer);
            listing.CheckboxLabeled("Bubbles.DoAnimals".Translate(), ref DoAnimals);
            listing.CheckboxLabeled("Bubbles.DoDrafted".Translate(), ref DoDrafted);
            listing.CheckboxLabeled("Bubbles.DoCombat".Translate(), ref DoCombat);
            listing.Gap();

            listing.IntSlider("Bubbles.FontSize".Translate(), ref FontSize, 9, 30);
            listing.IntSlider("Bubbles.PaddingX".Translate(), ref PaddingX, 1, 20);
            listing.IntSlider("Bubbles.PaddingY".Translate(), ref PaddingY, 1, 20);
            listing.Gap();

            listing.IntSlider("Bubbles.ScaleStart".Translate(), ref ScaleStart, 0, 100, "{0}%");
            listing.IntSlider("Bubbles.ScaleMin".Translate(), ref ScaleMin, 10, 75, "{0}%");
            listing.IntSlider("Bubbles.WidthMax".Translate(), ref WidthMax, 100, 500);
            listing.IntSlider("Bubbles.Spacing".Translate(), ref Spacing, 2, 12);
            listing.IntSlider("Bubbles.OffsetStart".Translate(), ref OffsetStart, 0, 250);
            listing.IntSlider("Bubbles.OffsetDirection".Translate(), ref OffsetDirection, 0, 3);
            listing.Gap();

            listing.IntSlider("Bubbles.OpacityStart".Translate(), ref OpacityStart, 30, 100, "{0}%");
            listing.IntSlider("Bubbles.OpacityMouseOver".Translate(), ref OpacityMouseOver, 5, 100, "{0}%");
            listing.IntSlider("Bubbles.TimeMin".Translate(), ref TimeMin, 1, 2000);
            listing.IntSlider("Bubbles.FadeStart".Translate(), ref FadeStart, 1, 2000);
            listing.IntSlider("Bubbles.FadeLength".Translate(), ref FadeLength, 1, 2000);
            listing.IntSlider("Bubbles.PawnMax".Translate(), ref PawnMax, 1, 10);
            listing.Gap();

            listing.ColorEntry("Bubbles.ForeColor".Translate(), ref _bufferForeColor, ref ForeColor);
            listing.ColorEntry("Bubbles.BackColor".Translate(), ref _bufferBackColor, ref BackColor);
            listing.ColorEntry("Bubbles.ForeColorSelected".Translate(), ref _bufferForeColorSelected, ref ForeColorSelected);
            listing.ColorEntry("Bubbles.BackColorSelected".Translate(), ref _bufferBackColorSelected, ref BackColorSelected);
            listing.Gap();

            listing.ColorEntry("Bubbles.ForeColorCombat".Translate(), ref _bufferForeColorCombat, ref ForeColorCombat);
            listing.ColorEntry("Bubbles.BackColorCombat".Translate(), ref _bufferBackColorCombat, ref BackColorCombat);
            listing.ColorEntry("Bubbles.ForeColorCombatSelected".Translate(), ref _bufferForeColorCombatSelected, ref ForeColorCombatSelected);
            listing.ColorEntry("Bubbles.BackColorCombatSelected".Translate(), ref _bufferBackColorCombatSelected, ref BackColorCombatSelected);

            listing.EndScrollView(ref _viewRect);
        }

        private void Reset()
        {
            _bufferBackColor = null;
            _bufferForeColor = null;
            _bufferForeColorSelected = null;
            _bufferBackColorSelected = null;
            _bufferForeColorCombat = null;
            _bufferBackColorCombat = null;
            _bufferForeColorCombatSelected = null;
            _bufferBackColorCombatSelected = null;

            Activated = DefaultActivated;

            DoNonPlayer = DefaultDoNonPlayer;
            DoAnimals = DefaultDoAnimals;
            DoDrafted = DefaultDoDrafted;
            DoCombat = DefaultDoCombat;

            FontSize = DefaultFontSize;
            PaddingX = DefaultPaddingX;
            PaddingY = DefaultPaddingY;

            ScaleStart = DefaultScaleStart;
            ScaleMin = DefaultScaleMin;
            WidthMax = DefaultWidthMax;
            Spacing = DefaultSpacing;
            OffsetStart = DefaultOffsetStart;
            OffsetDirection = DefaultOffsetDirection;

            OpacityStart = DefaultOpacityStart;
            OpacityMouseOver = DefaultOpacityMouseOver;
            TimeMin = DefaultTimeMin;
            FadeStart = DefaultFadeStart;
            FadeLength = DefaultFadeLength;
            PawnMax = DefaultPawnMax;

            BackColor = DefaultBackColor;
            ForeColor = DefaultForeColor;
            ForeColorSelected = DefaultForeColorSelected;
            BackColorSelected = DefaultBackColorSelected;

            ForeColorCombat = DefaultForeColorCombat;
            BackColorCombat = DefaultBackColorCombat;
            ForeColorCombatSelected = DefaultForeColorCombatSelected;
            BackColorCombatSelected = DefaultBackColorCombatSelected;
        }
    }
}
