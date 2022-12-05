using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AllyInfoPanel : MonoBehaviour
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
        if(ally.NumberOfAllies > 0)
        {
            StatsDisplay.gameObject.SetActive(true);
            BaseDamagePerSecondText.text = ally.BaseDamage.ToString();
            TotalDamageText.text = ally.GetDamage().ToString();
        } else
            StatsDisplay.gameObject.SetActive(false);


        transform.position = Input.mousePosition;
    }
}
