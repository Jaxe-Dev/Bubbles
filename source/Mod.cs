using Bubbles.Configuration;
using HarmonyLib;
using UnityEngine;
using Verse;

namespace Bubbles
{
  public class Mod : Verse.Mod
  {
    public const string Id = "Bubbles";
    public const string Name = "Interaction Bubbles";
    public const string Version = "2.2";

    public static Mod Instance;

    public Mod(ModContentPack content) : base(content)
    {
      Instance = this;

      GetSettings<Settings>();
      new Harmony(Id).PatchAll();

      Log("Initialized");
    }

    public static void Log(string message) => Verse.Log.Message(PrefixMessage(message));
    private static string PrefixMessage(string message) => $"[{Name} v{Version}] {message}";

    public override void DoSettingsWindowContents(Rect inRect)
    {
      Widgets.Label(inRect, $"<b>{"Bubbles.NonWindow".Translate()}</b>".Colorize(Color.yellow));
      inRect.yMin += 40f;
      SettingsEditor.DrawSettings(inRect);
      base.DoSettingsWindowContents(inRect);
    }

    public override string SettingsCategory() => Name;
  }
}
