using Sirenix.OdinInspector;
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
            _totalExperienceAchieved += value;

            SaveDataManager.Instance.SetPlayerActualExperience(_actualExperience);
            SaveDataManager.Instance.SetPlayerTotalExperience(_totalExperienceAchieved);

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

    public float TotalExperience
    {
        get => _totalExperienceAchieved;
    }

    public float GameTime
    {
        get => _gameTime;
    }

    public float TotalDamage
    {
        get => _totalDamage;
        set
        {
            _totalDamage = value;

            SaveDataManager.Instance.SetPlayerTotalDamage(_totalDamage);
        }
    }

    public bool InBattle
    {
        get => _inBattle;
        set => _inBattle = value;
    }

    public bool PassTime
    {
        get => _passTime;
        set => _passTime = value;
    }

    private float _baseDamage;

    private float _actualExperience;
    private int _actualHeads;

    private int _totalHeadsAchieved;
    private float _totalExperienceAchieved;
    private float _totalDamage;
    private float _gameTime;

    private bool _inBattle;
    private bool _passTime;

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

        if(_passTime)
        {
            _gameTime += Time.deltaTime;
            SaveDataManager.Instance.SetPlayerGameTime(_gameTime);
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
        _totalDamage = parameters.TotalDamage;
        _totalExperienceAchieved = parameters.TotalExperience;
        _gameTime = parameters.GameTime;

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

    public int GetTotalMonsterBuffs()
    {
        int numberOfBuffs = 0;

        foreach(Buff b in buffs)
        {
            if( b is MonsterBuff)
            {
                if(b.OneUseBuff)
                    numberOfBuffs++;
                else
                    numberOfBuffs += b.NumberOfBuffs;
            }
        }

        return numberOfBuffs;
    }

    public int GetTotalPlayerBuffs()
    {
        int numberOfBuffs = 0;

        foreach (Buff b in buffs)
        {
            if (b is PlayerBuff)
            {
                if (b.OneUseBuff)
                    numberOfBuffs++;
                else
                    numberOfBuffs += b.NumberOfBuffs;
            }
        }

        return numberOfBuffs;
    }

    public int GetTotalAlliesBuffs()
    {
        int numberOfBuffs = 0;

        foreach (Buff b in buffs)
        {
            if (b is AllyBuff)
            {
                if (b.OneUseBuff)
                    numberOfBuffs++;
                else
                    numberOfBuffs += b.NumberOfBuffs;
            }
        }

        return numberOfBuffs;
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
