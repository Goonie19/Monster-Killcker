using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public abstract class PowerUp : MonoBehaviour
{
    [Title("References")]
    public TextMeshProUGUI ExpText;
    public TextMeshProUGUI AmountText;

    [Title("Bonuses")]
    public float AttackBonus;
    public float MultiplierBonus;

    [Title("Price")]
    public float ExpRequired;
    public float PriceMultiplier = 1.3f;

    protected int _amount;

    void Start()
    {
        ExpText.text = ExpRequired.ToString() + " Exp";
    }

    // Update is called once per frame
    void Update()
    {
        if (PlayerManager.Instance.Experience >= ExpRequired)
            GetComponent<Button>().interactable = true;
        else
            GetComponent<Button>().interactable = false;
    }

    public abstract void GetPowerUp();
}
