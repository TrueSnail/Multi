using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletStats : MonoBehaviour, ICloneable
{
    [Tooltip("Damage of the bullet")]
    public float Damage = 20;
    [Tooltip("Rigidbody mass, affects knockback of shot game objects")]
    public float Mass = 0.3f;
    [Tooltip("Time in seconds after witch bullets despawn")]
    public float Livetime = 5;
    [Tooltip("Size of bullets")]
    public float Size = 0.2f;
    [Tooltip("Gravity effect scale")]
    public float Gravity = 0;

    public object Clone() => MemberwiseClone();
}
