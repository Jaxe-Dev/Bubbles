using HarmonyLib;
using Verse;

namespace Bubbles
{
    [StaticConstructorOnStartup]
    internal static class Mod
    {
        public const string Id = "Bubbles";
        public const string Name = "Interaction Bubbles";
        public const string Version = "1.8";

        public static readonly Harmony Harmony;
        public static Settings Settings;

        static Mod()
        {
            Harmony = new Harmony(Id);
            Harmony.PatchAll();

            Log("Initialized");
        }

        public static void Log(string message) => Verse.Log.Message(PrefixMessage(message));
        private static string PrefixMessage(string message) => $"[{Name} v{Version}] {message}";
    }
}
