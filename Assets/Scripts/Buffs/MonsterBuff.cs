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
    public float HealthAddToBase;
    [ShowIf("addToHealth")]
    public float HealthAddToMultiplier;
    [ShowIf("addToHealth")]
    public float HealthAddToMultiplierEXP;

    [ShowIf("addToHeads")]
    public int HeadsToAdd;
    [ShowIf("addToHeads")]
    public int MultiplierAdd;

    [ShowIf("addToExperience")]
    public float BaseExperienceToAdd;
    [ShowIf("addToExperience")]
    public float MultiplierExperienceToAdd;

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
            MonsterManager.Instance.HealthMultiplier += HealthAddToMultiplier;
            MonsterManager.Instance.HealthPercentageExp += HealthAddToMultiplierEXP;
        }

        if (addToHeads)
            MonsterManager.Instance.BaseHeads += HeadsToAdd;

        if(addToExperience)
        {
            MonsterManager.Instance.BaseExperience += BaseExperienceToAdd;
            MonsterManager.Instance.ExperienceMultiplier += MultiplierExperienceToAdd;
        }

        if (OneUseBuff)
        {
            Acquired = true;
            UIManager.Instance.DeleteMonsterBuff(Id);
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

        AudioManager.Instance.PlayBuySound();

        UIManager.Instance.CheckButtonInteraction();
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
