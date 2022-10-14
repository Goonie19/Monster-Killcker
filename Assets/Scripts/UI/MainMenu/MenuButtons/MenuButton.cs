using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class MenuButton : MonoBehaviour
{
    
    public void SelectedBehaviour()
    {
        transform.DOScale(1.2f, 0.1f).SetEase(Ease.Linear).Play();
    }

    public void DeselectedBehaviour()
    {
        transform.DOScale(1, 0.3f).SetEase(Ease.Linear).Play();
    }

}
