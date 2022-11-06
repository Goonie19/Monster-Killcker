using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class AllyType
{
    [Title("Ally Id")]
    public int AllyId;

    [Title("Ally References")]
    public string AllyName;
    public Sprite AllySprite;
    public Sprite Icon;
    public Image AllyImageReference;

    [Title("Ally Damage Parameters")]
    public float BaseDamage;
    public float DamageMultiplier;

    [Title("Ally Price Parameters")]
    public float Price;
    public float PriceMultiplier;

    [Title("Ally Parameters")]
    [TextArea(3, 5)]
    public string Description;

    [Space(20)]
    public int HeadsToUnlock;
    public int NumberOfAllies
    {
        get => _numberOfAllies;
        set
        {
            //this is for when the boss attacks allies
            bool attack = false;

            if(value < _numberOfAllies)
                attack = true;

            _numberOfAllies = value;
            UIManager.Instance.UpdateAllyInfo(AllyId, attack);
            AllyManager.Instance.UpdateAllies(AllyId);
        }
    }

    private int _numberOfAllies;
    

    public bool Unlocked
    {
        get => _unlocked;
        set
        {
            if (!_unlocked && value)
                UIManager.Instance.InstantiateAlly(this);
                

            _unlocked = value;
            AllyManager.Instance.UpdateAllies(AllyId);
        }
    }

    private bool _unlocked;

    public void BuyAlly()
    {

        PlayerManager.Instance.ActualExperience -= Price;

        /*if (NumberOfAllies == 0)
        {
            //AllyImageReference.gameObject.SetActive(true);
            AllyImageReference.sprite = AllySprite;
            AllyImageReference.SetNativeSize();
        }*/

        ++NumberOfAllies;

        Price *= PriceMultiplier;

        AudioManager.Instance.PlayBuySound();

        UIManager.Instance.CheckButtonInteraction();

        UIManager.Instance.UpdateAllyInfo(AllyId);

        UIManager.Instance.allyInfoPanel.Setup(this);

        AllyManager.Instance.UpdateAllies(AllyId);

        SaveDataManager.Instance.SaveData();

    }

    public void Unlock()
    {
        Unlocked = true;
    }

    public void SilentUnlock()
    {
        _unlocked = true;
    }

    public float GetPrice()
    {
        return Price;
    }

    public float GetDamage()
    {
        return BaseDamage * DamageMultiplier * NumberOfAllies;
    }

    public void SetSilentNumberOfAllies(int number)
    {
        _numberOfAllies = number;
    }

    public void SetSilentActualPrice(float p)
    {
        Price = p;
    }

    public void SetSilentUnlocked(bool u)
    {
        _unlocked = u;
    }
}
