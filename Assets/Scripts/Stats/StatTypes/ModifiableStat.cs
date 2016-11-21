using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;

[Serializable]
public class ModifiableStat : BaseStat {

    private List<StatModifier> modList;
    private int totalModValue;

    public override int TotalStatValue
    {
        get { return base.TotalStatValue + totalModValue; }
    }

    public ModifiableStat() : base()
    {
        modList = new List<StatModifier>();
        totalModValue = 0;
    }

    public ModifiableStat(string name, int baseValue) : base(name, baseValue) {
        modList = new List<StatModifier>();
        totalModValue = 0;
    }

    public void AddModifier(StatModifier mod)
    {
        modList.Add(mod);
        mod.OnValueChange += OnModValueChange;
        UpdateModifiers();
    }

    public void RemoveModifier(StatModifier mod)
    {
        modList.Remove(mod);
        mod.OnValueChange -= OnModValueChange;
        UpdateModifiers();
    }

    public void ClearModifiers()
    {
        foreach (var mod in modList)
        {
            mod.OnValueChange -= OnModValueChange;
        }
        modList.Clear();
    }

    public void UpdateModifiers()
    {
        totalModValue = 0;

        var orderGroups = modList.OrderBy(m => m.Order).GroupBy(m => m.Order);
        foreach (var group in orderGroups)
        {
            float stackSum = 0, nonStackMax = 0;
            foreach (var mod in group)
            {
                if (mod.Stacks)
                {
                    stackSum += mod.ModValue;
                }
                else
                {
                    if (mod.ModValue > nonStackMax)
                    {
                        nonStackMax = mod.ModValue;
                    }
                }
            }

            totalModValue += group.First().ApplyModifier(TotalStatValue, stackSum > nonStackMax ? stackSum : nonStackMax);
        }
        TriggerValueChange();
    }

    public void OnModValueChange(object modifier, System.EventArgs args)
    {
        UpdateModifiers();
    }
}
