using System.Collections.Generic;
using System.Linq;
using HarmonyLib;
using RimWorld.Planet;
using UnityEngine;
using Verse;

namespace Bubbles.Core
{
  public static class Bubbler
  {
    private const float LabelPositionOffset = -0.6f;

    private static readonly Dictionary<Pawn, List<Bubble>> Dictionary = new Dictionary<Pawn, List<Bubble>>();

    public static void Add(LogEntry entry)
    {
      if (!GetApplicable() || !(entry is PlayLogEntry_Interaction interaction)) { return; }

      var initiator = Traverse.Create(interaction).Field<Pawn>("initiator").Value;
      var recipient = Traverse.Create(interaction).Field<Pawn>("recipient").Value;
      if (initiator == null || initiator.Map != Find.CurrentMap) { return; }

      if (!Settings.DoNonPlayer.Value && (!initiator.Faction?.IsPlayer ?? true)) { return; }
      if (!Settings.DoAnimals.Value && ((initiator?.RaceProps?.Animal ?? false) || (recipient?.RaceProps?.Animal ?? false))) { return; }
      if (!Settings.DoDrafted.Value && ((initiator.drafter?.Drafted ?? false) || (recipient?.drafter?.Drafted ?? false))) { return; }

      if (!Dictionary.ContainsKey(initiator)) { Dictionary[initiator] = new List<Bubble>(); }

      Dictionary[initiator].Add(new Bubble(initiator, interaction));
    }

    private static void Remove(Pawn pawn, Bubble bubble)
    {
      Dictionary[pawn].Remove(bubble);
      if (Dictionary[pawn].Count == 0) { Dictionary.Remove(pawn); }
    }

    public static void Draw()
    {
      if (!GetApplicable()) { return; }

      var altitude = GetAltitude();
      if (altitude <= 0 || altitude > Settings.AltitudeMax.Value) { return; }

      var scale = Settings.AltitudeBase.Value / altitude;
      if (scale > Settings.ScaleMax.Value) { scale = Settings.ScaleMax.Value; }

      var selected = Find.Selector.SingleSelectedObject as Pawn;

      foreach (var pawn in Dictionary.Keys.OrderBy(pawn => pawn == selected).ThenBy(pawn => pawn.Position.y).ToArray()) { DrawBubble(pawn, pawn == selected, scale); }
    }

    private static void DrawBubble(Pawn pawn, bool isSelected, float scale)
    {
      if (!pawn.Spawned || pawn.Map != Find.CurrentMap || pawn.Map.fogGrid.IsFogged(pawn.Position)) { return; }

      var pos = GenMapUI.LabelDrawPosFor(pawn, LabelPositionOffset);

      var offset = Settings.OffsetStart.Value;
      var count = 0;

      foreach (var bubble in Dictionary[pawn].OrderByDescending(b => b.Entry.Tick).ToArray())
      {
        if (count > Settings.PawnMax.Value) { return; }
        if (!bubble.Draw(pos + GetOffset(offset), isSelected, scale)) { Remove(pawn, bubble); }
        offset += (Settings.OffsetDirection.Value.IsHorizontal ? bubble.Width : bubble.Height) + Settings.OffsetSpacing.Value;
        count++;
      }
    }

    private static bool GetApplicable() => Settings.Activated && !WorldRendererUtility.WorldRenderedNow && (Settings.AutoHideSpeed.Value == Settings.AutoHideSpeedDisabled || (int)Find.TickManager.CurTimeSpeed < Settings.AutoHideSpeed.Value);

    private static float GetAltitude()
    {
      var altitude = Mathf.Max(1f, Traverse.Create(Find.CameraDriver).Field<float>("rootSize").Value);
      Compatibility.Apply(ref altitude);

      return altitude;
    }

    private static Vector2 GetOffset(float offset)
    {
      var direction = Settings.OffsetDirection.Value.AsVector2;
      return new Vector2(offset * direction.x, offset * direction.y);
    }

    public static void Rebuild() => Dictionary.Values.Do(list => list.Do(bubble => bubble.Rebuild()));

    public static void Clear() => Dictionary.Clear();
  }
}
