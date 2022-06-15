using System.Globalization;
using UnityEngine;
using Verse;

namespace Bubbles.Configuration
{
  public class Listing_Settings : Listing_Standard
  {
    private const float ScrollAreaWidth = 24f;

    public void BeginScrollView(Rect rect, ref Vector2 scrollPosition, ref Rect viewRect)
    {
      if (viewRect == default) { viewRect = new Rect(rect.x, rect.y, rect.width - ScrollAreaWidth, 99999f); }

      Widgets.BeginScrollView(rect, ref scrollPosition, viewRect);

      Begin(viewRect);
    }

    public void EndScrollView(ref Rect viewRect)
    {
      End();
      Widgets.EndScrollView();
      viewRect.height = CurHeight;
    }

    public void SliderLabeled(string label, ref float value, float min, float max, float roundTo = -1f, string display = null)
    {
      var rect = GetRect(Text.LineHeight);

      Widgets.Label(rect.LeftHalf(), label);

      var anchor = Text.Anchor;
      Text.Anchor = TextAnchor.MiddleRight;
      Widgets.Label(rect.RightHalf(), display ?? value.ToString(CultureInfo.InvariantCulture));
      Text.Anchor = anchor;

      value = Slider(value, min, max);
      if (roundTo > 0f) { value = Mathf.Round(value / roundTo) * roundTo; }
    }

    public void SliderLabeled(string label, ref int value, int min, int max, int roundTo = -1, string display = null)
    {
      var floatValue = (float) value;
      SliderLabeled(label, ref floatValue, min, max, roundTo, display);
      value = (int) floatValue;
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
    }
  }
}
