using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

[Serializable]
public class StatCollection {

    [SerializeField]
    private StatDictionary<String, BaseStat> statDictionary = new StatDictionary<String, BaseStat>();

    public StatDictionary<String, BaseStat> StatDictionary
    {
        get { return statDictionary; }
    }

    public StatCollection()
    {
        SetBaseStats();
    }

    protected virtual void SetBaseStats() { }

    public bool ContainStat(String statName)
    {
        return statDictionary.ContainsKey(statName);
    }

    public BaseStat GetStat(String statName)
    {
        if (ContainStat(statName))
        {
            return statDictionary[statName];
        }
        return null;
    }

    public T GetStat<T>(String statName) where T : BaseStat
    {
        return GetStat(statName) as T;
    }

    protected T CreateStat<T>(String statName, int baseValue) where T : BaseStat
    {
        T stat = Activator.CreateInstance<T>();
        stat.BaseValue = baseValue;
        stat.Name = statName;
        statDictionary.Add(statName, stat);
        return stat;
    }

    public virtual void AddStat<T>(String statName, int baseValue) where T : BaseStat
    {
        T stat = GetStat<T>(statName);
        if (stat == null)
        {
            stat = CreateStat<T>(statName , baseValue);
        }
        else
        {
            stat.BaseValue += baseValue;
        }
    }

    public virtual void RemoveStat<T>(String statType, int baseValue) where T : BaseStat
    {
        T stat = GetStat<T>(statType);
        if (stat != null)
        {
            stat.BaseValue -= baseValue;
            if (stat.BaseValue <= 0)
            {
                statDictionary.Remove(statType);
            }
        }
    }

    public void TransferStats(StatCollection collection)
    {
        foreach (var key in collection.StatDictionary.Keys)
        {
            AddStat<BaseStat>(key, collection.GetStat(key).BaseValue);
        }
    }

    public void RemoveStats(StatCollection collection)
    {
        foreach (var key in collection.StatDictionary.Keys)
        {
            RemoveStat<BaseStat>(key, collection.GetStat(key).BaseValue);
        }
    }

    public string StatList()
    {
        string statList = string.Empty;
        foreach (var key in statDictionary.Keys)
        {
            BaseStat s = GetStat(key);
            if (s.GetType().Equals(typeof(VitalStat))){
                statList += "<color=orange>";
                statList += "\n" + s.Name + ": " + ((VitalStat)s).CurrentValue+"/"+s.TotalStatValue + "</color>";
            }
            else if (s.GetType().Equals(typeof(LinkableStat))){
                statList += "<color=blue>";
                statList += "\n" + s.Name + ": " + s.BaseValue + "</color>";
            }
            else if (s.GetType().Equals(typeof(LinkableStat)))
            {
                statList += "<color=green>";
                statList += "\n" + s.Name + ": " + s.BaseValue + "</color>";
            }
            else
            {
                statList += "<color=white>";
                statList += "\n" + s.Name + ": " + s.BaseValue + "</color>";
            }
        }
        return statList;
    }

    public override bool Equals(object obj)
    {
        if (obj == null) return false;

        StatCollection statCol = obj as StatCollection;
        if (statCol == null) return false;
        return this.StatDictionary.Equals(statCol.StatDictionary);
    }

    public override int GetHashCode()
    {
        return this.StatDictionary.GetHashCode();
    }
}
