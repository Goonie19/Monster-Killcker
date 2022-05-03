using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerPowerUp : PowerUp
{

    public override void GetPowerUp()
    {
        ++_amount;
        PlayerManager.Instance.Experience -= ExpRequired;
        ExpRequired *= PriceMultiplier;
        PlayerManager.Instance.BaseDamage += AttackBonus;
        PlayerManager.Instance.multiplierDamage += MultiplierBonus;
        ExpText.text = ExpRequired.ToString() + " Exp";
        AmountText.text = "X" + _amount.ToString();

        FMOD.Studio.EventInstance instance = FMODUnity.RuntimeManager.CreateInstance(Placeholders.COIN_SFX_EVENT_PATH);
        instance.start();
    }
}
