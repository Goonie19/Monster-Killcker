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
        IconRender.sprite = b.Icon;
        IconShadow.sprite = b.Icon;

        BuffName.text = b.BuffName;
        if (NumberOfBuffs)
            NumberOfBuffs.text = "x" + b.NumberOfBuffs.ToString();

        GetComponent<Button>().onClick.RemoveAllListeners();

        GetComponent<Button>().onClick.AddListener(b.ApplyBuff);

        _associatedBuff = b;

    }

    public void UpdateInfo()
    {
        IconRender.sprite = _associatedBuff.Icon;
        IconShadow.sprite = _associatedBuff.Icon;

        BuffName.text = _associatedBuff.BuffName;
        if (NumberOfBuffs)
            NumberOfBuffs.text = "x" + _associatedBuff.NumberOfBuffs.ToString();
    }

    public int GetBuffId()
    {
        return _associatedBuff.Id;
    }

}
