using System;

[Serializable]
public class PlayerData {
    public float ActualExperience;
    public int ActualHeads;
    public int TotalHeads;

    public float BaseDamage;
    public float DamageMultiplier;

    public PlayerData(PlayerData data)
    {
        ActualExperience = data.ActualExperience;
        ActualHeads = data.ActualHeads;
        TotalHeads = data.TotalHeads;
        BaseDamage = data.BaseDamage;
        DamageMultiplier = data.DamageMultiplier;
    }
}
