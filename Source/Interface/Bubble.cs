using System;
using Bubbles.Patch;
using UnityEngine;
using Verse;

namespace Bubbles.Interface
{
    internal class Bubble
    {
        private const int TextPadding = 2;

        public Pawn Pawn { get; }

        private readonly string _text;
        public int Height { get; }
        private int _index;

        private readonly int _rectHeight;
        private readonly DateTime _timeStart;
        private readonly int _tickStart;
        private int _offset;

        private Color BackColor { get; } = Color.white;
        private Color ForeColor { get; } = Color.black;

        public Bubble(Pawn pawn, string text, int index, int offset)
        {
            Pawn = pawn;

            _text = text;
            _rectHeight = Mathf.CeilToInt(Text.CalcHeight(_text, Bubbler.BubbleWidth + (Text.CurFontStyle.padding.horizontal * 2)) + (Text.CurFontStyle.padding.vertical * 2));
            Height = _rectHeight + Bubbler.BubblePadding + (TextPadding * 2);
            _index = index;
            _offset = offset;

            _timeStart = DateTime.Now;
            _tickStart = Find.TickManager.TicksAbs;
        }

        public void Pop(Bubble bubble)
        {
            if (bubble._index >= _index) { return; }

            _index -= 1;
            _offset -= bubble.Height;
        }

        private float GetFade(Rect rect)
        {
            int time;
            if (Bubbler.FadeMode == FadeBy.Time) { time = (int) (DateTime.Now - _timeStart).TotalMilliseconds; }
            else if (Bubbler.FadeMode == FadeBy.Tick) { time = Find.TickManager.TicksAbs - _tickStart; }
            else { throw new Mod.Exception($"Invalid value of {nameof(Bubbler.FadeMode)}"); }

            time -= Bubbler.FadeStart;

            if (time > Bubbler.FadeTime) { return 0f; }
            var fade = Mathf.Abs(1f - (time / (float) Bubbler.FadeTime));
            if (Mouse.IsOver(rect)) { return Mathf.Min(fade, 0.2f); }
            return time < 0 ? 1f : fade;
        }

        public bool Draw()
        {
            var pos = GenMapUI.LabelDrawPosFor(Pawn, -0.6f);

            var halfWidth = Bubbler.BubbleWidth / 2;
            var rect = new Rect(pos.x - halfWidth, pos.y + Text.fontStyles[(int) GameFont.Tiny].lineHeight + Bubbler.BubblePadding + _offset, Bubbler.BubbleWidth, _rectHeight);
            var textRect = rect.ContractedBy(2f);

            var fade = GetFade(rect);
            if (fade <= 0f) { return false; }

            var backColor = BackColor.WithAlpha(fade);
            var foreColor = ForeColor.WithAlpha(fade);

            var prevColor = GUI.color;
            var prevAnchor = Text.Anchor;
            var prevFont = Text.Font;

            Text.Anchor = TextAnchor.MiddleCenter;
            Text.Font = GameFont.Tiny;

            GUI.color = backColor;
            Widgets.DrawAtlas(rect, Textures.Inner);

            GUI.color = foreColor;
            Widgets.DrawAtlas(rect, Textures.Outer);
            Widgets.Label(textRect, _text);

            Text.Anchor = prevAnchor;
            GUI.color = prevColor;
            Text.Font = prevFont;

            return true;
        }

        internal enum FadeBy
        {
            Time,
            Tick
        }
    }
}
