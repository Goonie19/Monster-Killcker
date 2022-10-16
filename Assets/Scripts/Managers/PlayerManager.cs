using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    public float DamageMultiplier { get => _damageMultiplier;
        set
        {
            _damageMultiplier = value;

            SaveDataManager.Instance.SetPlayerDamageMultiplier(_damageMultiplier);
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

            if (_actualExperience % 1 == 0)
                UIManager.Instance.ExperienceDisplayText.text = _actualExperience.ToString();
            else
                UIManager.Instance.ExperienceDisplayText.text = string.Format("{0:0.00}", ActualExperience);
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

            UIManager.Instance.HeadsDisplayText.text = _actualHeads.ToString();

            if (_totalHeadsAchieved >= BossManager.Instance.HeadsToUnlock)
                UIManager.Instance.BossAppearButton.SetActive(true);
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
    private float _damageMultiplier;

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
        UIManager.Instance.OnFadeOut.AddListener(CheckBuffs);
    }

    void InitializeParameters()
    {
        PlayerData parameters;

        if (SaveDataManager.Instance.CanGetData())
            parameters = SaveDataManager.Instance.GetPlayerParameters();
        else
            parameters = GameManager.Instance.DefaultPlayerData;


        ActualExperience = parameters.ActualExperience;
        ActualHeads = parameters.ActualHeads;

        _totalHeadsAchieved = parameters.TotalHeads;

        _baseDamage = parameters.BaseDamage;
        _damageMultiplier = parameters.DamageMultiplier;

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
}
