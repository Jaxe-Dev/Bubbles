using Bubbles.Core;
using HarmonyLib;
using Verse;

namespace Bubbles.Patch
{
  [HarmonyPatch(typeof(PlayLog), nameof(PlayLog.Add))]
  public static class Verse_PlayLog_Add
  {
    private static void Postfix(LogEntry entry) => Bubbler.Add(entry);
  }
}
