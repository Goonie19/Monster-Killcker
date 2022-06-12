using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerBuff", menuName = "Buffs/PlayerBuff")]
public class PlayerBuff : Buff {

    [Title("Player Parameters")]
    public bool addToBaseDamage;
    public bool addToMultiplierDamage;

    [Title("Price")]
    public float ExpPrice;

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
                UIManager.Instance.InstantiatePlayerBuffButton(this);

            _unlocked = value;
        }
    }

    public override void ApplyBuff()
    {

        if (addToBaseDamage)
            PlayerManager.Instance.BaseDamage += DamageAddToBase;
        if (addToMultiplierDamage)
            PlayerManager.Instance.DamageMultiplier += MultiplierToAdd;

        if (OneUseBuff)
        {
            Acquired = true;
            UIManager.Instance.DeletePlayerBuff(Id);
        } else
        {
            ++NumberOfBuffs;
            UIManager.Instance.UpdatePlayerButtoninfo(Id);
        }
    }

    public override void Unlock()
    {
        Unlocked = true;
    }
}
