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

    private bool StatHovers = true;
    private bool AlliesHovers = true;
    private bool WindowMode = false;

    void Awake()
    {
        if(Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);

        if(PlayerPrefs.HasKey("StatHovers"))
            StatHovers = PlayerPrefs.GetInt("StatHovers") > 0 ? true : false;

        if(PlayerPrefs.HasKey("AlliesHovers"))
            AlliesHovers = PlayerPrefs.GetInt("AlliesHovers") > 0 ? true : false;
        
    }

    public void ShowStatHovers(bool show)
    {
        StatHovers = show;

        PlayerPrefs.SetInt("StatHovers", StatHovers ? 1 : 0);
        PlayerPrefs.Save();
    }

    public void ShowAlliesHovers(bool show)
    {
        AlliesHovers = show;

        PlayerPrefs.SetInt("AlliesHovers", AlliesHovers ? 1 : 0);
        PlayerPrefs.Save();
    }

    public void SetFullScreen(bool windowMode)
    {
        WindowMode = windowMode;
        Screen.fullScreen = WindowMode;

        PlayerPrefs.SetInt("WindowMode", WindowMode ? 1 : 0);
        PlayerPrefs.Save();
    }

    public bool GetStatHovers()
    {
        return StatHovers;
    }

    public bool GetAlliesHovers()
    {
        return AlliesHovers;
    }

    public bool GetWindowMode()
    {
        return WindowMode;
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
