using System.Collections.Generic;
using System.Linq;
using Bubbles.Access;
using HarmonyLib;
using RimWorld;
using RimWorld.Planet;
using UnityEngine;
using Verse;

namespace Bubbles.Core;

public static class Bubbler
{
  private const float LabelPositionOffset = -0.6f;

  private static readonly Dictionary<Pawn, List<Bubble>> Dictionary = new();

  private static bool CanRender() => Settings.Activated && WorldRendererUtility.CurrentWorldRenderMode is WorldRenderMode.None or WorldRenderMode.Background && (Settings.AutoHideSpeed.Value is Settings.AutoHideSpeedDisabled || (int)Find.TickManager!.CurTimeSpeed < Settings.AutoHideSpeed.Value);

  private static bool CanBeHeard(Pawn pawn) => !Settings.HearingCheck.Value || (pawn.Map is not null && (from colonist in pawn.Map.mapPawns?.FreeColonistsSpawned let hearing = colonist.health.capacities.GetLevel(PawnCapacityDefOf.Hearing) where !(hearing <= 0f) where colonist.Position.InHorDistOf(pawn.Position, Settings.HearingRange.Value * hearing) select colonist).Any());

  public static void Add(LogEntry entry)
  {
    if (!CanRender()) { return; }

    Pawn? initiator, recipient;

    switch (entry)
    {
      case PlayLogEntry_Interaction interaction:
        initiator = (Pawn?)Reflection.Verse_PlayLogEntry_Interaction_Initiator.GetValue(interaction);
        recipient = (Pawn?)Reflection.Verse_PlayLogEntry_Interaction_Recipient.GetValue(interaction);
        break;
      case PlayLogEntry_InteractionSinglePawn interaction:
        initiator = (Pawn?)Reflection.Verse_PlayLogEntry_InteractionSinglePawn_Initiator.GetValue(interaction);
        recipient = null;
        break;
      default:
        return;
    }

    if (initiator is null || initiator.Map != Find.CurrentMap) { return; }

    if ((!initiator.Faction?.IsPlayer ?? true) && !CanBeHeard(initiator)) { return; }

    if (!Settings.DoNonPlayer.Value && (!initiator.Faction?.IsPlayer ?? true)) { return; }
    if (!Settings.DoAnimals.Value && ((initiator.RaceProps?.Animal ?? false) || (recipient?.RaceProps?.Animal ?? false))) { return; }
    if (!Settings.DoDrafted.Value && ((initiator.drafter?.Drafted ?? false) || (recipient?.drafter?.Drafted ?? false))) { return; }

    if (!Dictionary.ContainsKey(initiator)) { Dictionary[initiator] = []; }

    Dictionary[initiator]!.Add(new Bubble(initiator, entry));
  }

  private static void Remove(Pawn pawn, Bubble bubble)
  {
    Dictionary[pawn]!.Remove(bubble);
    if (Dictionary[pawn]!.Count is 0) { Dictionary.Remove(pawn); }
  }

  public static void Draw()
  {
    var altitude = GetAltitude();
    if (altitude <= 0 || altitude > Settings.AltitudeMax.Value) { return; }

    var scale = Settings.AltitudeBase.Value / altitude;
    if (scale > Settings.ScaleMax.Value) { scale = Settings.ScaleMax.Value; }

    var selected = Find.Selector!.SingleSelectedObject as Pawn;

    foreach (var pawn in Dictionary.Keys.OrderBy(pawn => pawn == selected).ThenBy(static pawn => pawn.Position.y).ToArray()) { DrawBubble(pawn, pawn == selected, scale); }
  }

  private static void DrawBubble(Pawn pawn, bool isSelected, float scale)
  {
    if (!CanRender() || !pawn.Spawned || pawn.Map != Find.CurrentMap || pawn.Map!.fogGrid!.IsFogged(pawn.Position)) { return; }

    var pos = GenMapUI.LabelDrawPosFor(pawn, LabelPositionOffset);

    var offset = Settings.OffsetStart.Value;
    var count = 0;

    foreach (var bubble in Dictionary[pawn].OrderByDescending(static bubble => bubble.Entry.Tick).ToArray())
    {
      if (count > Settings.PawnMax.Value) { return; }
      if (!bubble.Draw(pos + GetOffset(offset), isSelected, scale)) { Remove(pawn, bubble); }
      offset += (Settings.OffsetDirection.Value.IsHorizontal ? bubble.Width : bubble.Height) + Settings.OffsetSpacing.Value;
      count++;
    }
  }

  private static float GetAltitude()
  {
    var altitude = Mathf.Max(1f, (float)Reflection.Verse_CameraDriver_RootSize.GetValue(Find.CameraDriver));
    Compatibility.Apply(ref altitude);

    return altitude;
  }

  private static Vector2 GetOffset(float offset)
  {
    var direction = Settings.OffsetDirection.Value.AsVector2;
    return new Vector2(offset * direction.x, offset * direction.y);
  }

  public static void Rebuild() => Dictionary.Values.Do(static list => list.Do(static bubble => bubble.Rebuild()));

  public static void Clear() => Dictionary.Clear();
}
