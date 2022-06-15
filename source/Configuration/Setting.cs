using UnityEngine;
using Verse;

namespace Bubbles.Configuration
{
  public abstract class Setting
  {
    protected readonly string Id;
    protected Setting(string id) => Id = id;

    public abstract void ToDefault();
    public abstract void Scribe();
  }

  public class Setting<T> : Setting where T : struct
  {
    public readonly T Default;
    public T Value;

    public Setting(string id, T defaultValue) : base(id)
    {
      Default = defaultValue;
      Value = defaultValue;
    }

    public override void ToDefault() => Value = Default;

    public override void Scribe()
    {
      if ((Value is float floatValue && Default is float floatDefault && Mathf.Approximately(floatValue, floatDefault)) || (Value is Color colorValue && Default is Color colorDefault && colorValue.IndistinguishableFromFast(colorDefault))) { Value = Default; }

      Scribe_Values.Look(ref Value, Id, Default);
    }
  }
}
