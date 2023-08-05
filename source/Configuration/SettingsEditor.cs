using System.Linq;
using Bubbles.Core;
using UnityEngine;
using Verse;

namespace Bubbles.Configuration
{
  public static class SettingsEditor
  {
    private static string[] _colorBuffer = new string[4];

    private static Vector2 _scrollPosition = Vector2.zero;
    private static Rect _viewRect;

    public static void DrawSettings(Rect rect)
    {
      var listingRect = new Rect(rect.x, rect.y + 40f, rect.width, rect.height - 40f);
      var l = new Listing_Settings();
      l.Begin(rect);

      if (l.ButtonText("Bubbles.ResetToDefault".Translate())) { Reset(); }

      l.End();
      l.BeginScrollView(listingRect, ref _scrollPosition, ref _viewRect);

      l.CheckboxLabeled("Bubbles.DoNonPlayer".Translate(), ref Settings.DoNonPlayer.Value);
      l.CheckboxLabeled("Bubbles.DoAnimals".Translate(), ref Settings.DoAnimals.Value);
      l.CheckboxLabeled("Bubbles.DoDrafted".Translate(), ref Settings.DoDrafted.Value);
      var doTextColors = Settings.DoTextColors.Value;
      l.CheckboxLabeled("Bubbles.DoTextColors".Translate(), ref Settings.DoTextColors.Value);
      if (doTextColors != Settings.DoTextColors.Value) { Bubbler.Rebuild(); }
      l.Gap();

      l.SliderLabeled("Bubbles.AutoHideSpeed".Translate(), ref Settings.AutoHideSpeed.Value, 1, 4, display: Settings.AutoHideSpeed.Value == Settings.AutoHideSpeedDisabled ? "Bubbles.AutoHideSpeedOff".Translate().ToString() : Settings.AutoHideSpeed.Value.ToString());

      l.SliderLabeled("Bubbles.AltitudeBase".Translate(), ref Settings.AltitudeBase.Value, 3, 44);
      l.SliderLabeled("Bubbles.AltitudeMax".Translate(), ref Settings.AltitudeMax.Value, 20, 60);
      l.SliderLabeled("Bubbles.ScaleMax".Translate(), ref Settings.ScaleMax.Value, 1f, 5f, 0.05f, Settings.ScaleMax.Value.ToStringPercent());
      l.SliderLabeled("Bubbles.PawnMax".Translate(), ref Settings.PawnMax.Value, 1, 15);

      l.SliderLabeled("Bubbles.FontSize".Translate(), ref Settings.FontSize.Value, 5, 30);
      l.SliderLabeled("Bubbles.PaddingX".Translate(), ref Settings.PaddingX.Value, 1, 40);
      l.SliderLabeled("Bubbles.PaddingY".Translate(), ref Settings.PaddingY.Value, 1, 40);
      l.SliderLabeled("Bubbles.WidthMax".Translate(), ref Settings.WidthMax.Value, 100, 500, 4);

      l.SliderLabeled("Bubbles.OffsetSpacing".Translate(), ref Settings.OffsetSpacing.Value, 2, 12);
      l.SliderLabeled("Bubbles.OffsetStart".Translate(), ref Settings.OffsetStart.Value, 0, 400, 2);

      var offsetDirection = Settings.OffsetDirection.Value.AsInt;
      l.SliderLabeled("Bubbles.OffsetDirection".Translate(), ref offsetDirection, 0, 3, display: "Bubbles.OffsetDirections".Translate().ToString().Split('|').ElementAtOrDefault(offsetDirection));
      Settings.OffsetDirection.Value = new Rot4(offsetDirection);

      l.SliderLabeled("Bubbles.OpacityStart".Translate(), ref Settings.OpacityStart.Value, 0.3f, 1f, 0.05f, Settings.OpacityStart.Value.ToStringPercent());
      l.SliderLabeled("Bubbles.OpacityHover".Translate(), ref Settings.OpacityHover.Value, 0.05f, 1f, 0.05f, Settings.OpacityHover.Value.ToStringPercent());
      l.SliderLabeled("Bubbles.FadeStart".Translate(), ref Settings.FadeStart.Value, 100, 5000, 50);
      l.SliderLabeled("Bubbles.FadeLength".Translate(), ref Settings.FadeLength.Value, 50, 2500, 50);

      l.ColorEntry("Bubbles.Background".Translate(), ref _colorBuffer[0], ref Settings.Background.Value);
      l.ColorEntry("Bubbles.Foreground".Translate(), ref _colorBuffer[1], ref Settings.Foreground.Value);
      l.ColorEntry("Bubbles.BackgroundSelected".Translate(), ref _colorBuffer[2], ref Settings.SelectedBackground.Value);
      l.ColorEntry("Bubbles.ForegroundSelected".Translate(), ref _colorBuffer[3], ref Settings.SelectedForeground.Value);

      l.EndScrollView(ref _viewRect);
    }

    private static void Reset()
    {
      _colorBuffer = new string[4];

      Settings.Reset();
    }

    public static void ShowWindow() => Find.WindowStack.Add(new Dialog());

    private class Dialog : Window
    {
      public override Vector2 InitialSize => new Vector2(600f, 600f);

      public Dialog()
      {
        optionalTitle = $"<b>{Mod.Name}</b>";
        doCloseX = true;
        doCloseButton = true;
        draggable = true;

        _viewRect = default;
      }

      public override void DoWindowContents(Rect rect)
      {
        rect.yMax -= 60f;
        DrawSettings(rect.ContractedBy(8f, 0f));
      }

      public override void PostClose()
      {
        Mod.Instance.WriteSettings();
        _viewRect = default;
      }
    }
  }
}
