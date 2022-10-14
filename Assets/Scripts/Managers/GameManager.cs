using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public bool StartAgain;

    public string MenuSceneName;
    public string GameSceneName;

    [Title("Save Default Parameters")]
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


    public void ChangeToGameScene()
    {
        AudioManager.Instance.PlayAmbientMusic();
        SceneManager.LoadScene(GameSceneName);
    }

    public void ChangeToMenuScene()
    {
        SceneManager.LoadScene(MenuSceneName);
    }

}
