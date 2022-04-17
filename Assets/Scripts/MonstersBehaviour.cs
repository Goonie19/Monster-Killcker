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
    [Title("Oleadas")]
    [SerializeField]
    public List<MonsterWave> waves;
    public Button NextMonsterButton;
    public Button PreviousMonsterButton;

    #region MONSTER WAVE BEHAVIOUR

    [Serializable]
    public class MonsterWave {
        public List<MonsterData> Monsters;
        public bool Completed;

        public bool unlockPowerUp;
        [ShowIf("unlockPowerUp")]
        public List<int> AllyIndexes;


        private int _index;

        public MonsterData Peek()
        {
            return Monsters[_index];
        }

        public int GetIndex()
        {
            return _index;
        }

        public MonsterData NextMonster()
        {
            if (_index < Monsters.Count - 1)
                ++_index;
            else
            {
                _index = 0;
                if (!Completed && unlockPowerUp)
                    AlliesManager.Instance.UnlockAllies(AllyIndexes);

                Completed = true;
                

            }
            return Monsters[_index];
            
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
        _life = waves[_monstersLevels].Peek().LifePoints;
        _renderer = GetComponentInChildren<Image>();
        _anim = GetComponent<Animator>();
        NextMonsterButton.interactable = waves[_monstersLevels].Completed;

        if (_monstersLevels == 0)
            PreviousMonsterButton.interactable = false;
    }

    public void TakeDamage(float damage, bool animate = true)
    {
        
        if(_life > 0)
        {
            _life -= damage;

            HealthFillImage.fillAmount = _life / (waves[_monstersLevels].Peek().LifePoints);

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
        PlayerManager.Instance.Experience += waves[_monstersLevels].Peek().MonsterExperience;
        Invoke("Spawn", 1f);
    }


    void Spawn()
    {
        _dead = false;
        _renderer.sprite = waves[_monstersLevels].NextMonster().sprite;
        _life = waves[_monstersLevels].Peek().LifePoints;
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

        _life = waves[_monstersLevels].Peek().LifePoints;
        _dead = false;
    }

    public void changeRandomSprite()
    {
        
        _renderer.sprite = waves[_monstersLevels].Peek().sprite;
        _renderer.SetNativeSize();
    }

}
