using UnityEngine;
using Verse;

namespace Bubbles.Configuration;

public abstract class Setting(string id)
{
  public abstract void ToDefault();
  public abstract void Scribe();
  protected readonly string Id = id;
}

public class Setting<T>(string id, T defaultValue) : Setting(id) where T : struct
{
  private readonly T _default = defaultValue;
  public T Value = defaultValue;

  public override void ToDefault() => Value = _default;

  public override void Scribe()
  {
    if ((Value is float floatValue && _default is float floatDefault && Mathf.Approximately(floatValue, floatDefault)) || (Value is Color colorValue && _default is Color colorDefault && colorValue.IndistinguishableFromFast(colorDefault))) { Value = _default; }

    Scribe_Values.Look(ref Value, Id, _default);
  }
}
