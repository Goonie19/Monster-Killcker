using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MonsterBuff", menuName = "Buffs/MonsterBuff")]
public class MonsterBuff : Buff
{
    [Title("Monster parameters")]
    public bool addToHealth;
    public bool addToHeads;

    
    [ShowIf("addToHealth")]
    public float HealthAddToBase;
    [ShowIf("addToHealth")]
    public float HealthAddToMultiplier;
    [ShowIf("addToHealth")]
    public float HealthAddToMultiplierEXP;

    [ShowIf("addToHeads")]
    public int HeadsToAdd;
    [ShowIf("addToHeads")]
    public int MultiplierAdd;

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

        PlayerManager.Instance.ActualHeads -= (int)_actualPrice;

        if (addToHealth) {
            MonsterManager.Instance.BaseHealth += HealthAddToBase;
            MonsterManager.Instance.HealthMultiplier += HealthAddToMultiplier;
            MonsterManager.Instance.HealthPercentageExp += HealthAddToMultiplierEXP;
        }

        if (addToHeads)
            MonsterManager.Instance.BaseHeads += HeadsToAdd;

        if (OneUseBuff)
        {
            Acquired = true;
            UIManager.Instance.DeleteMonsterBuff(Id);
        } else
        {
            _actualPrice *= PriceMultiplier;
            _actualPrice = (int)_actualPrice;

            ++NumberOfBuffs;
            UIManager.Instance.UpdateMonsterButtoninfo(Id);
        }

        UIManager.Instance.CheckButtonInteraction();
    }

    public override void SetBuffPanelInfo()
    {
        /*UIManager.Instance.AllyBuffPanel.BuffTitle.text = BuffName;
        UIManager.Instance.AllyBuffPanel.BuffDescription.text = BuffDescription;

        if (OneUseBuff)
            UIManager.Instance.AllyBuffPanel.StatsDisplay.gameObject.SetActive(false);
        else
        {
            UIManager.Instance.AllyBuffPanel.StatsDisplay.gameObject.SetActive(true);

            if (addToHealth)
            {
                if(HealthAddToBase > 0)
                    UIManager.Instance.AllyBuffPanel.DamagePerSecondText.text = "+" + HealthAddToBase.ToString();

                if(HealthAddToMultiplier > 0)
                    UIManager.Instance.AllyBuffPanel. = "+" + HealthAddToBase.ToString();


            }
            else if (addToHeads)
                UIManager.Instance.AllyBuffPanel.DamagePerSecondText.text = "x" + HeadsToAdd.ToString();

        }
        */
    }

    public override void Unlock()
    {
        Unlocked = true;
    }
}
