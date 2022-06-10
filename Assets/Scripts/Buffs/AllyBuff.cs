﻿using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AllyBuff", menuName = "Buffs/AllyBuff")]
public class AllyBuff : Buff
{
    [Title("Ally Parameters")]
    public int AllyId;

    public bool addToBaseDamage;
    public bool addToMultiplierDamage;

    [ShowIf("addToBaseDamage")]
    public float DamageAddToBase;
    [ShowIf("addToMultiplierDamage")]
    public float MultiplierToAdd;

    public bool Unlocked
    {
        get => _unlocked;
        set
        {
            if (!_unlocked && value)
                UIManager.Instance.InstantiateAllyButton(this);

            _unlocked = value;
        }
    }

    public override void ApplyBuff()
    {
        if (OneUseBuff)
            Acquired = true;

        int i = AllyManager.Instance.allies.FindIndex((x) => x.AllyId == AllyId);

        if (addToBaseDamage)
            AllyManager.Instance.allies[i].BaseDamage += DamageAddToBase;
        if(addToMultiplierDamage)
            AllyManager.Instance.allies[i].DamageMultiplier += MultiplierToAdd;
    }

    public override void Unlock()
    {
        Unlocked = true;
    }
}
