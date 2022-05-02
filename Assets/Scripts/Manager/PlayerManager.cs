using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{

    public static PlayerManager Instance;


    public enum UnlockableAllies
    {
        Archer, Lancer
    }

    [Title("Player Info")]
    public float BaseDamage;
    public float multiplierDamage;

    [Title("References")]
    public MonstersBehaviour Monster;
    public Transform PowerUpParent;
    public TextMeshProUGUI ExpText;
    public TextMeshProUGUI HeadsText;

    [Title("Events")]
    public Action OnHeadsAdded;


    public float Experience
    {
        get => _experience;
        set
        {
            _experience = value;
            ExpText.text = _experience.ToString();
        }
    }

    public float MonsterHeads
    {
        get => _heads;
        set
        {
            _heads = value;
            HeadsText.text = _heads.ToString();
        }
    } 

    private float _heads;

    private float _experience;

    private void Awake()
    {
        Instance = this;
    }

    public void Attack()
    {
        Monster.TakeDamage(BaseDamage * multiplierDamage, true, true);
    }

    public void AddMonsterHead()
    {
        ++MonsterHeads;
        OnHeadsAdded?.Invoke();

    }

}
