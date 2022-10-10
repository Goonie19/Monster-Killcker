using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public bool StartAgain;

    public PlayerData DefaultPlayerData;
    public MonsterData DefaultMonsterData;
    public BossData DefaultBossData;

    public List<AllyType> DefaultAllies;

    void Awake()
    {
        if(Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);

        
    }


}
