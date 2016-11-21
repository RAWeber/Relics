using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

[Serializable]
public class BaseStat {

    private int baseValue;

    public event EventHandler OnValueChange;

    public string Name { get; set; }
    public string Description { get; set; }
    public virtual int BaseValue {
        get { return baseValue; }
        set {
            if(value != baseValue)
            {
                baseValue = value;
                TriggerValueChange();
            }
        }
    }

    public virtual int TotalStatValue { get { return BaseValue; } }

    public BaseStat()
    {
        Name = string.Empty;
        BaseValue = 0;
    }

    public BaseStat(string name, int baseVal)
    {
        Name = name;
        BaseValue = baseVal;
    }

    public override bool Equals(object obj)
    {
        if (obj == null) return false;

        BaseStat stat = obj as BaseStat;
        if (stat == null) return false;

        return this.Name.Equals(stat.Name);
    }

    public override int GetHashCode()
    {
        return this.Name.GetHashCode();
    }

    protected void TriggerValueChange()
    {
        if (OnValueChange != null)
        {
            OnValueChange(this, null);
        }
    }
}
