using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BuffInfoPanel : MonoBehaviour
{

    public TextMeshProUGUI BuffTitle;
    public TextMeshProUGUI BuffDescription;
    public TextMeshProUGUI NumberOfBuffs;

    public void Setup(Buff b)
    {
        BuffTitle.text = b.BuffName;
        BuffDescription.text = b.BuffDescription;
        NumberOfBuffs.text = b.NumberOfBuffs.ToString();
        if (!b.OneUseBuff)
            NumberOfBuffs.transform.gameObject.SetActive(true);
        else
            NumberOfBuffs.transform.gameObject.SetActive(false);

        transform.position = Input.mousePosition;

    }

}
