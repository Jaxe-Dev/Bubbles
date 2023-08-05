using System;
using HarmonyLib;
using UnityEngine;
using Verse;

namespace Bubbles.Core
{
  internal static class Compatibility
  {
    private static readonly bool CameraPlusLoaded = ModsConfig.IsActive("brrainz.cameraplus");

    public static readonly FastInvokeHandler BeginGroupHandler = MethodInvoker.GetHandler(typeof(Widgets).GetMethod("BeginGroup", new[] { typeof(Rect) }) ?? typeof(GUI).GetMethod(nameof(GUI.BeginGroup), new[] { typeof(Rect) }));
    public static readonly FastInvokeHandler EndGroupHandler = MethodInvoker.GetHandler(typeof(Widgets).GetMethod("EndGroup", new Type[] { }) ?? typeof(GUI).GetMethod(nameof(GUI.EndGroup), new Type[] { }));

    private static FastInvokeHandler _cameraPlusLerpRootSize;

    public static void Apply(ref float altitude) => ApplyCameraPlus(ref altitude);

    private static void ApplyCameraPlus(ref float scale)
    {
      if (!CameraPlusLoaded) { return; }

      if (_cameraPlusLerpRootSize == null) { _cameraPlusLerpRootSize = MethodInvoker.GetHandler(AccessTools.Method("CameraPlus.Tools:LerpRootSize")); }

      scale = (float)_cameraPlusLerpRootSize(null, scale);
    }
  }
}
