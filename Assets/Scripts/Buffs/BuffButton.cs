using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BuffButton : MonoBehaviour
{
    public Image IconRender;
    public Image IconShadow;

    public Image MoneyImage;
    public Sprite MoneyAlly;
    public Sprite MoneyMonster;

    public TextMeshProUGUI BuffName;
    public TextMeshProUGUI NumberOfBuffs;
    public TextMeshProUGUI ExperienceText;

    public Animator SlashAnim;

    public ButtonType Type;

    public float MouseMovementThreshold;

    public float TimeToShowInfo = 0.5f;

    public enum ButtonType { Buff, Ally}

    private Buff _associatedBuff;

    private AllyType _associatedAlly;

    private bool _onButton;

    private Vector2 MouseLastPosition;

    private void Awake()
    {
        transform.DOScale(1, 1).SetEase(Ease.OutBounce).Play();
    }

    public void Setup(AllyType ally)
    {
        Type = ButtonType.Ally;

        _associatedAlly = ally;

        if (PlayerManager.Instance.ActualHeads < ally.Price || PlayerManager.Instance.ActualExperience < ally.Price)
            GetComponent<Button>().interactable = false;

        IconRender.sprite = ally.Icon;
        IconShadow.sprite = ally.Icon;

        BuffName.text = ally.AllyName;

        MoneyImage.sprite = MoneyAlly;

        if (NumberOfBuffs)
            NumberOfBuffs.text = "x" + ally.NumberOfAllies.ToString();

        ExperienceText.text = ally.Price.ToString();

        GetComponent<Button>().onClick.RemoveAllListeners();

        GetComponent<Button>().onClick.AddListener(ally.BuyAlly);
    }

    public void Setup(Buff b)
    {
        Type = ButtonType.Buff;

        if ((PlayerManager.Instance.ActualHeads < b.GetPrice()  && b is MonsterBuff) || PlayerManager.Instance.ActualExperience < b.GetPrice() && !(b is MonsterBuff))
            GetComponent<Button>().interactable = false;

        if (b is MonsterBuff)
            MoneyImage.sprite = MoneyMonster;
        else
            MoneyImage.sprite = MoneyAlly;

        IconRender.sprite = b.Icon;
        IconShadow.sprite = b.Icon;

        BuffName.text = b.BuffName;
        if (NumberOfBuffs)
            NumberOfBuffs.text = "x" + b.NumberOfBuffs.ToString();

        ExperienceText.text = b.GetPrice().ToString();

        GetComponent<Button>().onClick.RemoveAllListeners();

        GetComponent<Button>().onClick.AddListener(b.ApplyBuff);

        _associatedBuff = b;

    }

    public void UpdateInfo(bool attack = false)
    {
        if(Type == ButtonType.Buff)
        {
            IconRender.sprite = _associatedBuff.Icon;
            IconShadow.sprite = _associatedBuff.Icon;

            ExperienceText.text = _associatedBuff.GetPrice().ToString();

            BuffName.text = _associatedBuff.BuffName;
            if (NumberOfBuffs)
                NumberOfBuffs.text = "x" + _associatedBuff.NumberOfBuffs.ToString();

        } else
        {
            IconRender.sprite = _associatedAlly.Icon;
            IconShadow.sprite = _associatedAlly.Icon;

            ExperienceText.text = _associatedAlly.GetPrice().ToString();

            BuffName.text = _associatedAlly.AllyName;
            if (NumberOfBuffs)
                NumberOfBuffs.text = "x" + _associatedAlly.NumberOfAllies.ToString();
            if (attack)
                TakeDamageAnim();

        }

    }

    public void CheckInteractable()
    {
        if(Type == ButtonType.Buff)
        {
            if ((PlayerManager.Instance.ActualHeads < _associatedBuff.GetPrice() && _associatedBuff is MonsterBuff) ||
            PlayerManager.Instance.ActualExperience < _associatedBuff.GetPrice() && !(_associatedBuff is MonsterBuff))
                GetComponent<Button>().interactable = false;
            else
                GetComponent<Button>().interactable = true;
        } else
        {
            if (PlayerManager.Instance.ActualExperience < _associatedAlly.GetPrice())
                GetComponent<Button>().interactable = false;
            else
                GetComponent<Button>().interactable = true;
        }
    }

    public int GetBuffId()
    {
        if (Type == ButtonType.Ally)
            return _associatedAlly.AllyId;
        else
            return _associatedBuff.Id;
    }

    public void ShowInfo()
    {
        if (Type == ButtonType.Buff)
            UIManager.Instance.SpawnInfoPanel(_associatedBuff);
        else
            UIManager.Instance.SpawnInfoPanel(_associatedAlly);
    }

    public void OnButton(bool onButton)
    {
        _onButton = onButton;

        if (_onButton)
            StartCoroutine(ShowingInfo());
        else
        {
            if(_associatedAlly != null)
                UIManager.Instance.allyInfoPanel.gameObject.SetActive(false);
            else
                UIManager.Instance.buffInfoPanel.gameObject.SetActive(false);
            
        }
            
    }

    IEnumerator ShowingInfo()
    {
        float time = 0;
        MouseLastPosition = new Vector2(Input.mousePosition.x, Input.mousePosition.y);

        while (_onButton)
        {
            if (Vector2.Distance(MouseLastPosition, Input.mousePosition) > MouseMovementThreshold)
                time = 0;
            else
                time += 0.01f;

            if (time >= TimeToShowInfo)
            {
                ShowInfo();
                _onButton = false;
            }

            MouseLastPosition = Input.mousePosition;

            yield return new WaitForSeconds(0.01f);
        }

    }

    #region Animation

    public void TakeDamageAnim()
    {
        SlashAnim.gameObject.SetActive(true);
        Invoke(nameof(DisableDamage), 1f);
    }

    void DisableDamage()
    {
        SlashAnim.gameObject.SetActive(false);
    }

    #endregion


}
