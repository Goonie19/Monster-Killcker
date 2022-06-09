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

    public override void ApplyBuff()
    {
        if (addToBaseDamage)
            PlayerManager.Instance.BaseDamage += DamageAddToBase;
        if (addToMultiplierDamage)
            PlayerManager.Instance.DamageMultiplier += MultiplierToAdd;
    }
}
