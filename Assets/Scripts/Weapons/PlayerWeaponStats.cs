using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeaponStats : MonoBehaviour, ICloneable
{
    [Tooltip("How fast weapon rotates towards the cursor")]
    public float TurningSpeed = float.PositiveInfinity;
    [Tooltip("How accurate weapon turns, 1 means perfect accuracy, 0 means 45 degrees difference")]
    public float TurningAccuracy = 1;
    [Tooltip("How fast noise changes")]
    public float AccuracySpeed = 6;
    [Tooltip("How much weapon sways after shooting")]
    public float RecoilStrenght = 10;
    [Tooltip("How fast weapon stabilizes after applying recoil")]
    public float RecoilDecay = 15;

    public object Clone() => MemberwiseClone();
}
