using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class SaveDataManager : MonoBehaviour
{

    public static SaveDataManager Instance;

    public string FileName = "SaveData";

    private Data _saveData;

    [Serializable]
    public class Data
    {
        
        public PlayerData PlayerParameters;
        public MonsterData MonsterParameters;
        public BossData BossParameters;

        public List<AllyType> allies;
        public List<Buff> buffs;
    }

    void Awake()
    {
        Instance = this;

        if(File.Exists(Application.persistentDataPath + "/" + FileName + ".dat"))
            DeserializeBinary();
        else
            _saveData = new Data();
    }

    public void SaveData()
    {
        Serialize();
    }

    public bool CanGetData()
    {
        return File.Exists(Application.persistentDataPath + "/" + FileName + ".dat");
    }

    #region SERIALIZATION METHODS

    public void Serialize()
    {
        FileStream fs = new FileStream(Application.persistentDataPath + "/" + FileName + ".dat", FileMode.Create);

        BinaryFormatter formater = new BinaryFormatter();
        formater.Serialize(fs, _saveData);

        fs.Close();
    }

    void DeserializeBinary()
    {
        FileStream fs = new FileStream(Application.persistentDataPath + "/" + FileName + ".dat", FileMode.Open);

        BinaryFormatter formater = new BinaryFormatter();

        _saveData = (Data)formater.Deserialize(fs);

        fs.Close();

    }

    #endregion

    #region SET METHODS

    public void SetPlayerActualExperience(float actualExperience)
    {
        _saveData.PlayerParameters.ActualExperience = actualExperience;
    }

    public void SetPlayerActualHeads(int actualHeads)
    {
        _saveData.PlayerParameters.ActualHeads = actualHeads;
    }

    public void SetPlayerTotalHeads(int totalHeads)
    {
        _saveData.PlayerParameters.TotalHeads = totalHeads;
    }

    public void SetMonsterBaseHealth(float baseHealth)
    {
        _saveData.MonsterParameters.BaseHealth = baseHealth;
    }

    public void SetMonsterHealthMultiplier(float healthMultiplier)
    {
        _saveData.MonsterParameters.HealthMultiplier = healthMultiplier;
    }

    public void SetMonsterHealthPercentageMultiplier(float healthPercentageMultiplier)
    {
        _saveData.MonsterParameters.HealthPercentageMultiplier = healthPercentageMultiplier;
    }

    public void SetMonsterBaseHeads(int baseHeads)
    {
        _saveData.MonsterParameters.BaseHeads = baseHeads;
    }

    public void SetMonsterMultiplierHeads(float multiplierHeads)
    {
        _saveData.MonsterParameters.MultiplierHeads = multiplierHeads;
    }

    public void SetMonsterBaseExperience(float baseExperience)
    {
        _saveData.MonsterParameters.BaseExperience = baseExperience;
    }

    public void SetMonsterExperienceMultiplier(float experienceMultiplier)
    {
        _saveData.MonsterParameters.ExperienceMultiplier = experienceMultiplier;
    }

    public void SetBossDamageTaken(float damageTaken)
    {
        _saveData.BossParameters.DamageTaken = damageTaken;
    }

    public void SetBossActualHealth(float actualHealth)
    {
        _saveData.BossParameters.actualHealth = actualHealth;
    }

    #endregion

    #region GET METHODS

    public PlayerData GetPlayerParameters()
    {
        return _saveData.PlayerParameters;
    }

    public MonsterData GetMonsterParameters()
    {
        return _saveData.MonsterParameters;
    }

    public BossData GetBossParameters()
    {
        return _saveData.BossParameters;
    }

    #endregion

}
