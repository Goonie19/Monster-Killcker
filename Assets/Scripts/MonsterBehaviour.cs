using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterBehaviour : MonoBehaviour
{

    private float _actualHealth;

    private void Start()
    {
        _actualHealth = MonsterManager.Instance.GetHealth();
    }

    public void ClickMonster()
    {
        _actualHealth -= PlayerManager.Instance.BaseDamage * PlayerManager.Instance.DamageMultiplier;

        if (_actualHealth <= 0)
            Die();
    }

    void Die()
    {
        Debug.Log("I died");
    }
}
