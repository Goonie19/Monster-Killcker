using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterBehaviour : MonoBehaviour
{

    public float DyingTime = 0.2f;

    private bool _speakSpawned;

    private bool _dead;

    public float ActualHealth
    {
        get => _actualHealth;
    }

    private float _actualHealth;

    private void Start()
    {
        _actualHealth = MonsterManager.Instance.GetHealth();
        _dead = false;

        StartCoroutine(SpawnConversation());
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

            TakeDamage(PlayerManager.Instance.BaseDamage * PlayerManager.Instance.DamageMultiplier);
           
        }
    }

    public void TakeDamage(float damage)
    {
        if (!_dead)
        {

            _actualHealth -= damage;

            if (_actualHealth > 0)
            {
                UIManager.Instance.UpdateInfoPanels();
                UIManager.Instance.UpdateHealthBar(_actualHealth);
            }
            else
                Die();

        }
    }

    void Die()
    {
        _dead = true;

        _actualHealth = 0;
        UIManager.Instance.UpdateInfoPanels();
        UIManager.Instance.UpdateHealthBar(_actualHealth);

        StopSpeaking();
        CancelInvoke("StopSpeaking");

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

            UIManager.Instance.UpdateInfoPanels();
            UIManager.Instance.UpdateHealthBar(_actualHealth);

        });
    }

    void GetRewards()
    {
        PlayerManager.Instance.ActualExperience += MonsterManager.Instance.GetExperience();
        PlayerManager.Instance.ActualHeads += MonsterManager.Instance.GetHeads();

    }

    IEnumerator SpawnConversation()
    {
        float SpawnTimer = 0;

        while(true)
        {
            SpawnTimer = Random.Range(1, 10);
            yield return new WaitForSeconds(SpawnTimer);

            if (!_dead)
                Speak();
        }
    }

    void Speak()
    {
        if(!_speakSpawned)
        {
            UIManager.Instance.DialogueText.text = MonsterManager.Instance.GetRandomPhrase();
            Sequence sq = DOTween.Sequence();

            sq.Append(UIManager.Instance.DialoguePanel.transform.DOScale(1, DyingTime).SetEase(Ease.Linear));

            sq.Play();

            sq.OnComplete(() => {
                _speakSpawned = true;
                
                Invoke("StopSpeaking", 2f);
            });

           
        }
    }

    void StopSpeaking()
    {
        Sequence sq = DOTween.Sequence();

        sq.Append(UIManager.Instance.DialoguePanel.transform.DOScale(0, DyingTime).SetEase(Ease.Linear));

        sq.Play();

        sq.OnComplete(() => {
            _speakSpawned = false;
        });
    }
}
