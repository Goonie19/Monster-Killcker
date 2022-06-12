using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AllyBuff", menuName = "Buffs/AllyBuff")]
public class AllyBuff : Buff
{
    [Title("Ally Parameters")]
    public int AllyId;

    [Title("Ally Buff Adds")]
    public bool addToBaseDamage;
    public bool addToMultiplierDamage;
    public bool addAnAlly;

    [Title("Price")]
    public float ExpPrice;
    [HideIf("OneUseBuff")]
    public float PriceMultiplier;

    
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

        //Applying buff to specific ally
        int i = AllyManager.Instance.allies.FindIndex((x) => x.AllyId == AllyId);

        if (addToBaseDamage)
            AllyManager.Instance.allies[i].BaseDamage += DamageAddToBase;
        if(addToMultiplierDamage)
            AllyManager.Instance.allies[i].DamageMultiplier += MultiplierToAdd;
        if (addAnAlly)
        {
            ++NumberOfBuffs;
            AllyManager.Instance.BuyAlly(AllyId);
        }

        //If it for one use only, it erases the button.
        if (OneUseBuff)
        {
            Acquired = true;
            UIManager.Instance.DeleteAllyBuff(Id);

        }
    }

    public override void Unlock()
    {
        Unlocked = true;
    }
}
