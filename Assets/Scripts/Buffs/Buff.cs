using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


public abstract class Buff : ScriptableObject
{
    [Title("Buff base parameters")]
    public int Id;

    public Sprite Icon;

    public string BuffName;
    [TextArea(3,5)]
    public string BuffDescription;
    
    [Title("Price parameters")]
    public int HeadsToUnlock;
    public float Price;

    [HideIf("OneUseBuff")]
    public float PriceMultiplier;

    [Title("Bools for diferent behaviours")]
    public bool OneUseBuff;

    [ShowIf("OneUseBuff")]
    public bool Acquired;

    [HideIf("OneUseBuff")]
    public int NumberOfBuffs;

    protected bool _unlocked = false;

    protected float _actualPrice;

    [ContextMenu("Lock")]
    public void SetUnlockedToFalse()
    {
        _unlocked = false;
    }

    public abstract void Unlock();

    public abstract void ApplyBuff();

    public void Reset()
    {
        _unlocked = false;
        Acquired = false;
        NumberOfBuffs = 0;
        _actualPrice = Price;
    }

    public float GetPrice()
    {
        return _actualPrice;
    }

    public void SetActualPrice(float price)
    {
        _actualPrice = price;
    }

}
