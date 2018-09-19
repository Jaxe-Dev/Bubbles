using System.Collections.Generic;
using System.Linq;
using Harmony;
using RimWorld.Planet;
using Verse;

namespace Bubbles.Interface
{
    internal static class Bubbler
    {
        public static bool Activated { get; set; } = true;
        public static int FadeStart { get; set; } = 500;
        public static int FadeTime { get; set; } = 500;
        public static int BubbleWidth { get; set; } = 300;
        public static int BubblePadding { get; set; } = 2;
        public static int MaxBubblesPerPawn { get; set; } = 3;

        public static Bubble.FadeBy FadeMode { get; set; } = Bubble.FadeBy.Tick;

        private static readonly Dictionary<Pawn, List<Bubble>> Bubbles = new Dictionary<Pawn, List<Bubble>>();

        public static void Add(LogEntry entry)
        {
            if (!(entry is PlayLogEntry_Interaction interaction)) { return; }

            var pawn = Traverse.Create(interaction).Field<Pawn>("initiator").Value;
            if (pawn == null) { return; }

            if (!Bubbles.ContainsKey(pawn)) { Bubbles[pawn] = new List<Bubble>(); }
            if (Bubbles[pawn].Count >= MaxBubblesPerPawn) { Remove(Bubbles[pawn][0]); }

            var bubble = new Bubble(pawn, interaction.ToGameStringFromPOV(pawn), Bubbles[pawn].Count, Bubbles[pawn].Sum(item => item.Height));
            Bubbles[pawn].Add(bubble);
        }

        private static void Remove(Bubble bubble)
        {
            var pawn = bubble.Pawn;
            Bubbles[pawn].Remove(bubble);

            foreach (var existing in Bubbles[pawn]) { existing.Pop(bubble); }

            if (Bubbles[pawn].Count == 0) { Bubbles.Remove(pawn); }
        }

        public static void Draw()
        {
            if (!Activated || WorldRendererUtility.WorldRenderedNow) { return; }
            foreach (var pawn in Bubbles.Keys.OrderBy(pawn => pawn.Position.y).ToArray()) { DrawBubble(pawn); }
        }

        private static void DrawBubble(Pawn pawn)
        {
            if (!pawn.Spawned || (pawn.Map != Find.CurrentMap) || pawn.Map.fogGrid.IsFogged(pawn.Position)) { return; }

            foreach (var bubble in Bubbles[pawn].ToArray())
            {
                if (!bubble.Draw()) { Remove(bubble); }
            }
        }

        public static void Reset() => Bubbles.Clear();
    }
}
