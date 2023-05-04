﻿using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Instance;

    [Title("Damage of Player")]
    public float BaseDamage
    {
        get => _baseDamage;
        set
        {
            _baseDamage = value;

            SaveDataManager.Instance.SetPlayerBaseDamage(_baseDamage);
        }
    }

    [Title("Buff List")]
    public List<Buff> buffs;

    public float ActualExperience
    {
        get => _actualExperience;
        set
        {
            _actualExperience = value;

            SaveDataManager.Instance.SetPlayerActualExperience(_actualExperience);

            UIManager.Instance.ExperienceDisplayText.text = UIManager.Instance.SimplifyNumber(_actualExperience);

            Sequence sq = DOTween.Sequence();

            sq.Append(UIManager.Instance.ExperienceIcon.transform.DOScale(1.1f, 0.1f).SetEase(Ease.Linear));
            sq.Append(UIManager.Instance.ExperienceIcon.transform.DOScale(1f, 0.1f).SetEase(Ease.Linear));

            sq.Play();
        }
    }

    public int ActualHeads
    {
        get => _actualHeads;
        set
        {
            int difference = value - _actualHeads;

            _actualHeads = value;

            if(difference > 0)
                _totalHeadsAchieved += difference;

            SaveDataManager.Instance.SetPlayerActualHeads(_actualHeads);

            SaveDataManager.Instance.SetPlayerTotalHeads(_totalHeadsAchieved);

            UIManager.Instance.HeadsDisplayText.text = UIManager.Instance.SimplifyNumber(_actualHeads);

            if (_totalHeadsAchieved >= BossManager.Instance.HeadsToUnlock)
                UIManager.Instance.BossAppearButton.SetActive(true);

            Sequence sq = DOTween.Sequence();

            sq.Append(UIManager.Instance.HeadsIcon.transform.DOScale(1.1f, 0.1f).SetEase(Ease.Linear));
            sq.Append(UIManager.Instance.HeadsIcon.transform.DOScale(1f, 0.1f).SetEase(Ease.Linear));

            sq.Play();
        }
    }

    public int TotalHeads
    {
        get => _totalHeadsAchieved;
    }

    public bool InBattle
    {
        get => _inBattle;
        set => _inBattle = value;
    }

    private float _baseDamage;

    private float _actualExperience;
    private int _actualHeads;

    private int _totalHeadsAchieved;

    private bool _inBattle;

    private void Awake()
    {
        Instance = this;

        if (buffs.Count != 0)
        {
            foreach (Buff b in buffs)
            {
                b.SetUnlockedToFalse();
                b.Reset();
            }
        }

        UIManager.Instance.OnFadeOut.AddListener(() => _inBattle = true);

    }

    void Start()
    {
        InitializeParameters();

        buffs.Sort((x, y) => x.HeadsToUnlock.CompareTo(y.HeadsToUnlock));

        UIManager.Instance.OnFadeOut.AddListener(CheckBuffs);
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(1)) { 
            GameManager.Instance.TenShopMode = !GameManager.Instance.TenShopMode;

        }
    }

    void InitializeParameters()
    {
        PlayerData parameters;

        if (SaveDataManager.Instance.CanGetData())
            parameters = SaveDataManager.Instance.GetPlayerParameters();
        else
            parameters = new PlayerData(GameManager.Instance.DefaultPlayerData);

        _totalHeadsAchieved = parameters.TotalHeads;

        ActualExperience = parameters.ActualExperience;
        ActualHeads = parameters.ActualHeads;

        _baseDamage = parameters.BaseDamage;

        List<BuffData> buffsAcquired = SaveDataManager.Instance.GetBuffsAcquired();

        foreach(BuffData b in buffsAcquired)
        {
            Buff bufo = buffs.Find(X => X.Id == b.BuffId);

            bufo.NumberOfBuffs = b.NumberOfBuffs;
            bufo.Acquired = true;
            bufo.SetActualPrice(b.ActualPrice);

        }

        

    }

    public void CheckBuffs()
    {

        AllyManager.Instance.CheckAllies();

        foreach (Buff b in buffs)
        {
            if (b.HeadsToUnlock <= TotalHeads)
            {
                b.Unlock();
            }
            
        }

        UIManager.Instance.CheckButtonInteraction();
    }

    /*void SilentInitBuffs()
    {
        AllyManager.Instance.CheckAllies();

        foreach (Buff b in buffs)
        {
            if (b.HeadsToUnlock <= TotalHeads)
            {
                if (b.GetUnlocked())
                {
                    if (b.OneUseBuff)
                    {
                        if (!b.Acquired)
                            b.Instantiate();
                    }
                    else
                        b.Instantiate();

                }
                else
                    b.Unlock();

            }
        }

        UIManager.Instance.CheckButtonInteraction();
    }*/
}
