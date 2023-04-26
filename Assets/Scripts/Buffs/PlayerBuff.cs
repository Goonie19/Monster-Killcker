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
    public float DamageAddToBase = 0;
    [ShowIf("addToMultiplierDamage")]
    public float MultiplierDamage = 1;

    public bool Unlocked
    {
        get => _unlocked;
        set
        {
            if (!_unlocked && value)
            {
                if(OneUseBuff)
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

        PlayerManager.Instance.ActualExperience -= _actualPrice;

        if (addToBaseDamage)
            PlayerManager.Instance.BaseDamage += DamageAddToBase;
        if (addToMultiplierDamage)
            PlayerManager.Instance.BaseDamage *= MultiplierDamage;

        if (OneUseBuff)
        {
            Acquired = true;
            UIManager.Instance.DeletePlayerBuff(Id);
            UIManager.Instance.buffInfoPanel.gameObject.SetActive(false);
        } else
        {
            _actualPrice *= PriceMultiplier;
            Acquired = true;
            ++NumberOfBuffs;
            UIManager.Instance.UpdatePlayerButtoninfo(Id);
        }

        AudioManager.Instance.PlayBuySound();

        UIManager.Instance.CheckButtonInteraction();
        if(GameManager.Instance.GetAlliesHovers())
            UIManager.Instance.buffInfoPanel.Setup(this);
        UIManager.Instance.UpdateInfoPanels();

        SaveDataManager.Instance.SetBuff(this);
        SaveDataManager.Instance.SaveData();

    }

    public override void Instantiate()
    {
        UIManager.Instance.InstantiatePlayerBuffButton(this);
    }

    public override void Unlock()
    {
        Unlocked = true;
    }
}
