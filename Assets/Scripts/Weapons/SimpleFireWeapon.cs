using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleFireWeapon : PlayerWeapon
{
    private bool IsShooting = false;

    override protected void Start()
    {
        base.Start();
        OnPressedActionButton += StartShooting;
        OnCancelActionButton += StopShooting;
        OnCooldownEnd += RepeatShot;
    }

    private void RepeatShot()
    {
        if (IsShooting) Shoot(transform.right);
    }

    private void StopShooting()
    {
        IsShooting = false;
    }

    private void StartShooting()
    {
        IsShooting = true;
        Shoot(transform.right);
    }
}
