using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AllyButton : MonoBehaviour
{
    public int AllyId;

 
    public float PriceMultiplier = 1.3f;
    public TextMeshProUGUI ExpText;
    public TextMeshProUGUI LvlText;

    private int _level;

    private void Awake()
    {
        ExpText.text = AlliesManager.Instance.ActiveAllies[AllyId].ExperienceRequired.ToString() + " Exp";
    }

    private void Update()
    {
        if (PlayerManager.Instance.Experience >= AlliesManager.Instance.ActiveAllies[AllyId].ExperienceRequired)
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
        PlayerManager.Instance.Experience -= AlliesManager.Instance.ActiveAllies[AllyId].ExperienceRequired;
        AlliesManager.Instance.BuffAlly(AllyId, 1, 0, 0, PriceMultiplier);
        ExpText.text = AlliesManager.Instance.ActiveAllies[AllyId].ExperienceRequired.ToString() + " Exp";
        LvlText.text = "X" + _level.ToString();

        FMOD.Studio.EventInstance instance = FMODUnity.RuntimeManager.CreateInstance(Placeholders.COIN_SFX_EVENT_PATH);
        instance.start();
    }
}
