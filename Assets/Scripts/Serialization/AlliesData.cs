using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class AlliesData 
{
    public AlliesData(int Id, int NumberOfAllies, float baseDamage, float damageMultiplier, float price, float priceMultiplier)
    {
        this.Id = Id;
        this.NumberOfAllies = NumberOfAllies;
        BaseDamage = baseDamage;
        DamageMultiplier = damageMultiplier;
        Price = price;
        PriceMultiplier = priceMultiplier;

    }

    public int Id;

    [Title("Ally Damage Parameters")]
    public float BaseDamage;
    public float DamageMultiplier;

    [Title("Ally Price Parameters")]
    public float Price;
    public float PriceMultiplier;

    

    public int NumberOfAllies;
}
