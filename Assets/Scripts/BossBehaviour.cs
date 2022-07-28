using DG.Tweening;
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

    private void Die()
    {

    }

    public void Spawn()
    {
        BossImage.gameObject.SetActive(true);
    }
}
