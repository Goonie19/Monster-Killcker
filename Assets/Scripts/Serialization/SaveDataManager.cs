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

        public List<AlliesData> allies;
        public List<BuffData> buffs;

        
    }

    void Awake()
    {
        Instance = this;

        if (File.Exists(Application.persistentDataPath + "/" + FileName + ".dat") &&  !GameManager.Instance.StartAgain )
            DeserializeBinary();
        else
        {
            _saveData = new Data();
            _saveData.PlayerParameters = GameManager.Instance.DefaultPlayerData;
            _saveData.MonsterParameters = GameManager.Instance.DefaultMonsterData;
            _saveData.BossParameters = GameManager.Instance.DefaultBossData;

            _saveData.allies = new List<AlliesData>();
            _saveData.buffs = new List<BuffData>();
        }
    }

    public void SaveData()
    {
        Serialize();
    }

    public bool CanGetData()
    {
        return File.Exists(Application.persistentDataPath + "/" + FileName + ".dat") && !GameManager.Instance.StartAgain;
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

    public void UnlockAllies(int AllyId)
    {
        int index = -1;

        if (_saveData.allies.Count > 0)
            index = _saveData.allies.FindIndex(x => x.Id == AllyId);


        if (index == -1)
        {
            _saveData.allies.Add(new AlliesData(AllyId, 0, AllyManager.Instance.allies[AllyId].BaseDamage, AllyManager.Instance.allies[AllyId].DamageMultiplier,
                AllyManager.Instance.allies[AllyId].Price, AllyManager.Instance.allies[AllyId].PriceMultiplier));
        } else
        {
            _saveData.allies[index].NumberOfAllies = AllyManager.Instance.allies[AllyId].NumberOfAllies;
            _saveData.allies[index].Price = AllyManager.Instance.allies[AllyId].Price;
            _saveData.allies[index].PriceMultiplier = AllyManager.Instance.allies[AllyId].PriceMultiplier;
            _saveData.allies[index].BaseDamage = AllyManager.Instance.allies[AllyId].BaseDamage;
            _saveData.allies[index].DamageMultiplier = AllyManager.Instance.allies[AllyId].DamageMultiplier;
        }

    }

    public void SetBuff(Buff b)
    {
        int index = -1;
        if (_saveData.buffs.Count > 0)
            index = _saveData.buffs.FindIndex(x => x.BuffId == b.Id);

        if (index == -1)
        {
            _saveData.buffs.Add(new BuffData(b.Id, 1));
        } else
        {
            _saveData.buffs[index].NumberOfBuffs = b.NumberOfBuffs;
            _saveData.buffs[index].ActualPrice = b.GetPrice();
        }
    }

    public void SetPlayerBaseDamage(float baseDamage)
    {
        _saveData.PlayerParameters.BaseDamage = baseDamage;
    }

    public void SetPlayerDamageMultiplier(float damageMultiplier)
    {
        _saveData.PlayerParameters.DamageMultiplier = damageMultiplier;
    }

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

    public List<AlliesData> GetAllies()
    {
        return _saveData.allies;
    }

    public List<BuffData> GetBuffsAcquired()
    {
        return _saveData.buffs;
    }

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
