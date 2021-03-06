﻿using Bubbles.Interface;
using HarmonyLib;
using RimWorld;

namespace Bubbles.Patch
{
    [HarmonyPatch(typeof(MapInterface), "MapInterfaceOnGUI_BeforeMainTabs")]
    internal static class RimWorld_MapInterface_MapInterfaceOnGUI_BeforeMainTabs
    {
        private static void Postfix() => Bubbler.Draw();
    }
}
