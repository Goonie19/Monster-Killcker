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

    [Title("Monster Possible Sprites")]
    public List<Sprite> MonsterSprites;

    [Title("Monster Heads Related Atributes")]
    public int BaseHeads = 1;

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

}
