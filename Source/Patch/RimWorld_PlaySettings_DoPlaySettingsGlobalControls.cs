using Bubbles.Data;
using Bubbles.Interface;
using Harmony;
using RimWorld;
using Verse;

namespace Bubbles.Patch
{
    [HarmonyPatch(typeof(PlaySettings), "DoPlaySettingsGlobalControls")]
    internal static class RimWorld_PlaySettings_DoPlaySettingsGlobalControls
    {
        private static void Postfix(WidgetRow row, bool worldView)
        {
            if (worldView || (row == null)) { return; }

            var activated = Bubbler.Activated;
            row.ToggleableIcon(ref activated, Textures.Icon, Lang.Get("Toggle"), SoundDefOf.Mouseover_ButtonToggle);
            Bubbler.Activated = activated;
        }
    }
}
