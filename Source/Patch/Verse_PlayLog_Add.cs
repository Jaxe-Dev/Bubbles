﻿using Bubbles.Interface;
using HarmonyLib;
using Verse;

namespace Bubbles.Patch
{
    [HarmonyPatch(typeof(PlayLog), "Add")]
    internal static class Verse_PlayLog_Add
    {
        private static void Postfix(LogEntry entry) => Bubbler.Add(entry, false);
    }
}
