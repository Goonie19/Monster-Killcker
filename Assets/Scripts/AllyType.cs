﻿using Sirenix.OdinInspector;
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
            {
                _actualPrice = Price;
                UIManager.Instance.InstantiateAlly(this);
                
            }

            _unlocked = value;
            AllyManager.Instance.UpdateAllies(AllyId);
        }
    }

    private bool _unlocked;
    private float _actualPrice;

    public void BuyAlly()
    {

        PlayerManager.Instance.ActualExperience -= _actualPrice;

        /*if (NumberOfAllies == 0)
        {
            //AllyImageReference.gameObject.SetActive(true);
            AllyImageReference.sprite = AllySprite;
            AllyImageReference.SetNativeSize();
        }*/

        ++NumberOfAllies;

        _actualPrice *= PriceMultiplier;

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

    public float GetPrice()
    {
        return _actualPrice;
    }

    public float GetDamage()
    {
        return BaseDamage * DamageMultiplier * NumberOfAllies;
    }

}
