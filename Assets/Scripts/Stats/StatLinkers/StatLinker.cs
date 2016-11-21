using UnityEngine;
using System.Collections;
using System;

[Serializable]
public abstract class StatLinker {

    public BaseStat LinkedStat { get; private set; }

    public event EventHandler OnValueChange;

    public abstract int Value { get; }

    public StatLinker(BaseStat stat)
    {
        LinkedStat = stat;
        stat.OnValueChange += OnLinkedStatValueChange;
    }

    private void OnLinkedStatValueChange(object stat, EventArgs args)
    {
        if (OnValueChange != null)
        {
            OnValueChange(this, null);
        }
    }
}
