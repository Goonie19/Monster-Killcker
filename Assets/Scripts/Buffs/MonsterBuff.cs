using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MonsterBuff", menuName = "Buffs/MonsterBuff")]
public class MonsterBuff : Buff
{

    public bool addToHealth;
    public bool addToHeads;

    [Title("Price")]
    public int HeadsPrice;

    
    [ShowIf("addToHealth")]
    public float HealthAddToBase;
    [ShowIf("addToHeads")]
    public int HeadsToAdd;

    public bool Unlocked
    {
        get => _unlocked;
        set
        {
            if (!_unlocked && value)
                UIManager.Instance.InstantiateMonsterButton(this);

            _unlocked = value;
        }
    }

    public override void ApplyBuff()
    {
        
        if(addToHealth)
            MonsterManager.Instance.BaseHealth += HealthAddToBase;
        if (addToHeads)
            MonsterManager.Instance.BaseHeads += HeadsToAdd;

        if (OneUseBuff)
        {
            Acquired = true;
            UIManager.Instance.DeleteMonsterBuff(Id);
        } else
        {
            ++NumberOfBuffs;
            UIManager.Instance.UpdateMonsterButtoninfo(Id);
        }
    }

    public override void Unlock()
    {
        Unlocked = true;
    }
}
