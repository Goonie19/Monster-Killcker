using DG.Tweening;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.InputSystem.Interactions;
using UnityEngine.UI;

public class MonstersBehaviour : MonoBehaviour
{
    public Image HealthFillImage;
    public Image Renderer;
    public Image BossRenderer;

    public Transform DamageDisplaySpawn;
    public GameObject DamageInfoPrefab;
    [Title("Oleadas")]
    [SerializeField]
    public AnimatorController EnemyAnimator;
    public List<MonsterWave> waves;
    public Button NextMonsterButton;
    public Button PreviousMonsterButton;

    [Title("Boss Final")]
    public Image FlashImage;
    public AnimatorController BossAnimator;
    public MonsterData BossData;

    [Title("Iteraciones")]
    public float LifeMultiplier;
    public float ExperienceMultiplier;
    public int headsMultiplier;


    #region MONSTER WAVE BEHAVIOUR

    [Serializable]
    public class MonsterWave {
        public MonsterData Monster;
        public bool Completed;

        public bool unlockPowerUp;
        [ShowIf("unlockPowerUp")]
        public List<int> AllyIndexes;


        private int _index;

        public MonsterData GetMonster()
        {
            return Monster;
        }

    }
    [Serializable]
    public class MonsterData
    {
        public string Name;
        public Sprite sprite;
        public float LifePoints;
        public float MonsterExperience;
        public int MonsterHeads;
    }

    #endregion

    private float _life;
    private bool _dead = false;
    private Animator _anim;

    private bool _isOnBoss;
    private float _bossActualLifePoints;

    private int _monstersLevels;



    private void Awake()
    {
        _monstersLevels = 0;
        _life = waves[_monstersLevels].GetMonster().LifePoints;
        _bossActualLifePoints = BossData.LifePoints;
        _anim = GetComponent<Animator>();
        _anim.runtimeAnimatorController = EnemyAnimator;
        NextMonsterButton.interactable = waves[_monstersLevels].Completed;

        if (_monstersLevels == 0)
            PreviousMonsterButton.interactable = false;
    }

    public void TakeDamage(float damage, bool animate = true, bool player = false)
    {
        
        if(!_isOnBoss)
        {
            if (_life > 0)
            {
                _life -= damage;

                if (player)
                {
                    GameObject display = Instantiate(DamageInfoPrefab, DamageDisplaySpawn);
                    display.GetComponent<DamageInfoText>().SetDamage(damage);

                    FMOD.Studio.EventInstance instance = FMODUnity.RuntimeManager.CreateInstance(Placeholders.HIT_SFX_EVENT_PATH);
                    instance.start();
                }

                HealthFillImage.fillAmount = _life / (waves[_monstersLevels].GetMonster().LifePoints);

                if (_life <= 0 && !_dead)
                    Die();
                else if (animate)
                    _anim.SetTrigger("GetHit");
            }
        } else
        {
            if (_bossActualLifePoints > 0)
            {
                _bossActualLifePoints -= damage;

                if (player)
                {
                    GameObject display = Instantiate(DamageInfoPrefab, DamageDisplaySpawn);
                    display.GetComponent<DamageInfoText>().SetDamage(damage);

                    FMOD.Studio.EventInstance instance = FMODUnity.RuntimeManager.CreateInstance(Placeholders.HIT_SFX_EVENT_PATH);
                    instance.start();
                }

                HealthFillImage.fillAmount = _bossActualLifePoints / (BossData.LifePoints);

                if (_life <= 0 && !_dead)
                    BossDie();
                else if (animate)
                    _anim.SetTrigger("GetHit");
            }
        }

        
        
    }

    private void Die()
    {
        _anim.SetTrigger("Die");
        _dead = true;
        PlayerManager.Instance.Experience += waves[_monstersLevels].GetMonster().MonsterExperience;
        PlayerManager.Instance.AddMonsterHead(waves[_monstersLevels].GetMonster().MonsterHeads);

        if (!waves[_monstersLevels].Completed && waves[_monstersLevels].unlockPowerUp)
            AlliesManager.Instance.UnlockAllies(waves[_monstersLevels].AllyIndexes);

        if (_monstersLevels + 1 < waves.Count)
        {
            if (!waves[_monstersLevels].Completed)
            {
                waves[_monstersLevels].Completed = true;
                ++_monstersLevels;
            } else
                waves[_monstersLevels].Completed = true;
        }
        Invoke("Spawn", 1f);
    }

    void Spawn()
    {
        _dead = false;        

        Renderer.sprite = waves[_monstersLevels].GetMonster().sprite;
        _life = waves[_monstersLevels].GetMonster().LifePoints;
        HealthFillImage.fillAmount = 1;

        NextMonsterButton.interactable = waves[_monstersLevels].Completed;

        if (_monstersLevels == 0)
            PreviousMonsterButton.interactable = false;
        else
            PreviousMonsterButton.interactable = true;

        Renderer.SetNativeSize();
        _anim.SetTrigger("Spawn");
    }

    #region BOSS

    [ContextMenu("Spawn Boss")]
    void BossSpawn()
    {
        _anim.runtimeAnimatorController = BossAnimator;
        _isOnBoss = true;

        HealthFillImage.fillAmount = 1;

        Sequence sq = DOTween.Sequence();

        sq.Append(FlashImage.DOFade(1, 1).SetEase(Ease.Linear));

        sq.Play();

        sq.OnComplete(() => { BossRenderer.gameObject.SetActive(true); Renderer.gameObject.SetActive(false); Deflash(); StartCoroutine(BossBehaviour()); });

    }

    void Deflash()
    {
        Sequence sq = DOTween.Sequence();

        sq.Append(FlashImage.DOFade(0, 1).SetEase(Ease.Linear));

        sq.Play();
    }

    [ContextMenu("Boss Die")]
    private void BossDie()
    {
        StartCoroutine(BossDieCoroutine());
    }

    IEnumerator BossDieCoroutine()
    {
        while (BossRenderer.material.GetFloat("_Fade") > 0)
        {
            yield return new WaitForSeconds(0.1f);
            float g = BossRenderer.material.GetFloat("_Fade");
            g -= 0.1f;
            BossRenderer.material.SetFloat("_Fade", g);
        }


    }

    IEnumerator BossBehaviour()
    {
        while(!AlliesDefeated())
        {
            SubstractFromAllAllies(10);

            yield return new WaitForSeconds(10f);
        }

    }

    bool AlliesDefeated()
    {
        bool defeated = true;
        int i = 0;
        while (i < AlliesManager.Instance.ActiveAllies.Count && defeated)
        {
            if (AlliesManager.Instance.ActiveAllies[i].Amount != 0)
                defeated = false;
            ++i;
        }

        return defeated;

    }

    void SubstractFromAllAllies(int amount)
    {
        int i = 0;

        while(i < AlliesManager.Instance.ActiveAllies.Count)
        {
            AlliesManager.Instance.ActiveAllies[i].Amount -= amount;

            ++i;
        }
    }

    #endregion

    public void ChangeMonster(bool add)
    {
        if (add)
        {
            if(_monstersLevels < waves.Count - 1)
            {
                ++_monstersLevels;
                _anim.SetTrigger("LeftFade");

            }
        }
        else 
        {
            if (_monstersLevels > 0)
            {
                --_monstersLevels;
                _anim.SetTrigger("RightFade");
            }
        }
        NextMonsterButton.interactable = waves[_monstersLevels].Completed && _monstersLevels != waves.Count - 1;


        if (_monstersLevels == 0)
            PreviousMonsterButton.interactable = false;
        else
            PreviousMonsterButton.interactable = true;

        _life = waves[_monstersLevels].GetMonster().LifePoints;
        _dead = false;
    }

    public void changeRandomSprite()
    {
        
        Renderer.sprite = waves[_monstersLevels].GetMonster().sprite;
        Renderer.SetNativeSize();
    }



}
