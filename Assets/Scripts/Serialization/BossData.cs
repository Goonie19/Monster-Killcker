using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class BossData {
    public float DamageTaken;
    public float actualHealth;

    public BossData()
    {
        DamageTaken = Placeholder.BossDamageTaken;
        actualHealth = Placeholder.BossActualHealth;
    }

    public BossData(BossData data)
    {
        DamageTaken = data.DamageTaken;
        actualHealth = data.actualHealth;
    }
}
