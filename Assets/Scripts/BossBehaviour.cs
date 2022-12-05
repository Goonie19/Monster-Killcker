using DG.Tweening;
using System;
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
            parameters = GameManager.Instance.DefaultBossData;

        _actualHealth = parameters.actualHealth;

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

            TakeDamage(PlayerManager.Instance.BaseDamage);

        }
    }

    public void TakeDamage(float damage)
    {
        if (!_dead)
        {

            _actualHealth -= damage;
            _damageTaken += damage;

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

    private void Die()
    {
        _dead = true;

        _actualHealth = 0;
        UIManager.Instance.UpdateHealthBar(_actualHealth);

        BossManager.Instance.EndBoss();
    }

    public void Spawn()
    {
        BossImage.gameObject.SetActive(true);
    }

    IEnumerator BossAttackBehaviour()
    {
        _timer = 0;

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

    private void Attack()
    {
        int i = 0;

        while(i < AllyManager.Instance.allies.Count)
        {
            if (AllyManager.Instance.allies[i].NumberOfAllies - BossManager.Instance.NumberOfAlliesToKill > 0)
                AllyManager.Instance.allies[i].NumberOfAllies -= BossManager.Instance.NumberOfAlliesToKill;
            else
                AllyManager.Instance.allies[i].NumberOfAllies = 0;

            ++i;
        }

    }

    public void GetRewards(int goalIndex)
    {
        PlayerManager.Instance.ActualExperience += BossManager.Instance.GetBossExpByGoal(goalIndex);
    }

    public float GetDamageTaken()
    {
        return _damageTaken;
    }
}
