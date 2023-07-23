using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FinalCredits : MonoBehaviour
{

    public List<MainMenuUI.Credit> credits;

    public TextMeshProUGUI CreditHeaderText;
    public TextMeshProUGUI CreditText;
    public Image MonsterCredit;

    private int _creditIndex;

    private Sequence _nonSelectedTween;

    void OnEnable()
    {
        CreditHeaderText.text = credits[_creditIndex].CreditHeader;
        CreditText.text = credits[_creditIndex].CreditName;
        MonsterCredit.sprite = credits[_creditIndex].CreditSprite;
        transform.DOScale(1, 0.5f).SetEase(Ease.InBounce).Play();
    }

    public void ClickOnCredit()
    {
        Sequence sq = DOTween.Sequence();

        sq.Append(transform.DOScale(0f, 0.1f).SetEase(Ease.Linear));

        sq.Play();

        sq.OnComplete(() => {

            _creditIndex++;

            if (_creditIndex >= credits.Count)
                _creditIndex = 0;

            CreditHeaderText.text = credits[_creditIndex].CreditHeader;
            CreditText.text = credits[_creditIndex].CreditName;
            MonsterCredit.sprite = credits[_creditIndex].CreditSprite;

            transform.DOScale(1, 0.1f).SetEase(Ease.Linear).Play();

        });


        AudioManager.Instance.PlayHitSound();
    }
}
