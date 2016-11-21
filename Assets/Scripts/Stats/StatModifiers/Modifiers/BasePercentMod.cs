using UnityEngine;
using System.Collections;

public class BasePercentMod : StatModifier {


    public override int Order
    {
        get { return 1; }
    }

    public override int ApplyModifier(int statValue, float modValue)
    {
        return (int)(statValue * modValue);
    }

    public BasePercentMod(float value, bool stacks) : base(value, stacks) { }
}
