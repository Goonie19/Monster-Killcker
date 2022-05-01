using Sirenix.OdinInspector;
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
        FMOD.Studio.EventInstance instance = FMODUnity.RuntimeManager.CreateInstance(Placeholders.HIT_SFX_EVENT_PATH);
        instance.start();
    }

}
