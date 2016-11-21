using UnityEngine;
using System.Collections;
using System;

[Serializable]
public class VitalStat : LinkableStat {

    public event EventHandler OnCurrentValueChange;

    private int currentValue;

    public int CurrentValue
    {
        get
        {
            return currentValue;
        }
        set
        {
            if (currentValue != Mathf.Clamp(value, 0, TotalStatValue))
            {
                currentValue = Mathf.Clamp(value, 0, TotalStatValue);
                TriggerCurrentValueChange();
            }
        }
    }

    public VitalStat() : base()
    {
        CurrentValue = BaseValue;
    }

    public VitalStat(string name, int baseValue) : base(name, baseValue)
    {
        CurrentValue = BaseValue;
    }

    public void SetCurrentValueToMax()
    {
        CurrentValue = TotalStatValue;
    }

    public float GetPercentage()
    {
        return (float)CurrentValue / TotalStatValue;
    }

    private void TriggerCurrentValueChange()
    {
        if (OnCurrentValueChange != null)
        {
            OnCurrentValueChange(this, null);
        }
    }
}
