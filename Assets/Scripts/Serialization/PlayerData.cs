using System;

[Serializable]
public class PlayerData {
    public float ActualExperience;
    public int ActualHeads;
    public int TotalHeads;
    public float TotalDamage;
    public float TotalExperience;
    public float GameTime;

    public float BaseDamage;
    public float DamageMultiplier;

    public PlayerData()
    {
        ActualExperience = Placeholder.PlayerActualExperience;
        ActualHeads = Placeholder.PlayerActualHeads;
        TotalHeads = Placeholder.PlayerTotalHeads;
        TotalDamage = Placeholder.PlayerTotalDamage;
        TotalExperience = Placeholder.PlayerTotalExperience;
        GameTime = Placeholder.PlayerGameTime;
        BaseDamage = Placeholder.PlayerBaseDamage;
        DamageMultiplier = Placeholder.PlayerDamageMultiplier;
    }

    public PlayerData(PlayerData data)
    {
        ActualExperience = data.ActualExperience;
        ActualHeads = data.ActualHeads;
        TotalHeads = data.TotalHeads;
        BaseDamage = data.BaseDamage;
        DamageMultiplier = data.DamageMultiplier;
        TotalExperience = data.TotalExperience;
        TotalDamage = data.TotalDamage;
        GameTime = data.GameTime;
    }
}
