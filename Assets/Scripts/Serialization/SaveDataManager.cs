using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveDataManager : MonoBehaviour
{

    public static SaveDataManager Instance;

    [Serializable]
    public class Data
    {
        public class PlayerData
        {
            public float ActualExperience;
            public float ActualHeads;
            public float TotalHeads;
        }

        public class MonsterData
        {
            public float BaseHealth;
            public float HealthMultiplier;
            public float HealthPercentageMultiplier;

            public int BaseHeads;
            public float MultiplierHeads;

            public float BaseExperience;
            public float ExperienceMultiplier;
        }

        public class BossData
        {
            public float DamageTaken;
            public float actualHealth;
        }

        public PlayerData PlayerParameters;
        public MonsterData MonsterParameters;
        public BossData BossParameters;

        public List<AllyType> allies;
        public List<Buff> buffs;
    }

    void Awake()
    {
        Instance = this;
    }


}
