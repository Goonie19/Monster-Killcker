using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class AlliesData 
{
    public AlliesData(int Id, int NumberOfAllies, float baseDamage, float price, float priceMultiplier)
    {
        this.Id = Id;
        this.NumberOfAllies = NumberOfAllies;
        BaseDamage = baseDamage;
        Price = price;
        PriceMultiplier = priceMultiplier;

    }

    public int Id;

    [Title("Ally Damage Parameters")]
    public float BaseDamage;

    [Title("Ally Price Parameters")]
    public float Price;
    public float PriceMultiplier;

    

    public int NumberOfAllies;
}
