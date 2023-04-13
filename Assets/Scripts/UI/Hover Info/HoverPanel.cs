using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HoverPanel : MonoBehaviour
{
    public TextMeshProUGUI HoverNameText;
    public TextMeshProUGUI HoverDescriptionText;



    public void Setup(string HoverName, string HoverDescription)
    {
        HoverNameText.text = HoverName;
        HoverDescriptionText.text = HoverDescription;

        if (Input.mousePosition.y > Screen.height / 2)
            GetComponent<RectTransform>().pivot = new Vector2(0, 1);
        else
            GetComponent<RectTransform>().pivot = new Vector2(0, 0);

        UIManager.Instance.hoverPanel.transform.position = Input.mousePosition;
    }

}
