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
    public bool addToExperience;
    
    [ShowIf("addToHealth")]
    public float HealthAddToBase = 0;
    [ShowIf("addToHealth")]
    public float MultiplyHealth = 1;
    [ShowIf("addToHealth")]
    public float HealthAddToMultiplierEXP = 0;

    [ShowIf("addToHeads")]
    public int HeadsToAdd;
    [ShowIf("addToHeads")]
    public float MultiplierHeads;

    [ShowIf("addToExperience")]
    public float BaseExperienceToAdd = 0;
    [ShowIf("addToExperience")]
    public float MultiplyExperienceValue = 1;

    public bool Unlocked
    {
        get => _unlocked;
        set
        {

            if (!_unlocked && value)
            {
                if (OneUseBuff)
                {
                    if (!Acquired)
                        Instantiate();
                }
                else
                    Instantiate();
                


            }

            _unlocked = value;
        }
    }

    public override void ApplyBuff()
    {

        PlayerManager.Instance.ActualHeads -= (int)_actualPrice;

        if (addToHealth) {
            MonsterManager.Instance.BaseHealth += HealthAddToBase;
            MonsterManager.Instance.BaseHealth *= MultiplyHealth;
            MonsterManager.Instance.HealthPercentageExp += HealthAddToMultiplierEXP;
        }

        if (addToHeads)
        {
            MonsterManager.Instance.BaseHeads += HeadsToAdd;
            MonsterManager.Instance.BaseHeads = (int)(MonsterManager.Instance.BaseHeads * MultiplierHeads);
        }
        if(addToExperience)
        {
            MonsterManager.Instance.BaseExperience += BaseExperienceToAdd;
            MonsterManager.Instance.BaseExperience *= MultiplyExperienceValue;
        }

        if (OneUseBuff)
        {
            Acquired = true;
            UIManager.Instance.DeleteMonsterBuff(Id);
            UIManager.Instance.buffInfoPanel.gameObject.SetActive(false);
        } else
        {
            //In case the multiplier is so little, it adds 1 to the price
            int newPrice = (int)(_actualPrice * PriceMultiplier);
            int difference = newPrice - (int)_actualPrice;
            if (difference == 0)
                difference = 1;

            _actualPrice += difference;
            _actualPrice = (int)_actualPrice;

            ++NumberOfBuffs;
            UIManager.Instance.UpdateMonsterButtoninfo(Id);
        }

        UIManager.Instance.CheckButtonInteraction();
        if(GameManager.Instance.GetAlliesHovers())
            UIManager.Instance.buffInfoPanel.Setup(this);
        UIManager.Instance.UpdateInfoPanels();

        SaveDataManager.Instance.SetBuff(this);
        SaveDataManager.Instance.SaveData();
    }

    public override void Unlock()
    {
        Unlocked = true;
    }

    public override void Instantiate()
    {
        UIManager.Instance.InstantiateMonsterButton(this);
    }
}
