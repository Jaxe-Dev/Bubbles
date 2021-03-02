using System;
using Bubbles.Patch;
using UnityEngine;
using Verse;

namespace Bubbles.Interface
{
    internal class Bubble
    {
        private readonly string _text;

        public int Width { get; private set; }
        public int Height { get; private set; }

        private readonly DateTime _timeStart;
        private int _tickStart;
        private readonly bool _combat;

        public Bubble(TaggedString text, bool combat)
        {
            _text = text.RawText.StripTags();

            _timeStart = DateTime.Now;
            _tickStart = -1;

            _combat = combat;
        }

        private float GetFade()
        {
            if (_tickStart == -1)
            {
                if ((DateTime.Now - _timeStart).TotalMilliseconds > Mod.Settings.TimeMin) { _tickStart = Find.TickManager.TicksAbs; }
                return Mod.Settings.OpacityStart.ToPercentageFloat();
            }

            var elasped = Find.TickManager.TicksAbs - _tickStart - Mod.Settings.FadeStart;

            if (elasped <= 0f) { return Mod.Settings.OpacityStart.ToPercentageFloat(); }
            if (elasped > Mod.Settings.FadeLength) { return 0f; }

            var fade = Mod.Settings.OpacityStart.ToPercentageFloat() * (1f - (elasped / (float) Mod.Settings.FadeLength));
            return elasped < 0 ? 1f : fade;
        }

        private Color GetColor(bool foreground, bool isSelected)
        {
            if (isSelected)
            {
                if (_combat) { return foreground ? Mod.Settings.ForeColorCombatSelected : Mod.Settings.BackColorCombatSelected; }
                return foreground ? Mod.Settings.ForeColorSelected : Mod.Settings.BackColorSelected;
            }

            if (_combat) { return foreground ? Mod.Settings.ForeColorCombat : Mod.Settings.BackColorCombat; }
            return foreground ? Mod.Settings.ForeColor : Mod.Settings.BackColor;
        }

        public bool Draw(Vector2 pos, bool isSelected, float scale)
        {
            var direction = Settings.GetOffsetDirection();

            var font = Settings.GetFont(scale);

            var paddingX = Mathf.CeilToInt(Mod.Settings.PaddingX * scale);
            var paddingY = Mathf.CeilToInt(Mod.Settings.PaddingY * scale);
            var maxWidth = Mathf.CeilToInt(Mod.Settings.WidthMax * scale);

            var content = new GUIContent(_text);

            Width = Mathf.CeilToInt(Mathf.Min(font.CalcSize(content).x + (paddingX * 2), maxWidth));
            Height = Mathf.CeilToInt(font.CalcHeight(content, Width - (paddingX * 2)) + (paddingY * 2));

            var posX = pos.x;
            var posY = pos.y;

            if (direction.IsHorizontal) { posY -= Height / 2f; }
            else { posX -= Width / 2f; }

            var outer = new Rect(Mathf.CeilToInt(posX), Mathf.CeilToInt(posY), Width, Height);
            var inner = outer.ContractedBy(paddingX, paddingY);

            var fade = Mathf.Min(GetFade(), Mouse.IsOver(outer) ? Mod.Settings.OpacityMouseOver.ToPercentageFloat() : 1f);
            if (fade <= 0f) { return false; }

            var backColor = GetColor(false, isSelected).WithAlpha(fade);
            var foreColor = GetColor(true, isSelected).WithAlpha(fade);

            var prevColor = GUI.color;

            GUI.color = backColor;
            Widgets.DrawAtlas(outer, Textures.Inner);

            GUI.color = foreColor;
            Widgets.DrawAtlas(outer, Textures.Outer);
            GUI.Label(inner, content, font);

            GUI.color = prevColor;

            return true;
        }
    }
}
