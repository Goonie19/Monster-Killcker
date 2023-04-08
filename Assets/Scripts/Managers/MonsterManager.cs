using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

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

    [Title("Event for parameters initialized")]
    public UnityEvent OnParametersInitialized;

    private float _baseHealth;
    private float _healthPercentageExp;

    private int _baseHeads;

    private float _baseExperience;



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
            parameters = new MonsterData(GameManager.Instance.DefaultMonsterData);

        _baseHealth = parameters.BaseHealth;
        _healthPercentageExp = parameters.HealthPercentageMultiplier;

        _baseHeads = parameters.BaseHeads;

        _baseExperience = parameters.BaseExperience;

        OnParametersInitialized?.Invoke();
        UIManager.Instance.UpdateHealthBar(GetHealth());
    }

    public float GetHealth()
    {
        return BaseHealth;
    }

    public float GetExperience()
    {
        return BaseExperience + (GetHealth() * HealthPercentageExp);
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
