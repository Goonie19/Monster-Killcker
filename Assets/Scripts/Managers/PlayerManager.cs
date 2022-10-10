using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Instance;

    [Title("Damage of Player")]
    public float BaseDamage = 1;
    public float DamageMultiplier = 1;

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

    private float _actualExperience;
    private int _actualHeads;

    private int _totalHeadsAchieved;

    private bool _inBattle = true;

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

    }

    void Start()
    {
        InitializeParameters();
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

        BaseDamage = parameters.BaseDamage;
        DamageMultiplier = parameters.DamageMultiplier;
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
