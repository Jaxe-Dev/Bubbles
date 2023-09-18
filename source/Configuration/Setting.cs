using UnityEngine;
using Verse;

namespace Bubbles.Configuration
{
  public abstract class Setting
  {
    public abstract void ToDefault();
    public abstract void Scribe();
    protected readonly string Id;
    protected Setting(string id) => Id = id;
  }

  public class Setting<T> : Setting where T : struct
  {
    private readonly T _default;
    public T Value;

    public Setting(string id, T defaultValue) : base(id)
    {
      _default = defaultValue;
      Value = defaultValue;
    }

    public override void ToDefault() => Value = _default;

    public override void Scribe()
    {
      if ((Value is float floatValue && _default is float floatDefault && Mathf.Approximately(floatValue, floatDefault)) || (Value is Color colorValue && _default is Color colorDefault && colorValue.IndistinguishableFromFast(colorDefault))) { Value = _default; }

      Scribe_Values.Look(ref Value, Id, _default);
    }
  }
}
