﻿using DG.Tweening;
using Sirenix.OdinInspector;
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

    public bool InBossFight { 
        get => _inBossFight;
        set => _inBossFight = value; 
    }



    private bool _bossSpeaking;
    private bool _inBossFight;
    private void Awake()
    {
        Instance = this;
    }

    [ContextMenu("StartBoss")]
    public void StartBoss()
    {
        UIManager.Instance.BossAppearingImage.color = new Color(1, 1, 1, 0);
        UIManager.Instance.BossAppearingPanel.SetActive(true);
        _bossSpeaking = true;
        UIManager.Instance.BossAppearingImage.raycastTarget = true;

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
            AllyManager.Instance.canAttack = true;
            UIManager.Instance.BossToClick.StartBattle();
        });

    }

    public void ComeBackToMonsterBattle()
    {
        //In Process
        UIManager.Instance.BossAppearingImage.color = new Color(1, 1, 1, 0);
        UIManager.Instance.BossAppearingPanel.SetActive(true);
        _bossSpeaking = true;
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
            UIManager.Instance.BossAppearingImage.raycastTarget = false;
            InBossFight = false;
            AllyManager.Instance.canAttack = true;
        });
    }

    public void EndBoss()
    {
        UIManager.Instance.BossAppearingImage.color = new Color(1, 1, 1, 0);
        UIManager.Instance.BossAppearingPanel.SetActive(true);
        _bossSpeaking = true;
        UIManager.Instance.BossAppearingImage.raycastTarget = true;

        Sequence sq = DOTween.Sequence();

        sq.Append(UIManager.Instance.BossAppearingImage.DOFade(1, 1).SetEase(Ease.Linear));

        sq.Play();

        sq.OnComplete(() => {
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

        Sequence sq = DOTween.Sequence();

        sq.Append(UIManager.Instance.BossAppearingImage.DOFade(0, 1).SetEase(Ease.Linear));

        sq.Play();

        sq.OnComplete(() => {
            UIManager.Instance.BossAppearingImage.raycastTarget = false;
        });

    }
}
