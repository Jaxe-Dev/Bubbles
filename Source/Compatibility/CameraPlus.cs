using System.Linq;
using Bubbles.Interface;
using Verse;

namespace Bubbles.Compatibility
{
    internal static class CameraPlus
    {
        public const float CameraPlusOptimalZoom = Bubbler.OptimalZoom / 2f;

        public static readonly bool Loaded = LoadedModManager.RunningModsListForReading.FirstOrDefault(mod => mod.Name == "Camera+")?.assemblies.loadedAssemblies.FirstOrDefault(assembly => assembly.GetName().Name == "CameraPlus") != null;
    }
}
