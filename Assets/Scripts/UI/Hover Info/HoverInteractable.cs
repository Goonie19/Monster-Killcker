using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoverInteractable : MonoBehaviour
{
    public string Interactablename;
    [TextArea(1, 8)]
    public string Information;


    public void ShowInfo()
    {
        UIManager.Instance.hoverPanel.Setup(Interactablename, Information);

        UIManager.Instance.hoverPanel.gameObject.SetActive(true);

    }

    public void HideInfo()
    {
        UIManager.Instance.hoverPanel.gameObject.SetActive(false);
    }
}
