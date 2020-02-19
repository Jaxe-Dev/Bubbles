using Bubbles.Interface;
using HarmonyLib;
using Verse;

namespace Bubbles.Patch
{
    [HarmonyPatch(typeof(BattleLog), "Add")]
    internal static class Verse_BattleLog_Add
    {
        private static void Postfix(LogEntry entry) => Bubbler.Add(entry, true);
    }
}
