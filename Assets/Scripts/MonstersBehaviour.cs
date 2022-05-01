using DG.Tweening;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.Interactions;
using UnityEngine.UI;

public class MonstersBehaviour : MonoBehaviour
{
    public Image HealthFillImage;
    public Transform DamageDisplaySpawn;
    public GameObject DamageInfoPrefab;
    [Title("Oleadas")]
    [SerializeField]
    public List<MonsterWave> waves;
    public Button NextMonsterButton;
    public Button PreviousMonsterButton;

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
    public struct MonsterData
    {
        public string Name;
        public Sprite sprite;
        public float LifePoints;
        public float MonsterExperience;
    }

    #endregion

    private float _life;
    private bool _dead = false;
    private Image _renderer;
    private Animator _anim;

    private int _monstersLevels;



    private void Awake()
    {
        _monstersLevels = 0;
        _life = waves[_monstersLevels].GetMonster().LifePoints;
        _renderer = GetComponentInChildren<Image>();
        _anim = GetComponent<Animator>();
        NextMonsterButton.interactable = waves[_monstersLevels].Completed;

        if (_monstersLevels == 0)
            PreviousMonsterButton.interactable = false;
    }

    public void TakeDamage(float damage, bool animate = true, bool player = false)
    {
        
        if(_life > 0)
        {
            _life -= damage;

            if(player)
            {
                GameObject display = Instantiate(DamageInfoPrefab, DamageDisplaySpawn);
                display.GetComponent<DamageInfoText>().SetDamage(damage);

                FMOD.Studio.EventInstance instance = FMODUnity.RuntimeManager.CreateInstance(Placeholders.HIT_SFX_EVENT_PATH);
                instance.start();
            }

            HealthFillImage.fillAmount = _life / (waves[_monstersLevels].GetMonster().LifePoints);

            if (_life <= 0 && !_dead)
                Die();
            else if(animate)
                _anim.SetTrigger("GetHit");
        }

        
        
    }

    private void Die()
    {
        _anim.SetTrigger("Die");
        _dead = true;
        PlayerManager.Instance.Experience += waves[_monstersLevels].GetMonster().MonsterExperience;

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

        _renderer.sprite = waves[_monstersLevels].GetMonster().sprite;
        _life = waves[_monstersLevels].GetMonster().LifePoints;
        HealthFillImage.fillAmount = 1;

        NextMonsterButton.interactable = waves[_monstersLevels].Completed;

        if (_monstersLevels == 0)
            PreviousMonsterButton.interactable = false;
        else
            PreviousMonsterButton.interactable = true;

        _renderer.SetNativeSize();
        _anim.SetTrigger("Spawn");
    }
    
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
        
        _renderer.sprite = waves[_monstersLevels].GetMonster().sprite;
        _renderer.SetNativeSize();
    }

}
