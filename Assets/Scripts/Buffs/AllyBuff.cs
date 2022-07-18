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

        PlayerManager.Instance.ActualExperience -= _actualPrice;

        UIManager.Instance.CheckButtonInteraction();

        //Applying buff to specific ally
        int i = AllyManager.Instance.allies.FindIndex((x) => x.AllyId == AllyId);

        if (addToBaseDamage)
            AllyManager.Instance.allies[i].BaseDamage += DamageAddToBase;
        if(addToMultiplierDamage)
            AllyManager.Instance.allies[i].DamageMultiplier += MultiplierToAdd;
        
        

        //If it for one use only, it erases the button.
        if (OneUseBuff)
        {
            Acquired = true;
            UIManager.Instance.DeleteAllyBuff(Id);

        } else
        {
            _actualPrice *= PriceMultiplier;

            ++NumberOfBuffs;
            UIManager.Instance.UpdateAllyButtoninfo(Id);
            
        }

        UIManager.Instance.buffInfoPanel.Setup(this);

    }

    public override void Unlock()
    {
        Unlocked = true;
    }
}
