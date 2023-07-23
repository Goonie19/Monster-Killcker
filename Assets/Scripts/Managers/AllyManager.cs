using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllyManager : MonoBehaviour
{

    public static AllyManager Instance;

    public List<AllyType> allies;

    public bool canAttack;

    private Coroutine _alliesCoroutine;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        UIManager.Instance.OnFadeOut.AddListener(InitializeParameters);
        UIManager.Instance.OnFadeOut.AddListener(CheckAllies);
        UIManager.Instance.OnFadeOut.AddListener(() => _alliesCoroutine = StartCoroutine(AlliesBehaviour()));
        
    }

    void InitializeParameters()
    {

        for (int i = 0; i < GameManager.Instance.DefaultAllies.Count; ++i)
        {
            allies.Add(new AllyType(GameManager.Instance.DefaultAllies[i]));
        }

        if (SaveDataManager.Instance.CanGetData())
            InitializeAllies(SaveDataManager.Instance.GetAllies());
    }

    void InitializeAllies(List<AlliesData> AlliesData)
    {
        

        foreach (AlliesData data in AlliesData)
        {
            
            int index;
            index = allies.FindIndex((x) => x.AllyId == data.Id);
            
            allies[index].SetSilentNumberOfAllies(data.NumberOfAllies);
            allies[index].BaseDamage = data.BaseDamage;
            allies[index].Price = data.Price;
            allies[index].PriceMultiplier = data.PriceMultiplier;
            allies[index].SetSilentUnlocked(false);
            
        }
    }

    public void UpdateAllies(int id)
    {
        SaveDataManager.Instance.UnlockAllies(id);
    }

    public void CheckAllies()
    {
        foreach(AllyType ally in allies)
        {
            if (ally.HeadsToUnlock <= PlayerManager.Instance.TotalHeads)
                ally.Unlock();
        }
    }

    IEnumerator AlliesBehaviour()
    {
        float damage = 0;

        while(PlayerManager.Instance.InBattle)
        {
            if(canAttack)
            {
                foreach (AllyType ally in allies)
                    damage += ally.GetDamage();

                UIManager.Instance.AlliesDamageText.text = string.Format("{0:0.00}", damage);

                if (!BossManager.Instance.InBossFight)
                {
                    if (damage > 0)
                        UIManager.Instance.MonsterToClick.TakeDamage(damage * 0.1f);
                    
                }
                else
                {
                    if (damage > 0)
                        UIManager.Instance.BossToClick.TakeDamage(damage * 0.1f);
                }
            }

            damage = 0;
            yield return new WaitForSeconds(0.1f);
        }
    }

    public int GetTotalAllies()
    {
        int numberOfAllies = 0;

        foreach(AllyType ally in allies)
            numberOfAllies += ally.NumberOfAllies;
        
        return numberOfAllies;
    }
}
