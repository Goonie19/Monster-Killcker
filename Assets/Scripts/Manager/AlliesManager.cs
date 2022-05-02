using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AlliesManager : MonoBehaviour
{
    public static AlliesManager Instance;

    public List<AllyInfo> ActiveAllies;

    public List<GameObject> Allies;

    public List<Image> AlliesImages;

    public List<AllyPowerUp> AlliesPowerUps;

    [Title("Allies Damage Coroutine Data")]

    public float TotalDamage { get => _totalDamage; }

    private float _totalDamage;

    [Serializable]
    public class AllyInfo {
        
        public int Amount;
        public float DamageMultiplier;
        public float BaseDamage;

        public float ExperienceRequired;

        public AllyInfo(int id, int amount, float damageMultiplier, float baseDamage)
        {
            Amount = amount;
            DamageMultiplier = damageMultiplier;
            BaseDamage = baseDamage;
        }


    }

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);

    }

    private void OnEnable()
    {
        PlayerManager.Instance.OnHeadsAdded += CheckPowerUp;
    }

    private void OnDisable()
    {
        PlayerManager.Instance.OnHeadsAdded -= CheckPowerUp;
    }

    private void Start()
    {
        StartCoroutine(MakeAllyDamage());
    }

    public void ActivateAllyImage(int id)
    {
        AlliesImages[id].gameObject.SetActive(true);
        AlliesImages[id].GetComponent<Image>().SetNativeSize();

    }

    public void UnlockAllies(List<int> ally)
    {
        foreach(int a in ally)
        {
            if (a >= 0 && a < Allies.Count)
            {
                Allies[a].SetActive(true);
                
            }
        }
    }

    public void BuffAlly(int id, int amount, float Damage, float multiplier, float PriceMultiplier)
    {
        ActiveAllies[id].Amount += amount;
        ActiveAllies[id].BaseDamage += Damage;
        ActiveAllies[id].DamageMultiplier += multiplier;
        ActiveAllies[id].ExperienceRequired *= PriceMultiplier;
    }

    IEnumerator MakeAllyDamage()
    {
        while(true)
        {
            foreach(AllyInfo info in ActiveAllies)
            {
                PlayerManager.Instance.Monster.TakeDamage(info.Amount * info.BaseDamage * info.DamageMultiplier * 0.1f, false);
            }
            yield return new WaitForSeconds(0.1f);
        }
    }

    void CheckPowerUp()
    {
        AllyPowerUp p = AlliesPowerUps.Find(x => x.HeadsRequiredToUnlock <= PlayerManager.Instance.MonsterHeads && x.gameObject.activeInHierarchy == false && !x.used);
        p?.gameObject.SetActive(true);
    }

}
