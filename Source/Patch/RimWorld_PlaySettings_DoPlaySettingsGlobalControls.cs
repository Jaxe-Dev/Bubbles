using Bubbles.Interface;
using HarmonyLib;
using RimWorld;
using Verse;

namespace Bubbles.Patch
{
    [HarmonyPatch(typeof(PlaySettings), "DoPlaySettingsGlobalControls")]
    internal static class RimWorld_PlaySettings_DoPlaySettingsGlobalControls
    {
        private static void Postfix(WidgetRow row, bool worldView)
        {
            if (worldView || row == null) { return; }

            var activated = Bubbler.Visibility;
            row.ToggleableIcon(ref activated, Textures.Icon, "Bubbles.Toggle".Translate(), SoundDefOf.Mouseover_ButtonToggle);
            Bubbler.Visibility = activated;
        }
    }
}
