using UnityEngine;
using System.Collections;
using System;

public class BaseAddMod : StatModifier {

    public override int Order
    {
        get { return 2; }
    }

    public override int ApplyModifier(int statValue, float modValue)
    {
        return (int)(modValue);
    }

    public BaseAddMod(float value, bool stacks) : base(value, stacks) { }
}
