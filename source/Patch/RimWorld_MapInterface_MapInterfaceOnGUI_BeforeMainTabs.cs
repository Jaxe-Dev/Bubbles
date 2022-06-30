using System;
using Bubbles.Core;
using HarmonyLib;
using RimWorld;

namespace Bubbles.Patch
{
  [HarmonyPatch(typeof(MapInterface), nameof(MapInterface.MapInterfaceOnGUI_BeforeMainTabs))]
  public static class RimWorld_MapInterface_MapInterfaceOnGUI_BeforeMainTabs
  {
    private static void Postfix()
    {
      try { Bubbler.Draw(); }
      catch (Exception exception)
      {
        Mod.Error($"Deactivated because draw failed with error: [{exception.Source}: {exception.Message}]\n\nStacktrace:\n{exception.StackTrace}");
        Settings.Activated = false;
      }
    }
  }
}
