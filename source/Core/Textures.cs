using UnityEngine;
using Verse;

namespace Bubbles.Core;

[StaticConstructorOnStartup]
public static class Textures
{
  public static readonly Texture2D Icon = ContentFinder<Texture2D>.Get("Bubbles/Icon");
  public static readonly Texture2D Inner = ContentFinder<Texture2D>.Get("Bubbles/Inner");
  public static readonly Texture2D Outer = ContentFinder<Texture2D>.Get("Bubbles/Outer");
}
