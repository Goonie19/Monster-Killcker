using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllyManager : MonoBehaviour
{

    public static AllyManager Instance;

    public List<AllyType> allies;

    private Coroutine _alliesCoroutine;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        _alliesCoroutine = StartCoroutine(AlliesBehaviour());
    }

    public void CheckAllies()
    {
        foreach(AllyType ally in allies)
        {
            if (ally.HeadsToUnlock <= PlayerManager.Instance.ActualHeads)
                ally.Unlock();
        }
    }

    IEnumerator AlliesBehaviour()
    {
        float damage = 0;

        while(PlayerManager.Instance.InBattle)
        {
            foreach (AllyType ally in allies)
                damage += ally.BaseDamage * ally.DamageMultiplier * ally.NumberOfAllies;

            if(damage > 0)
                UIManager.Instance.MonsterToClick.TakeDamage(damage * 0.1f);

            damage = 0;
            yield return new WaitForSeconds(0.1f);
        }
    }
}
