using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterManager : MonoBehaviour
{

    public static MonsterManager Instance;

    [Title("Monster Health Related Atributes")]
    public float BaseHealth = 5;
    public float HealthMultiplier = 1;
    public float HealthPercentageExp;

    [Title("Monster Possible Sprites")]
    public List<Sprite> MonsterSprites;
    [TextArea(0,3)]
    public List<string> MonsterPhrases;

    [Title("Monster Heads Related Atributes")]
    public int BaseHeads = 1;
    public float MultiplierHeads;

    [Title("Monster Experience Related Atributes")]
    public float BaseExperience = 10;
    public float ExperienceMultiplier = 1;

    private void Awake()
    {
        Instance = this;
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
        return BaseHeads;
    }

    public string GetRandomPhrase()
    {
        return MonsterPhrases[Random.Range(0, MonsterPhrases.Count)];
    }
}
