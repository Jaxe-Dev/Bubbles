using Bubbles.Core;
using HarmonyLib;
using Verse;

namespace Bubbles.Access;

[HarmonyPatch(typeof(PlayLog), nameof(PlayLog.Add))]
public static class Verse_PlayLog_Add
{
  private static void Postfix(LogEntry entry) => Bubbler.Add(entry);
}
