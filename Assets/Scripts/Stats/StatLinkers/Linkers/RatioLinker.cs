using UnityEngine;
using System.Collections;
using System;

[Serializable]
public class RatioLinker : StatLinker
{
    [SerializeField]
    private float ratio;

    public override int Value
    {
        get { return (int)(LinkedStat.BaseValue * ratio); }
    }

    public RatioLinker(BaseStat stat, float ratio) : base(stat)
    {
        this.ratio = ratio;
    }
}
