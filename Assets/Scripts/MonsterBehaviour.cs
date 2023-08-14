using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MonsterBehaviour : MonoBehaviour
{

    public float DyingTime = 0.2f;

    public Image MonsterImage;

    private bool _speakSpawned;

    private bool _dead;

    private bool _autoClick;

    public float ActualHealth
    {
        get => _actualHealth;
    }

    private float _actualHealth;

    private void Awake()
    {
        MonsterManager.Instance.OnParametersInitialized.AddListener(InitializeParameters);
    }

    private void Start()
    {
        StartCoroutine(SpawnConversation());
    }

    void InitializeParameters()
    {
        _actualHealth = MonsterManager.Instance.GetHealth();
        _dead = false;
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

            UIDamagePool.instance.ShowDamage(PlayerManager.Instance.BaseDamage);

            TakeDamage(PlayerManager.Instance.BaseDamage);
           
        }
    }

    public void TakeDamage(float damage)
    {
        if (!_dead)
        {

            _actualHealth -= damage;
            PlayerManager.Instance.TotalDamage += damage;

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
            SaveDataManager.Instance.SaveData();
            Spawn(); 
        });
        
    }

    void Spawn()
    {

        MonsterImage.sprite = MonsterManager.Instance.MonsterSprites[Random.Range(0, MonsterManager.Instance.MonsterSprites.Count)];

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
                
                Invoke("StopSpeaking", 6f);
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

    #region TESTING
    [ContextMenu("Start AutoClick")]
    public void StartAutoClick()
    {
        _autoClick = true;
        StartCoroutine(AutoClick());
    }

    IEnumerator AutoClick()
    {
        while (_autoClick)
        {
            ClickMonster();

            yield return new WaitForSeconds(0.25f);
        }
    }

    [ContextMenu("Stop AutoClick")]
    public void StopAutoClick()
    {
        _autoClick = false;
    }

    #endregion
}
