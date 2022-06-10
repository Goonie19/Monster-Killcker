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

    public void Setup(Buff b)
    {
        IconRender.sprite = b.Icon;
        IconShadow.sprite = b.Icon;

        BuffName.text = b.BuffName;
        if (NumberOfBuffs)
            NumberOfBuffs.text = "x" + b.NumberOfBuffs.ToString();
    }
}
