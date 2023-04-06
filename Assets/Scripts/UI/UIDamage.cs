using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIDamage : MonoBehaviour
{

    public TextMeshProUGUI DmgText;

    private bool moving;

    private float speed;

    private Sequence LifeDamageSequence;

    void Update()
    {
        if(moving)
            transform.Translate(Vector2.up * Time.deltaTime * speed);
    }

    public void ShowText(string text, float TextSpeed)
    {
        DmgText.text = text + " Dmg";

        LifeDamageSequence.Kill();

        LifeDamageSequence = DOTween.Sequence();

        transform.position = Input.mousePosition;

        LifeDamageSequence.Append(DmgText.transform.DOMoveX(Input.mousePosition.x + Random.Range(-100f, 100f), 0.2f).SetEase(Ease.InBounce));
        LifeDamageSequence.Join(DmgText.transform.DOPunchScale(new Vector3(0.2f,0.2f,0.2f), 0.5f).SetEase(Ease.InBounce));

        LifeDamageSequence.Append(DmgText.DOFade(0, 3f));


        speed = TextSpeed;
        moving = true;

        LifeDamageSequence.OnComplete(() =>
        {
            moving = false;

            DmgText.DOFade(1f, 0f);

            gameObject.SetActive(false);
            
        });

        LifeDamageSequence.Play();
        


    }
}
