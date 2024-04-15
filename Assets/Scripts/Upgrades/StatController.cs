using System;
using System.Collections.Generic;
using System.Linq;

public class StatsController<Stats> where Stats : ICloneable
{
    private readonly Stats BaseStats;
    private List<StatsChange<Stats>> StatsChanges = new();
    
    public Stats CurrentStats
    {
        get => CalculateStats();
    }

    public StatsChange<Stats>[] GetModifications()
    {
        return StatsChanges.ToArray();
    }

    public StatsController(Stats baseStats)
    {
        BaseStats = baseStats;
    }

    public void Modify(StatsChange<Stats> statsChange)
    {
        StatsChanges.Add(statsChange);
        StatsChanges = StatsChanges.OrderBy(change => change.Order).ToList();
    }

    public void RemoveModification(StatsChange<Stats> statsChange)
    {
        StatsChanges.Remove(statsChange);
        StatsChanges = StatsChanges.OrderBy(change => change.Order).ToList();
    }

    private Stats CalculateStats()
    {
        Stats currentStats = (Stats)BaseStats.Clone();
        foreach (StatsChange<Stats> statsChange in StatsChanges)
        {
             statsChange.Modification.Invoke(currentStats);
        }
        return currentStats;
    }

}
