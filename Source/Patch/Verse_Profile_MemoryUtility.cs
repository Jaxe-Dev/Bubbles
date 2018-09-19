using Bubbles.Interface;
using Harmony;
using Verse.Profile;

namespace Bubbles.Patch
{
    [HarmonyPatch(typeof(MemoryUtility), "ClearAllMapsAndWorld")]
    internal static class Verse_Profile_MemoryUtility_ClearAllMapsAndWorld
    {
        private static void Prefix() => Bubbler.Reset();
    }
}
