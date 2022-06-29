using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerBuff", menuName = "Buffs/PlayerBuff")]
public class PlayerBuff : Buff {

    [Title("Player Parameters")]
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
                UIManager.Instance.InstantiatePlayerBuffButton(this);

            _unlocked = value;
        }
    }

    public override void ApplyBuff()
    {

        PlayerManager.Instance.ActualExperience -= _actualPrice;

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
            _actualPrice *= PriceMultiplier;

            ++NumberOfBuffs;
            UIManager.Instance.UpdatePlayerButtoninfo(Id);
        }

        UIManager.Instance.CheckButtonInteraction();
    }

    public override void Unlock()
    {
        Unlocked = true;
    }
}
