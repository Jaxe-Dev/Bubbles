using UnityEngine;
using Verse;

namespace Bubbles.Interface
{
    internal class ListingPlus : Listing_Standard
    {
        private const float ScrollbarWidth = 20f;

        public void BeginScrollView(Rect rect, ref Vector2 scrollPosition, ref Rect viewRect)
        {
            if (viewRect == default) { viewRect = new Rect(rect.x, rect.y, rect.width - ScrollbarWidth, 99999f); }

            Widgets.BeginScrollView(rect, ref scrollPosition, viewRect);

            Begin(viewRect);
        }

        public void EndScrollView(ref Rect viewRect)
        {
            End();
            Widgets.EndScrollView();
            viewRect.height = CurHeight;
        }

        public void IntSlider(string label, ref int value, int min, int max, string format = null)
        {
            LabelDouble(label, format == null ? value.ToString() : string.Format(format, value));
            value = (int) Slider(value, min, max);
        }

        public void ColorEntry(string label, ref string buffer, ref Color original)
        {
            var rect = GetRect(Text.LineHeight);
            var rectLeft = rect.LeftHalf().Rounded();
            var rectRight = rect.RightHalf().Rounded();
            var rectEntry = rectRight.LeftHalf().Rounded();
            var rectPreview = rectRight.RightHalf().Rounded();
            Widgets.Label(rectLeft, label);

            Widgets.DrawBoxSolid(rectPreview, original);
            Widgets.DrawBox(rectPreview);

            if (buffer == null) { buffer = ColorUtility.ToHtmlStringRGB(original); }

            buffer = (rect.height <= 30f ? Widgets.TextField(rectEntry, buffer) : Widgets.TextArea(rectEntry, buffer)).ToUpper();

            var color = original;
            var valid = buffer.Length == 6 && ColorUtility.TryParseHtmlString("#" + buffer, out color);

            if (!valid)
            {
                var guiColor = GUI.color;
                GUI.color = Color.red;
                Widgets.DrawBox(rectEntry);
                GUI.color = guiColor;
            }

            original = valid ? color : original;

            Gap(verticalSpacing);
        }
    }
}
