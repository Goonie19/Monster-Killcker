using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public Texture2D DefaultCursor;
    public Texture2D TenCursor;

    public bool StartAgain;
    public bool TenShopMode
    {
        get => _tenShopMode;
        set
        {
            _tenShopMode = value;

            if (_tenShopMode)
                Cursor.SetCursor(TenCursor, Vector2.zero, CursorMode.Auto);
            else
                Cursor.SetCursor(DefaultCursor, Vector2.zero, CursorMode.Auto);

            if(SceneManager.GetSceneByName(GameSceneName) == SceneManager.GetActiveScene())
                UIManager.Instance.CheckButtonInteraction();
        }
    }

    public string MenuSceneName;
    public string GameSceneName;

    [Title("Save Default Parameters")]
    public PlayerData DefaultPlayerData;
    public MonsterData DefaultMonsterData;
    public BossData DefaultBossData;

    public List<AllyType> DefaultAllies;

    public List<Vector2Int> Resolutions;

    private bool StatHovers = true;
    private bool AlliesHovers = true;
    private bool WindowMode = false;

    private int _resolutionIndex;

    private bool _tenShopMode;

    void Awake()
    {
        if(Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);

        WindowMode = !Screen.fullScreen;

        if(PlayerPrefs.HasKey("StatHovers"))
            StatHovers = PlayerPrefs.GetInt("StatHovers") > 0 ? true : false;

        if(PlayerPrefs.HasKey("AlliesHovers"))
            AlliesHovers = PlayerPrefs.GetInt("AlliesHovers") > 0 ? true : false;

        if (PlayerPrefs.HasKey("ScreenIndex"))
            _resolutionIndex = PlayerPrefs.GetInt("ScreenIndex");
        
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
        Screen.fullScreen = !WindowMode;

        PlayerPrefs.SetInt("WindowMode", WindowMode ? 1 : 0);
        PlayerPrefs.Save();
    }

    public void SetScreenResolution(int resIndex)
    {
        _resolutionIndex = resIndex;

        PlayerPrefs.SetInt("ScreenIndex", resIndex);

        PlayerPrefs.Save();

        Screen.SetResolution(Resolutions[_resolutionIndex].x, Resolutions[_resolutionIndex].y, Screen.fullScreen);
    }

    public int GetResolutionIndex()
    {
        return _resolutionIndex;
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
        AudioManager.Instance.StopMusic();
        AudioManager.Instance.PlayAmbientMusic();
        TenShopMode = false;
        Debug.Log("Ola");
        SceneManager.LoadScene(GameSceneName);
    }

    public void ChangeToMenuScene()
    {
        AudioManager.Instance.StopMusic();
        TenShopMode = false;
        SceneManager.LoadScene(MenuSceneName);
    }

}
