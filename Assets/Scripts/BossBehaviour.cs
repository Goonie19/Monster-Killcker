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
    private bool _dead;

    private float _timer;

    private void Awake()
    {
        _timer = BossManager.Instance.BossFightTime;
        _actualHealth = BossManager.Instance.BossHealth;

    }

    public void ClickMonster()
    {

        if (!_dead)
        {
            Sequence sq = DOTween.Sequence();

            sq.Append(transform.DOScale(0.8f, 0.1f).SetEase(Ease.Linear));
            sq.Append(transform.DOScale(1, 0.1f).SetEase(Ease.Linear));

            sq.Play();

            TakeDamage(PlayerManager.Instance.BaseDamage * PlayerManager.Instance.DamageMultiplier);

        }
    }

    public void TakeDamage(float damage)
    {
        if (!_dead)
        {

            _actualHealth -= damage;

            if (_actualHealth <= 0)
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

        BossManager.Instance.EndBoss();
    }

    public void Spawn()
    {
        BossImage.gameObject.SetActive(true);
    }

    IEnumerator BossAttackBehaviour()
    {
        _timer = 0;

        int i = 1;
        float timeOfNextAttack = BossManager.Instance.BossFightTime / BossManager.Instance.NumberOfAttacks;

        while(_timer < BossManager.Instance.BossFightTime)
        {
            _timer += Time.deltaTime;

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
}
