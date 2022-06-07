using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class AllyType
{
    [Title("Ally Id")]
    public int AllyId;

    [Title("Ally References")]
    public string AllyName;
    public Sprite AllySprite;
    public Image AllyImageReference;

    [Title("Ally Damage Parameters")]
    public float BaseDamage;
    public float DamageMultiplier;

    [Title("Ally Parameters")]
    public int NumberOfAllies;

    
}
