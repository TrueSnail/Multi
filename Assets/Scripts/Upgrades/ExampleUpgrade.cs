using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExampleUpgrade : MonoBehaviour
{
    public float StartFireRatePercentage;
    public float IncreaseFireRatePercentage;
    public float MaxFireRatePercentage;

    private int Stacks = 0;

    private void Start()
    {
        PlayerWeapon weapon = GetComponent<PlayerWeapon>();
        weapon.OnCancelActionButton += ResetStacks;
        weapon.OnAfterShot += AddStack;

        StatsChange<WeaponStats> statsChange = new(Modification, 1, this);
        weapon.WeaponStatsController.Modify(statsChange);
    }

    private void Modification(WeaponStats stats)
    {
        stats.FireRate = Mathf.Min((stats.FireRate * StartFireRatePercentage) + (stats.FireRate * IncreaseFireRatePercentage * Stacks), stats.FireRate * MaxFireRatePercentage);
    }

    private void ResetStacks()
    {
        Stacks = 0;
    }

    private void AddStack()
    {
        Stacks++;
    }
}
