using System.Text.RegularExpressions;
using LudeonTK;
using UnityEngine;
using Verse;

namespace Bubbles.Core
{
  public class Bubble
  {
    private static readonly Regex RemoveColorTag = new("<\\/?color[^>]*>");
    private static readonly GUIContent Content = new();

    public PlayLogEntry_Interaction Entry { get; }

    private readonly Pawn _pawn;

    private string? _text;
    private string Text => _text ??= GetText();

    private GUIStyle? _style;
    private GUIStyle Style => _style ??= new GUIStyle(Verse.Text.CurFontStyle)
    {
      alignment = TextAnchor.MiddleCenter,
      clipping = TextClipping.Clip
    };

    public int Height { get; private set; }
    public int Width { get; private set; }

    public Bubble(Pawn pawn, PlayLogEntry_Interaction entry)
    {
      Entry = entry;
      _pawn = pawn;
    }

    private static Color GetBackground(bool isSelected) => isSelected ? Settings.SelectedBackground.Value : Settings.Background.Value;
    private static Color GetForeground(bool isSelected) => isSelected ? Settings.SelectedForeground.Value : Settings.Foreground.Value;

    private static void DrawAtlas(Rect rect, Texture2D atlas)
    {
      rect.xMin = UIScaling.AdjustCoordToUIScalingFloor(rect.xMin);
      rect.yMin = UIScaling.AdjustCoordToUIScalingFloor(rect.yMin);
      rect.xMax = UIScaling.AdjustCoordToUIScalingCeil(rect.xMax);
      rect.yMax = UIScaling.AdjustCoordToUIScalingCeil(rect.yMax);

      var scale = Mathf.RoundToInt(Mathf.Min(atlas.width * 0.25f, rect.height * 0.25f, rect.width * 0.25f));

      Compatibility.BeginGroupHandler(null, rect);

      Widgets.DrawTexturePart(new Rect(0.0f, 0.0f, scale, scale), new Rect(0.0f, 0.0f, 0.25f, 0.25f), atlas);
      Widgets.DrawTexturePart(new Rect(rect.width - scale, 0.0f, scale, scale), new Rect(0.75f, 0.0f, 0.25f, 0.25f), atlas);
      Widgets.DrawTexturePart(new Rect(0.0f, rect.height - scale, scale, scale), new Rect(0.0f, 0.75f, 0.25f, 0.25f), atlas);

      Widgets.DrawTexturePart(new Rect(rect.width - scale, rect.height - scale, scale, scale), new Rect(0.75f, 0.75f, 0.25f, 0.25f), atlas);
      Widgets.DrawTexturePart(new Rect(scale, scale, rect.width - (scale * 2f), rect.height - (scale * 2f)), new Rect(0.25f, 0.25f, 0.5f, 0.5f), atlas);
      Widgets.DrawTexturePart(new Rect(scale, 0.0f, rect.width - (scale * 2f), scale), new Rect(0.25f, 0.0f, 0.5f, 0.25f), atlas);

      Widgets.DrawTexturePart(new Rect(scale, rect.height - scale, rect.width - (scale * 2f), scale), new Rect(0.25f, 0.75f, 0.5f, 0.25f), atlas);
      Widgets.DrawTexturePart(new Rect(0.0f, scale, scale, rect.height - (scale * 2f)), new Rect(0.0f, 0.25f, 0.25f, 0.5f), atlas);
      Widgets.DrawTexturePart(new Rect(rect.width - scale, scale, scale, rect.height - (scale * 2f)), new Rect(0.75f, 0.25f, 0.25f, 0.5f), atlas);

      Compatibility.EndGroupHandler(null);
    }

    public bool Draw(Vector2 pos, bool isSelected, float scale)
    {
      ScaleFont(ref scale);
      ScaleDimensions(scale);
      ScalePadding(scale);

      var posX = pos.x;
      var posY = pos.y;

      if (Settings.OffsetDirection.Value.IsHorizontal) { posY -= Height * 0.5f; }
      else { posX -= Width * 0.5f; }

      var rect = new Rect(Mathf.Ceil(posX), Mathf.Ceil(posY), Width, Height).RoundedCeil();

      var fade = Event.current!.shift && Mouse.IsOver(rect) ? Settings.OpacityStart.Value : Mathf.Min(GetFade(), Mouse.IsOver(rect) ? Settings.OpacityHover.Value : 1f);
      if (fade <= 0f) { return false; }

      var background = GetBackground(isSelected).ToTransparent(fade);
      var foreground = GetForeground(isSelected).ToTransparent(fade);

      var prevColor = GUI.color;

      GUI.color = background;
      DrawAtlas(rect, Textures.Inner);

      GUI.color = foreground;
      DrawAtlas(rect, Textures.Outer);
      GUI.Label(rect, Text, Style);

      GUI.color = prevColor;

      return true;
    }

    private void ScaleFont(ref float scale)
    {
      Style.fontSize = Mathf.RoundToInt(Settings.FontSize.Value * scale);
      scale = Style.fontSize / (float)Settings.FontSize.Value;
    }

    private void ScalePadding(float scale)
    {
      var paddingX = Mathf.RoundToInt(Settings.PaddingX.Value * scale);
      var paddingY = Mathf.RoundToInt(Settings.PaddingY.Value * scale);
      Style.padding = new RectOffset(paddingX, paddingX, paddingY, paddingY);
    }

    private void ScaleDimensions(float scale)
    {
      Content.text = Text;
      Width = Mathf.RoundToInt(Mathf.Min(Style.CalcSize(Content).x, Settings.WidthMax.Value * scale));
      Height = Mathf.RoundToInt(Style.CalcHeight(Content, Width));
    }

    private string GetText()
    {
      var text = Entry.ToGameStringFromPOV(_pawn);
      return Settings.DoTextColors.Value ? text : RemoveColorTag.Replace(text, string.Empty);
    }

    private float GetFade()
    {
      var elasped = Find.TickManager!.TicksAbs - Entry.Tick - Settings.FadeStart.Value;

      if (elasped <= 0) { return Settings.OpacityStart.Value; }
      if (elasped > Settings.FadeLength.Value) { return 0f; }

      var fade = Settings.OpacityStart.Value * (1f - (elasped / (float)Settings.FadeLength.Value));
      return fade;
    }

    public void Rebuild() => _text = null;
  }
}
