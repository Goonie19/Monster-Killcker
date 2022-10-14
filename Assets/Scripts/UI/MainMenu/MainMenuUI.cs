using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour
{

    public Button ContinueButton;
    public Button NewGameButton;
    public Button OptionsButton;
    public Button ExitButton;

    void Awake()
    {
        Setup();
    }

    public void Setup()
    {
        ContinueButton.onClick.AddListener(() => { 
            GameManager.Instance.StartAgain = false;
            GameManager.Instance.ChangeToGameScene();
        });
        NewGameButton.onClick.AddListener(() => { 
            GameManager.Instance.StartAgain = true;
            GameManager.Instance.ChangeToGameScene();
        });
        OptionsButton.onClick.AddListener(() => {  });
        ExitButton.onClick.AddListener(() => { Application.Quit(); });
    }
}
