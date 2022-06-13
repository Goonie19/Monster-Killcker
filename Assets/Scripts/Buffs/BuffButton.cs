using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BuffButton : MonoBehaviour
{
    public Image IconRender;
    public Image IconShadow;

    public TextMeshProUGUI BuffName;
    public TextMeshProUGUI NumberOfBuffs;
    public TextMeshProUGUI ExperienceText;

    private Buff _associatedBuff;

    public void Setup(Buff b)
    {
        if ((PlayerManager.Instance.ActualHeads < b.Price && b is MonsterBuff) || PlayerManager.Instance.ActualExperience < b.Price && !(b is MonsterBuff))
            GetComponent<Button>().interactable = false;


        IconRender.sprite = b.Icon;
        IconShadow.sprite = b.Icon;

        BuffName.text = b.BuffName;
        if (NumberOfBuffs)
            NumberOfBuffs.text = "x" + b.NumberOfBuffs.ToString();

        ExperienceText.text = b.Price.ToString();

        GetComponent<Button>().onClick.RemoveAllListeners();

        GetComponent<Button>().onClick.AddListener(b.ApplyBuff);

        _associatedBuff = b;

    }

    public void UpdateInfo()
    {
        IconRender.sprite = _associatedBuff.Icon;
        IconShadow.sprite = _associatedBuff.Icon;

        ExperienceText.text = _associatedBuff.Price.ToString();

        BuffName.text = _associatedBuff.BuffName;
        if (NumberOfBuffs)
            NumberOfBuffs.text = "x" + _associatedBuff.NumberOfBuffs.ToString();
    }

    public void CheckInteractable()
    {
        if ((PlayerManager.Instance.ActualHeads < _associatedBuff.Price && _associatedBuff is MonsterBuff) || 
            PlayerManager.Instance.ActualExperience < _associatedBuff.Price && !(_associatedBuff is MonsterBuff))
            GetComponent<Button>().interactable = false;
        else
            GetComponent<Button>().interactable = true;
    }

    public int GetBuffId()
    {
        return _associatedBuff.Id;
    }

}
