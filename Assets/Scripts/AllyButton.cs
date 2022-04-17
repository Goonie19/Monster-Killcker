using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AllyButton : MonoBehaviour
{
    public int AllyId;

    public float DamagePerSecond;
    public float ExpRequired;
    public float PriceMultiplier = 1.3f;
    public TextMeshProUGUI ExpText;
    public TextMeshProUGUI LvlText;

    private int _level;

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

    public void ActivateCharacter()
    {
        AlliesManager.Instance.ActivateAllyImage(AllyId);
    }

    public void BuffAlly()
    {
        ++_level;
        PlayerManager.Instance.Experience -= ExpRequired;
        ExpRequired *= PriceMultiplier;
        ExpText.text = ExpRequired.ToString() + " Exp";
        LvlText.text = "X" + _level.ToString();
        AlliesManager.Instance.BuffAlly(DamagePerSecond);
    }
}
