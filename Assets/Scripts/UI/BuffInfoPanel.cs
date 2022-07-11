using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BuffInfoPanel : MonoBehaviour
{

    public TextMeshProUGUI BuffTitle;
    public TextMeshProUGUI BuffDescription;
    public TextMeshProUGUI NumberOfBuffs;

    public Transform StatsDisplay;
    public TextMeshProUGUI BaseDamagePerSecondText;
    public TextMeshProUGUI MultiplierDamageText;
    public TextMeshProUGUI TotalDamageText;

    public void Setup(AllyType ally)
    {
        BuffTitle.text = ally.AllyName;
        BuffDescription.text = ally.Description;
        NumberOfBuffs.text = ally.NumberOfAllies.ToString();
        BaseDamagePerSecondText.text = ally.BaseDamage.ToString();
        MultiplierDamageText.text = ally.DamageMultiplier.ToString();
        TotalDamageText.text = ally.GetDamage().ToString();


        transform.position = Input.mousePosition;
    }
}
