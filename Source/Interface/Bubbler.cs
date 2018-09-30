using System.Collections.Generic;
using System.Linq;
using Bubbles.Patch;
using Harmony;
using RimWorld.Planet;
using UnityEngine;
using Verse;

namespace Bubbles.Interface
{
    internal static class Bubbler
    {
        private const float OptimalHeight = 1050f;
        private const float OptimalZoom = 45f;
        private const float OptimalMinZoom = 8f;
        private const float CameraPlusOptimalZoom = OptimalZoom / 2f;

        public static bool Visibility { get; set; } = true;
        public static bool IsVisible => Theme.Activated && Visibility;

        // Temporary fix for Camera+
        private static readonly bool CameraPlusLoaded = LoadedModManager.RunningModsListForReading.FirstOrDefault(mod => mod.Name == "Camera+")?.assemblies.loadedAssemblies.FirstOrDefault(assembly => assembly.GetName().Name == "CameraPlus") != null;

        // Temporary backwards compatibility
        public static int FadeStart { get => Theme.FadeStart; set => Theme.FadeStart = value; }
        public static int FadeTime { get => Theme.FadeLength; set => Theme.FadeLength = value; }
        public static int BubbleWidth { get => Theme.MaxWidth; set => Theme.MaxWidth = value; }
        public static int BubblePadding { get => Theme.Spacing; set => Theme.Spacing = value; }
        public static int MaxBubblesPerPawn { get => Theme.MaxPerPawn; set => Theme.MaxPerPawn = value; }

        private static readonly Dictionary<Pawn, List<Bubble>> Bubbles = new Dictionary<Pawn, List<Bubble>>();

        public static void Add(LogEntry entry)
        {
            if (!(entry is PlayLogEntry_Interaction interaction)) { return; }

            var initiator = Traverse.Create(interaction).Field<Pawn>("initiator").Value;
            var recipient = Traverse.Create(interaction).Field<Pawn>("recipient").Value;
            if (initiator == null) { return; }

            if ((!initiator.Faction?.IsPlayer ?? true) && !Theme.DoNonPlayer) { return; }
            if ((recipient?.RaceProps?.Animal ?? false) && !Theme.DoAnimals) { return; }

            if (!Bubbles.ContainsKey(initiator)) { Bubbles[initiator] = new List<Bubble>(); }

            var bubble = new Bubble(interaction.ToGameStringFromPOV(initiator));
            Bubbles[initiator].Add(bubble);
        }

        private static void Remove(Pawn pawn, Bubble bubble)
        {
            Bubbles[pawn].Remove(bubble);
            if (Bubbles[pawn].Count == 0) { Bubbles.Remove(pawn); }
        }

        public static void Draw()
        {
            if (!IsVisible || WorldRendererUtility.WorldRenderedNow) { return; }

            var correction = UI.screenHeight / OptimalHeight;
            var optimal = (CameraPlusLoaded ? CameraPlusOptimalZoom : OptimalZoom) * correction;
            var minZoom = OptimalMinZoom * correction;
            var range = Mathf.Max((optimal - minZoom) * Theme.ScaleStart.ToPercentageFloat(), 1f);
            var scale = Mathf.Min((Find.CameraDriver.CellSizePixels - minZoom) / range, 1f);

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
