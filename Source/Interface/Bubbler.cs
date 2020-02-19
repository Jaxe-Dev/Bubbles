using System.Collections.Generic;
using System.Linq;
using Bubbles.Compatibility;
using Bubbles.Patch;
using HarmonyLib;
using RimWorld.Planet;
using UnityEngine;
using Verse;

namespace Bubbles.Interface
{
    internal static class Bubbler
    {
        public const float OptimalZoom = 45f;
        private const float OptimalMinZoom = 8f;
        private const float OptimalHeight = 1050f;

        public static bool Visibility { get; set; } = true;
        public static bool CanShow => Theme.Activated && Visibility && !WorldRendererUtility.WorldRenderedNow;

        private static readonly Dictionary<Pawn, List<Bubble>> Bubbles = new Dictionary<Pawn, List<Bubble>>();

        public static void Add(LogEntry entry, bool isCombat)
        {
            if (!CanShow || (isCombat && !Theme.DoCombat)) { return; }
            if (!isCombat && !(entry is PlayLogEntry_Interaction)) { return; }

            var initiator = Traverse.Create(entry).Field<Pawn>("initiator").Value;
            var recipient = Traverse.Create(entry).Field<Pawn>("recipient").Value;
            if ((initiator == null) || (initiator.Map != Find.CurrentMap)) { return; }

            if ((!initiator.Faction?.IsPlayer ?? true) && !Theme.DoNonPlayer) { return; }
            if ((recipient?.RaceProps?.Animal ?? false) && !Theme.DoAnimals) { return; }

            if (!Bubbles.ContainsKey(initiator)) { Bubbles[initiator] = new List<Bubble>(); }

            var bubble = new Bubble(entry.ToGameStringFromPOV(initiator), isCombat);
            Bubbles[initiator].Add(bubble);
        }

        private static void Remove(Pawn pawn, Bubble bubble)
        {
            Bubbles[pawn].Remove(bubble);
            if (Bubbles[pawn].Count == 0) { Bubbles.Remove(pawn); }
        }

        private static float GetScale()
        {
            var correction = UI.screenHeight / OptimalHeight;
            var optimal = (CameraPlus.Loaded ? CameraPlus.CameraPlusOptimalZoom : OptimalZoom) * correction;
            var minZoom = OptimalMinZoom * correction;
            var range = Mathf.Max((optimal - minZoom) * Theme.ScaleStart.ToPercentageFloat(), 1f);
            return Mathf.Min((Find.CameraDriver.CellSizePixels - minZoom) / range, 1f);
        }

        public static void Draw()
        {
            if (!CanShow) { return; }

            var scale = GetScale();
            if (scale <= Theme.MinScale.ToPercentageFloat()) { return; }

            var selected = Find.Selector.SingleSelectedObject as Pawn;

            foreach (var pawn in Bubbles.Keys.OrderBy(pawn => pawn.Position.y).Where(pawn => pawn != selected).ToArray()) { DrawBubble(pawn, false, scale); }

            if ((selected != null) && Bubbles.ContainsKey(selected)) { DrawBubble(selected, true, scale); }
        }

        private static void DrawBubble(Pawn pawn, bool isSelected, float scale)
        {
            if (!pawn.Spawned || (pawn.Map != Find.CurrentMap) || pawn.Map.fogGrid.IsFogged(pawn.Position)) { return; }

            var pos = GenMapUI.LabelDrawPosFor(pawn, -0.6f);

            var offset = Mathf.CeilToInt(Theme.StartOffset * scale);
            var count = 0;

            foreach (var bubble in Bubbles[pawn].ToArray())
            {
                if (count > Theme.MaxPerPawn) { return; }
                if (!bubble.Draw(pos + GetOffset(offset), isSelected, scale)) { Remove(pawn, bubble); }
                offset += (Theme.GetOffsetDirection().IsHorizontal ? bubble.Width : bubble.Height) + Theme.Spacing;
                count++;
            }
        }

        private static Vector2 GetOffset(int offset)
        {
            var direction = Theme.GetOffsetDirection().AsVector2;
            return new Vector2(offset * direction.x, offset * direction.y);
        }

        public static void Clear() => Bubbles.Clear();
    }
}
