using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Instance;

    public float BaseDamage = 1;
    public float DamageMultiplier = 1;

    private void Awake()
    {
        Instance = this;
    }
}
