using UnityEngine;
using Verse;

namespace Bubbles
{
    internal class Instance : Verse.Mod
    {
        public Instance(ModContentPack content) : base(content) => Mod.Settings = GetSettings<Settings>();

        public override void DoSettingsWindowContents(Rect inRect)
        {
            Mod.Settings.DrawSettings(inRect);
            base.DoSettingsWindowContents(inRect);
        }

        public override string SettingsCategory() => Mod.Name;
    }
}
