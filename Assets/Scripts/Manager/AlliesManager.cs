using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AlliesManager : MonoBehaviour
{
    public static AlliesManager Instance;

    public List<GameObject> Allies;

    public List<Image> AlliesImages;

    [Title("Allies Damage Coroutine Data")]
    public float TotalDamage { get => _totalDamage; }

    private float _totalDamage;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        StartCoroutine(MakeAllyDamage());
    }

    public void ActivateAllyImage(int id)
    {
        AlliesImages[id].gameObject.SetActive(true);
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

    public void BuffAlly(float Damage)
    {
        _totalDamage += Damage;
    }

    IEnumerator MakeAllyDamage()
    {
        while(true)
        {
            if(_totalDamage > 0)
                PlayerManager.Instance.Monster.TakeDamage(_totalDamage * 0.1f, false);
            yield return new WaitForSeconds(0.1f);
        }
    }

}
