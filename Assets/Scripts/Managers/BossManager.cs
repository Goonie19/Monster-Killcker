using DG.Tweening;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//I have to clean this code 

public class BossManager : MonoBehaviour
{
    public static BossManager Instance;

    public float BossHealth;
    public int HeadsToUnlock;

    [Title("Boss attacks")]
    public int NumberOfAlliesToKill = 10;
    public float BossFightTime = 60;
    public float NumberOfAttacks = 6;

    [Title("Boss Messages")]
    public float TimeBetweenMessages;
    public float MessageAppearTime;

    [TextArea(0,3)]
    public List<string> BossDialog;
    [TextArea(0, 3)]
    public List<string> BossDyingDialogs;
    [TextArea(0, 3)]
    public List<string> BossBackDialogs;

    [Title("Damage goals")]
    public List<LifeGoal> Goals;

    public bool InBossFight { 
        get => _inBossFight;
        set => _inBossFight = value; 
    }

    [Serializable]
    public class LifeGoal
    {
        public float DamageGoal;
        public float ExpToGive;

        public bool achieved;
    }


    private bool _bossSpeaking;
    private bool _inBossFight;
    private void Awake()
    {
        Instance = this;

    }

    void Start()
    {
        InitializeGoals();
    }

    void InitializeGoals()
    {
        BossData parameters;

        if (SaveDataManager.Instance.CanGetData())
            parameters = SaveDataManager.Instance.GetBossParameters();
        else
            parameters = new BossData(GameManager.Instance.DefaultBossData);

        foreach(LifeGoal goal in Goals)
        {
            if (goal.DamageGoal <= parameters.DamageTaken)
                goal.achieved = true;
            else
                break;
        }
            
    }

    #region BOSS SEQUENCES

    [ContextMenu("StartBoss")]
    public void StartBoss()
    {
        UIManager.Instance.BossAppearingImage.color = new Color(1, 1, 1, 0);
        UIManager.Instance.BossAppearingPanel.SetActive(true);
        _bossSpeaking = true;
        UIManager.Instance.BossAppearingImage.raycastTarget = true;

        AudioManager.Instance.StopMusic();

        Sequence sq = DOTween.Sequence();

        AllyManager.Instance.canAttack = false;

        sq.Append(UIManager.Instance.BossAppearingImage.DOFade(1, 1).SetEase(Ease.Linear));

        sq.Play();

        sq.OnComplete(() => {
            StartCoroutine(BossSpeakSequence());
            
        });
    }

    IEnumerator BossSpeakSequence()
    {
        UIManager.Instance.BossAppearButton.SetActive(false);
        int index = 0;
        UIManager.Instance.MonsterToClick.gameObject.SetActive(false);
        UIManager.Instance.BossToClick.gameObject.SetActive(true);

        UIManager.Instance.BossTimer.gameObject.SetActive(true);

        while (_bossSpeaking)
        {

            UIManager.Instance.BossSpeakingText.text = BossDialog[index];

            if (index >= BossDialog.Count - 1)
            {
                _bossSpeaking = false;
            }
            UIManager.Instance.BossSpeakingText.DOFade(1, MessageAppearTime).SetEase(Ease.Linear).Play();

            yield return new WaitForSeconds(TimeBetweenMessages);

            UIManager.Instance.BossSpeakingText.DOFade(0, MessageAppearTime).SetEase(Ease.Linear).Play();

            yield return new WaitForSeconds(MessageAppearTime);
            ++index;
        }

        Sequence sq = DOTween.Sequence();

        sq.Append(UIManager.Instance.BossAppearingImage.DOFade(0, 1).SetEase(Ease.Linear));

        sq.Play();

        sq.OnComplete(() => {
            UIManager.Instance.BossAppearingImage.raycastTarget = false;
            InBossFight = true;
            UIManager.Instance.ChangeInfoPanel();
            UIManager.Instance.UpdateInfoPanels();
            AllyManager.Instance.canAttack = true;
            UIManager.Instance.BossToClick.StartBattle();
        });

    }

    public void ComeBackToMonsterBattle()
    {
        InBossFight = false;
        //In Process
        UIManager.Instance.BossAppearingImage.color = new Color(1, 1, 1, 0);
        UIManager.Instance.BossAppearingPanel.SetActive(true);
        _bossSpeaking = true;

        AudioManager.Instance.StopMusic();
        UIManager.Instance.BossAppearingImage.raycastTarget = true;

        Sequence sq = DOTween.Sequence();

        AllyManager.Instance.canAttack = false;

        sq.Append(UIManager.Instance.BossAppearingImage.DOFade(1, 1).SetEase(Ease.Linear));

        sq.Play();

        sq.OnComplete(() => {
            StartCoroutine(ComeBackSpeakSequence());
        });
    }

    IEnumerator ComeBackSpeakSequence()
    {
        int index = 0;
        UIManager.Instance.MonsterToClick.gameObject.SetActive(true);
        UIManager.Instance.BossToClick.gameObject.SetActive(false);
        UIManager.Instance.BossAppearButton.SetActive(false);
        while (_bossSpeaking)
        {

            UIManager.Instance.BossSpeakingText.text = BossBackDialogs[index];

            if (index >= BossBackDialogs.Count - 1)
            {
                _bossSpeaking = false;
            }
            UIManager.Instance.BossSpeakingText.DOFade(1, MessageAppearTime).SetEase(Ease.Linear).Play();

            yield return new WaitForSeconds(TimeBetweenMessages);

            UIManager.Instance.BossSpeakingText.DOFade(0, MessageAppearTime).SetEase(Ease.Linear).Play();

            yield return new WaitForSeconds(MessageAppearTime);
            ++index;
        }

        Sequence sq = DOTween.Sequence();

        sq.Append(UIManager.Instance.BossAppearingImage.DOFade(0, 1).SetEase(Ease.Linear));

        sq.Play();

        sq.OnComplete(() => {
            AudioManager.Instance.PlayAmbientMusic();
            UIManager.Instance.BossAppearingImage.raycastTarget = false;
            UIManager.Instance.BossTimer.gameObject.SetActive(false);
            UIManager.Instance.ChangeInfoPanel();
            AllyManager.Instance.canAttack = true;
        });
    }
    [ContextMenu("EndBoss")]
    public void EndBoss()
    {
        AudioManager.Instance.PlayEndingWind();
        UIManager.Instance.BossAppearingImage.color = new Color(1, 1, 1, 0);
        UIManager.Instance.BossAppearingPanel.SetActive(true);
        _bossSpeaking = true;
        UIManager.Instance.BossAppearingImage.raycastTarget = true;

        Sequence sq = DOTween.Sequence();

        sq.Append(UIManager.Instance.BossAppearingImage.DOFade(1, 1).SetEase(Ease.Linear));

        sq.Play();

        sq.OnComplete(() => {
            InBossFight = false;
            UIManager.Instance.ChangeInfoPanel();
            UIManager.Instance.UpdateInfoPanels();
            StartCoroutine(EndingBossSpeakSequence());
            AllyManager.Instance.canAttack = false;
            UIManager.Instance.BossTimer.gameObject.SetActive(false);
        });
    }

    IEnumerator EndingBossSpeakSequence()
    {
        int index = 0;
        UIManager.Instance.MonsterToClick.gameObject.SetActive(false);
        UIManager.Instance.BossToClick.gameObject.SetActive(true);
        while (_bossSpeaking)
        {

            UIManager.Instance.BossSpeakingText.text = BossDyingDialogs[index];

            if (index >= BossDyingDialogs.Count - 1)
            {
                _bossSpeaking = false;
            }
            UIManager.Instance.BossSpeakingText.DOFade(1, MessageAppearTime).SetEase(Ease.Linear).Play();

            yield return new WaitForSeconds(TimeBetweenMessages);

            UIManager.Instance.BossSpeakingText.DOFade(0, MessageAppearTime).SetEase(Ease.Linear).Play();

            yield return new WaitForSeconds(MessageAppearTime);
            ++index;
        }

        UIManager.Instance.ActivateCreditsScreen();

        Sequence sq = DOTween.Sequence();

        sq.Append(UIManager.Instance.BossAppearingImage.DOFade(0, 1).SetEase(Ease.Linear));

        sq.Play();

        sq.OnComplete(() => {
            UIManager.Instance.CreditsSequence();
        });

    }

    #endregion

    public int GetGoalIndexCompleted(float DamageTaken)
    {
        int i = Goals.FindIndex((x) => !x.achieved && DamageTaken >= x.DamageGoal);

        if (i >= 0)
            return i;
        else
            return -1;
    }

    public LifeGoal GetGoalCompleted()
    {
        return Goals.Find((x) => !x.achieved);
    }

    public float GetBossExpByGoal(int index)
    {
        return Goals[index].ExpToGive;
    }

    public float GetMaxHealth()
    {
        return BossHealth;
    }

    
}
