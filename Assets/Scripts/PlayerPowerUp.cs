using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerPowerUp : MonoBehaviour
{
    public float ExpRequired;
    public float PriceMultiplier = 1.3f;
    public TextMeshProUGUI ExpText;
    public TextMeshProUGUI AmountText;

    public float AttackBonus;
    public float MultiplierBonus;

    private int _amount;

    private void Awake()
    {
        ExpText.text = ExpRequired.ToString() + " Exp";
    }

    private void Update()
    {
        if (PlayerManager.Instance.Experience >= ExpRequired)
            GetComponent<Button>().interactable = true;
        else
            GetComponent<Button>().interactable = false;
    }

    public void GetPowerUp()
    {
        ++_amount;
        PlayerManager.Instance.Experience -= ExpRequired;
        ExpRequired *= PriceMultiplier;
        PlayerManager.Instance.BaseDamage += AttackBonus;
        PlayerManager.Instance.multiplierDamage += MultiplierBonus;
        ExpText.text = ExpRequired.ToString() + " Exp";
        AmountText.text = "X" + _amount.ToString();

    }
}
