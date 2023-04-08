using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class MonsterData {
    public float BaseHealth;
    public float HealthMultiplier;
    public float HealthPercentageMultiplier;

    public int BaseHeads;
    public float MultiplierHeads;

    public float BaseExperience;
    public float ExperienceMultiplier;

    public MonsterData(MonsterData data) { 
        BaseHealth = data.BaseHealth;
        HealthMultiplier = data.HealthMultiplier;
        HealthPercentageMultiplier = data.HealthPercentageMultiplier;
        BaseHeads = data.BaseHeads;
        MultiplierHeads = data.MultiplierHeads;
        ExperienceMultiplier = data.ExperienceMultiplier;
        BaseExperience = data.BaseExperience;
    }
}
