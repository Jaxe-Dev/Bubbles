using Bubbles.Core;
using HarmonyLib;
using Verse.Profile;

namespace Bubbles.Access;

[HarmonyPatch(typeof(MemoryUtility), nameof(MemoryUtility.ClearAllMapsAndWorld))]
public static class Verse_Profile_MemoryUtility_ClearAllMapsAndWorld
{
  private static void Prefix() => Bubbler.Clear();
}
