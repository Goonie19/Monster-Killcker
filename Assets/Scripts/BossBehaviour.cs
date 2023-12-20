using DG.Tweening;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossBehaviour : MonoBehaviour
{
    public GameObject AppearPanel;

    public Image BossImage;

    private float _actualHealth;
    private float _damageTaken;
    private bool _dead;


    private float _timer;

    private void Awake()
    {
        _timer = BossManager.Instance.BossFightTime;
        
        

    }

    void Start()
    {
        InitializeParameters();
    }

    void InitializeParameters()
    {
        BossData parameters;

        if (SaveDataManager.Instance.CanGetData())
            parameters = SaveDataManager.Instance.GetBossParameters();
        else
            parameters = new BossData();

        _actualHealth = parameters.actualHealth;

        AudioManager.Instance.SetBossLifeParameter(_actualHealth/BossManager.Instance.GetMaxHealth());

        _damageTaken = parameters.DamageTaken;

    }

    public void ClickMonster()
    {

        if (!_dead)
        {
            Sequence sq = DOTween.Sequence();

            sq.Append(transform.DOScale(0.8f, 0.1f).SetEase(Ease.Linear));
            sq.Append(transform.DOScale(1, 0.1f).SetEase(Ease.Linear));

            sq.Play();

            AudioManager.Instance.PlayHitSound();

            TakeDamage(PlayerManager.Instance.BaseDamage);

        }
    }

    public void TakeDamage(float damage)
    {
        if (!_dead)
        {

            _actualHealth -= damage;
            AudioManager.Instance.SetBossLifeParameter(_actualHealth / BossManager.Instance.GetMaxHealth());

            Debug.Log("Boss life per is: " + _actualHealth / BossManager.Instance.GetMaxHealth());
            _damageTaken += damage;
            PlayerManager.Instance.TotalDamage += damage;

            int i = BossManager.Instance.GetGoalIndexCompleted(_damageTaken);

            if (i >= 0)
                GetRewards(i);

            if (_actualHealth > 0)
            {
                UIManager.Instance.UpdateInfoPanels();
                UIManager.Instance.UpdateHealthBar(_actualHealth);
            }
            else
                Die();


        }
    }

    public void StartBattle()
    {
        StartCoroutine(BossAttackBehaviour());
    }

    [ContextMenu("Die")]
    private void Die()
    {
        _dead = true;

        _actualHealth = 0;
        UIManager.Instance.UpdateHealthBar(_actualHealth);

        transform.DOShakePosition(5f).SetEase(Ease.Linear).Play();

        BossManager.Instance.EndBoss();
    }

    public void Spawn()
    {
        BossImage.gameObject.SetActive(true);
    }

    IEnumerator BossAttackBehaviour()
    {
        _timer = 0;

        AudioManager.Instance.PlayBossMusic();

        float timeOfNextAttack = BossManager.Instance.BossFightTime / BossManager.Instance.NumberOfAttacks;

        int minutes = 0;
        int seconds = 0;

        while (_timer < BossManager.Instance.BossFightTime)
        {
            _timer += Time.deltaTime;

            if(_timer < BossManager.Instance.BossFightTime)
            {
                
                minutes = Mathf.FloorToInt(_timer / 60);
                seconds = Mathf.FloorToInt(_timer % 60);

                UIManager.Instance.BossTimer.text = String.Format("{0:00}:{1:00}", minutes, seconds);
            }

            if(_timer >= timeOfNextAttack)
            {
                Attack();
                timeOfNextAttack += BossManager.Instance.BossFightTime / BossManager.Instance.NumberOfAttacks;
            }

            yield return null;
        }

        if(!_dead)
        {
            BossManager.Instance.ComeBackToMonsterBattle();
        }
    }

    //This gets the 2 allies that have less numbers and kills an amount of them
    private void Attack()
    {
        List<AllyType> alliesIds = new List<AllyType>();

        foreach (AllyType ally in AllyManager.Instance.allies)
            alliesIds.Add(ally);

        alliesIds = alliesIds.OrderBy(x => x.NumberOfAllies).ToList();


        foreach(AllyType ally in alliesIds.FindAll(x => x.NumberOfAllies == 0))
            alliesIds.Remove(ally);
        

        if (AllyManager.Instance.allies.Find(x => x.AllyId == alliesIds[0].AllyId).NumberOfAllies - BossManager.Instance.NumberOfAlliesToKill > 0)
            AllyManager.Instance.allies.Find(x => x.AllyId == alliesIds[0].AllyId).NumberOfAllies -= BossManager.Instance.NumberOfAlliesToKill;
        else
            AllyManager.Instance.allies.Find(x => x.AllyId == alliesIds[0].AllyId).NumberOfAllies = 0;

        if (AllyManager.Instance.allies.Find(x => x.AllyId == alliesIds[1].AllyId).NumberOfAllies - BossManager.Instance.NumberOfAlliesToKill > 0)
            AllyManager.Instance.allies.Find(x => x.AllyId == alliesIds[1].AllyId).NumberOfAllies -= BossManager.Instance.NumberOfAlliesToKill;
        else
            AllyManager.Instance.allies.Find(x => x.AllyId == alliesIds[1].AllyId).NumberOfAllies = 0;

        AudioManager.Instance.PlayKillAllySound();
    }

    public void GetRewards(int goalIndex)
    {
        PlayerManager.Instance.ActualExperience += BossManager.Instance.GetBossExpByGoal(goalIndex);
        BossManager.Instance.Goals[goalIndex].achieved = true;
    }

    public float GetDamageTaken()
    {
        return _damageTaken;
    }
}
