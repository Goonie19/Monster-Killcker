using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Instance;

    [Title("Damage of Player")]
    public float BaseDamage = 1;
    public float DamageMultiplier = 1;

    public float ActualExperience
    {
        get => _actualExperience;
        set => _actualExperience = value;
    }

    public int ActualHeads
    {
        get => _actualHeads;
        set => _actualHeads = value;
    }

    private float _actualExperience;
    private int _actualHeads;

    private void Awake()
    {
        Instance = this;
    }
}
