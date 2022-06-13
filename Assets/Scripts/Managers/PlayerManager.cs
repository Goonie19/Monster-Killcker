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

    [Title("Buff List")]
    public List<Buff> buffs;

    public float ActualExperience
    {
        get => _actualExperience;
        set
        {
            _actualExperience = value;

            UIManager.Instance.ExperienceDisplayText.text = _actualExperience.ToString();
        }
    }

    public int ActualHeads
    {
        get => _actualHeads;
        set
        {
            _actualHeads = value;

            UIManager.Instance.HeadsDisplayText.text = _actualHeads.ToString();
        }
    }

    private float _actualExperience;
    private int _actualHeads;

    private void Awake()
    {
        Instance = this;

        foreach(Buff b in buffs)
        {
            b.SetUnlockedToFalse();
            b.Reset();
        }
        
    }

    public void CheckBuffs()
    {

        foreach (Buff b in buffs)
        {
            if (b.HeadsToUnlock <= ActualHeads)
            {
                b.Unlock();

            }
        }

        UIManager.Instance.CheckButtonInteraction();
    }
}
