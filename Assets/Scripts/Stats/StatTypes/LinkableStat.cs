using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

[Serializable]
public class LinkableStat : ModifiableStat
{
    private List<StatLinker> linkerList;
    private int totalLinkerValue;

    public override int TotalStatValue
    {
        get { return base.TotalStatValue + totalLinkerValue; }
    }

    public LinkableStat() : base() {
        totalLinkerValue = 0;
        linkerList = new List<StatLinker>();
    }

    public LinkableStat(string name, int baseValue) : base(name, baseValue)
    {
        totalLinkerValue = 0;
        linkerList = new List<StatLinker>();
    }

    public void AddLinker(StatLinker linker)
    {
        linkerList.Add(linker);
        linker.OnValueChange += OnLinkerValueChange;
        UpdateLinkers();
    }

    public void RemoveLinker(StatLinker linker)
    {
        linkerList.Remove(linker);
        linker.OnValueChange -= OnLinkerValueChange;
        UpdateLinkers();
    }

    public void ClearLinker()
    {
        foreach (var linker in linkerList)
        {
            linker.OnValueChange -= OnLinkerValueChange;
        }
        linkerList.Clear();
    }

    public void UpdateLinkers()
    {
        totalLinkerValue = 0;
        foreach (StatLinker link in linkerList)
        {
            totalLinkerValue += link.Value;
        }
        TriggerValueChange();
    }

    private void OnLinkerValueChange(object linker, EventArgs args)
    {
        UpdateLinkers();
    }
}
