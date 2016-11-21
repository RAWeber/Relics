using UnityEngine;
using System.Collections;

public class TotalAddMod : StatModifier {


    public override int Order
    {
        get { return 4; }
    }

    public override int ApplyModifier(int statValue, float modValue)
    {
        return (int)(modValue);
    }

    public TotalAddMod(float value, bool stacks) : base(value, stacks) { }
}
