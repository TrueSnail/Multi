using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponStats : MonoBehaviour, ICloneable
{
    [Tooltip("Amount of shots per second")]
    public float FireRate = 2;
    [Tooltip("How many bullets spawns pre shot")]
    public int BulletCount = 1;
    [Tooltip("How fast spawned bullets move")]
    public float ShotSpeed = 60;
    [Tooltip("How much to knockback shooting object")]
    public float ShootKnockback = 240;
    [Tooltip("Degree by witch each bullet is offset when shot")]
    public float ShootSpread = 2;

    public object Clone() => MemberwiseClone();
}
