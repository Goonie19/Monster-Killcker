using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MonsterBuff", menuName = "Buffs/MonsterBuff")]
public class MonsterBuff : Buff
{
    [Title("Monster Buff Parameters")]
    public int MonsterId;

    public bool addToHealth;
    public bool addToHeads;

    [ShowIf("addToHealth")]
    public float HealthAddToBase;
    [ShowIf("addToHeads")]
    public int HeadsToAdd;

    public override void ApplyBuff()
    {
        if (OneUseBuff)
            Acquired = true;

        if(addToHealth)
            MonsterManager.Instance.BaseHealth += HealthAddToBase;
        if (addToHeads)
            MonsterManager.Instance.BaseHeads += HeadsToAdd;
    }

}
