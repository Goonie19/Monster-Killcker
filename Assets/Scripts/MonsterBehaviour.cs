using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterBehaviour : MonoBehaviour
{

    public float DyingTime = 0.2f;

    private bool _dead;
    private float _actualHealth;

    private void Start()
    {
        _actualHealth = MonsterManager.Instance.GetHealth();
        _dead = false;
    }

    public void ClickMonster()
    {
        
        if(!_dead)
        {
            _actualHealth -= PlayerManager.Instance.BaseDamage * PlayerManager.Instance.DamageMultiplier;

            if (_actualHealth <= 0)
                Die();
        }
    }

    void Die()
    {
        _dead = true;

        Sequence sq = DOTween.Sequence();

        sq.Append(transform.DOScale(0, DyingTime));

        sq.Play();

        sq.OnComplete(() => {
            GetRewards();
            PlayerManager.Instance.CheckBuffs();
            Spawn(); 
        });
        
    }

    void Spawn()
    {
        Sequence sq = DOTween.Sequence();

        sq.Append(transform.DOScale(1, DyingTime));

        sq.Play();

        sq.OnComplete(() => { 
            
            _dead = false;

            _actualHealth = MonsterManager.Instance.GetHealth();
            
        });
    }

    void GetRewards()
    {
        PlayerManager.Instance.ActualExperience += MonsterManager.Instance.GetExperience();
        PlayerManager.Instance.ActualHeads += MonsterManager.Instance.GetHeads();
    }

}
