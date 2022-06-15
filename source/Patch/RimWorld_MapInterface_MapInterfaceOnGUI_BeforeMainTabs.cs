using Bubbles.Core;
using HarmonyLib;
using RimWorld;

namespace Bubbles.Patch
{
  [HarmonyPatch(typeof(MapInterface), nameof(MapInterface.MapInterfaceOnGUI_BeforeMainTabs))]
  public static class RimWorld_MapInterface_MapInterfaceOnGUI_BeforeMainTabs
  {
    private static void Postfix() => Bubbler.Draw();
  }
}
