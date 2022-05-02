using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AllyPowerUp : PowerUp
{

    [Title("Ally Parameters")]

    public int AllyId;

    public int HeadsRequiredToUnlock;

    public bool used = false;

    public override void GetPowerUp()
    {
        PlayerManager.Instance.Experience -= AlliesManager.Instance.ActiveAllies[AllyId].ExperienceRequired;
        AlliesManager.Instance.BuffAlly(AllyId, 0, AttackBonus, MultiplierBonus, PriceMultiplier);
        used = true;
        gameObject.SetActive(false);
    }
}
