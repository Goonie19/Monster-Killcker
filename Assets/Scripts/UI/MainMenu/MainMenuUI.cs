using DG.Tweening;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour
{

    public Image FadePanelImage;
    public float FadeTime = 2f;

    [Title("Main Menu Buttons")]
    public Button ContinueButton;
    public Button NewGameButton;
    public Button OptionsButton;
    public Button ExitButton;

    [Title("Credits")]
    public Image MonsterCredit;
    public TextMeshProUGUI CreditHeaderText;
    public TextMeshProUGUI CreditText;

    public List<Credit> CreditList;

    [Title("Volume Sliders")]
    public Slider MasterVolumeSlider;
    public Slider MusicVolumeSlider;
    public Slider SFXVolumeSlider;

    [Title("Settings Elements")]
    public Toggle StatsHoverToggle;
    public GameObject StatsHoverCheckmark;
    public Toggle AlliesHoverToggle;
    public GameObject AlliesHoverCheckmark;
    public Toggle WindowModeToggle;
    public GameObject WindowModeCheckmark;
    public Toggle DialoguesToggle;
    public GameObject DialoguesCheckmark;

    public TMP_Dropdown ResolutionDropdown;

    public Button ExitSettingsButton;

    [Serializable]
    public class Credit
    {
        public string CreditHeader;
        public string CreditName;
        public Sprite CreditSprite;
    }

    private int _creditIndex;

    private Sequence _nonSelectedTween;

    void Awake()
    {
        FadeOutPanel();

        _creditIndex = 0;

        NonSelectedAnimation();
    }

    void Start()
    {
        Setup();
        AudioManager.Instance.PlayMainMenuMusic();
        
    }

    void SetSettingsBehaviour()
    {
        bool stats = GameManager.Instance.GetStatHovers();
        bool allies = GameManager.Instance.GetAlliesHovers();
        bool window = GameManager.Instance.GetWindowMode();
        bool dialogues = GameManager.Instance.GetDialogues();

        StatsHoverToggle.isOn = stats;
        StatsHoverCheckmark.SetActive(stats);

        AlliesHoverToggle.isOn = allies;
        AlliesHoverCheckmark.SetActive(allies);

        WindowModeToggle.isOn = window;
        WindowModeCheckmark.SetActive(window);

        DialoguesToggle.isOn = dialogues;
        DialoguesCheckmark.SetActive(dialogues);

        ResolutionDropdown.ClearOptions();

        List<string> resOptions = new List<string>();
        foreach (Vector2Int res in GameManager.Instance.Resolutions)
        {
            resOptions.Add((res.x + " x " + res.y).ToString());
        }

        ResolutionDropdown.AddOptions(resOptions);
        ResolutionDropdown.value = GameManager.Instance.GetResolutionIndex();

        WindowModeToggle.onValueChanged.AddListener(GameManager.Instance.SetFullScreen);
        StatsHoverToggle.onValueChanged.AddListener(GameManager.Instance.ShowStatHovers);
        AlliesHoverToggle.onValueChanged.AddListener(GameManager.Instance.ShowAlliesHovers);
        DialoguesToggle.onValueChanged.AddListener(GameManager.Instance.ShowMonsterDialogues);
        ResolutionDropdown.onValueChanged.AddListener(GameManager.Instance.SetScreenResolution);

        MasterVolumeSlider.value = AudioManager.Instance.GetMasterVolume();
        MusicVolumeSlider.value = AudioManager.Instance.GetMusicVolume();
        SFXVolumeSlider.value = AudioManager.Instance.GetSFXVolume();

        MasterVolumeSlider.onValueChanged.AddListener(AudioManager.Instance.SetMasterVolume);
        MusicVolumeSlider.onValueChanged.AddListener(AudioManager.Instance.SetMusicVolume);
        SFXVolumeSlider.onValueChanged.AddListener(AudioManager.Instance.SetSFXVolume);


    }

    void FadeOutPanel()
    {
        Sequence sq = DOTween.Sequence();

        sq.Append(FadePanelImage.DOFade(0f, FadeTime).SetEase(Ease.Linear));

        sq.Play();

        sq.OnComplete(() => FadePanelImage.raycastTarget = false);
    }

    void GoToGameScene()
    {
        FadePanelImage.raycastTarget = true;
        Sequence sq = DOTween.Sequence();

        sq.Append(FadePanelImage.DOFade(1f, FadeTime).SetEase(Ease.Linear));

        sq.Play();

        sq.OnComplete(() => GameManager.Instance.ChangeToGameScene());
    }

    public void Setup()
    {
        ContinueButton.onClick.AddListener(() => { 
            GameManager.Instance.StartAgain = false;
            AudioManager.Instance.PlayClickPlayButtonSound();
            GoToGameScene();
        });
        NewGameButton.onClick.AddListener(() => { 
            GameManager.Instance.StartAgain = true;
            AudioManager.Instance.PlayClickPlayButtonSound();
            GoToGameScene();
        });
        OptionsButton.onClick.AddListener(() => { AudioManager.Instance.PlayClickButtonSound(); });
        ExitSettingsButton.onClick.AddListener(() => { AudioManager.Instance.PlayClickButtonSound(); });
        ExitButton.onClick.AddListener(() => { AudioManager.Instance.PlayClickButtonSound(); Application.Quit(); });

        CreditHeaderText.text = CreditList[_creditIndex].CreditHeader;
        CreditText.text = CreditList[_creditIndex].CreditName;
        MonsterCredit.sprite = CreditList[_creditIndex].CreditSprite;

        SetSettingsBehaviour();
    }

    public void ClickOnCredit()
    {
        Sequence sq = DOTween.Sequence();

        sq.Append(MonsterCredit.transform.DOScale(0f, 0.1f).SetEase(Ease.Linear));

        sq.Play();

        sq.OnComplete(() => {

            _creditIndex++;

            if (_creditIndex >= CreditList.Count)
                _creditIndex = 0;

            CreditHeaderText.text = CreditList[_creditIndex].CreditHeader;
            CreditText.text = CreditList[_creditIndex].CreditName;
            MonsterCredit.sprite = CreditList[_creditIndex].CreditSprite;

            MonsterCredit.transform.DOScale(1, 0.1f).SetEase(Ease.Linear).Play(); 
        
        });
       
        
        AudioManager.Instance.PlayHitSound();
    }

    public void NonSelectedAnimation()
    {
        _nonSelectedTween = DOTween.Sequence();

        _nonSelectedTween.Append(MonsterCredit.transform.DOScale(1f, 1f).SetEase(Ease.Linear));
        _nonSelectedTween.Append(MonsterCredit.transform.DOScale(0.95f, 1f).SetEase(Ease.Linear));

        _nonSelectedTween.SetLoops(-1);

        _nonSelectedTween.Play();
    }

    public void StopNonSlectedAnimation()
    {
        _nonSelectedTween.Kill();

        MonsterCredit.transform.DOScale(0.95f, 0.1f).SetEase(Ease.Linear).Play();

    }
}
