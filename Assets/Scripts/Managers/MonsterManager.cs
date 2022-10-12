using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterManager : MonoBehaviour
{

    public static MonsterManager Instance;

    [Title("Monster Health Related Atributes")]
    public float BaseHealth
    {
        get => _baseHealth;
        set
        {
            _baseHealth = value;

            SaveDataManager.Instance.SetMonsterBaseHealth(_baseHealth);
        }
    }
    public float HealthMultiplier
    {
        get => _healthMultiplier;
        set
        {
            _healthMultiplier = value;

            SaveDataManager.Instance.SetMonsterHealthMultiplier(_healthMultiplier);
        }
    }
    public float HealthPercentageExp
    {
        get => _healthPercentageExp;
        set
        {
            _healthPercentageExp = value;

            SaveDataManager.Instance.SetMonsterHealthPercentageMultiplier(_healthPercentageExp);
        }
    }

    [Title("Monster Possible Sprites")]
    public List<Sprite> MonsterSprites;
    [TextArea(0,3)]
    public List<string> MonsterPhrases;

    [Title("Monster Heads Related Atributes")]
    public int BaseHeads
    {
        get => _baseHeads;
        set
        {
            _baseHeads = value;

            SaveDataManager.Instance.SetMonsterBaseHeads(_baseHeads);
        }
    }
    public float MultiplierHeads
    {
        get => _multiplierHeads;
        set
        {
            _multiplierHeads = value;

            SaveDataManager.Instance.SetMonsterMultiplierHeads(_multiplierHeads);
        }
    }

    [Title("Monster Experience Related Atributes")]
    public float BaseExperience
    {
        get => _baseExperience;
        set
        {
            _baseExperience = value;

            SaveDataManager.Instance.SetMonsterBaseExperience(_baseExperience);
        }
    }
    public float ExperienceMultiplier
    {
        get => _experienceMultiplier;
        set
        {
            _experienceMultiplier = value;

            SaveDataManager.Instance.SetMonsterExperienceMultiplier(_experienceMultiplier);
        }
    }

    private float _baseHealth;
    private float _healthMultiplier;
    private float _healthPercentageExp;

    private int _baseHeads;
    private float _multiplierHeads;

    private float _baseExperience;
    private float _experienceMultiplier;



    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        InitializeParameters();
    }

    void InitializeParameters()
    {
        MonsterData parameters;

        if (SaveDataManager.Instance.CanGetData())
            parameters = SaveDataManager.Instance.GetMonsterParameters();
        else
            parameters = GameManager.Instance.DefaultMonsterData;

        _baseHealth = parameters.BaseHealth;
        _healthMultiplier = parameters.HealthMultiplier;
        _healthPercentageExp = parameters.HealthPercentageMultiplier;

        _baseHeads = parameters.BaseHeads;
        _multiplierHeads = parameters.MultiplierHeads;

        _baseExperience = parameters.BaseExperience;
        _experienceMultiplier = parameters.ExperienceMultiplier;
    }

    public float GetHealth()
    {
        return BaseHealth * HealthMultiplier;
    }

    public float GetExperience()
    {
        return BaseExperience * ExperienceMultiplier + (GetHealth() * HealthPercentageExp);
    }

    public int GetHeads()
    {
        return (int)(GetHealth() * HealthPercentageExp) + BaseHeads;
    }

    public string GetRandomPhrase()
    {
        return MonsterPhrases[Random.Range(0, MonsterPhrases.Count)];
    }
}
