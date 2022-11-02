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

        Setup();
        _creditIndex = 0;

        NonSelectedAnimation();
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
        ExitButton.onClick.AddListener(() => { AudioManager.Instance.PlayClickButtonSound(); Application.Quit(); });

        CreditHeaderText.text = CreditList[_creditIndex].CreditHeader;
        CreditText.text = CreditList[_creditIndex].CreditName;
        MonsterCredit.sprite = CreditList[_creditIndex].CreditSprite;

        MasterVolumeSlider.onValueChanged.AddListener(AudioManager.Instance.SetMasterVolume);
        MusicVolumeSlider.onValueChanged.AddListener(AudioManager.Instance.SetMusicVolume);
        SFXVolumeSlider.onValueChanged.AddListener(AudioManager.Instance.SetSFXVolume);
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
