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

    public ButtonType Type;

    public enum ButtonType { Buff, Ally}

    private Buff _associatedBuff;

    private AllyType _associatedAlly;

    public void Setup(AllyType ally)
    {
        Type = ButtonType.Ally;

        _associatedAlly = ally;

        if (PlayerManager.Instance.ActualHeads < ally.Price || PlayerManager.Instance.ActualExperience < ally.Price)
            GetComponent<Button>().interactable = false;

        IconRender.sprite = ally.Icon;
        IconShadow.sprite = ally.Icon;

        BuffName.text = ally.AllyName;

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

    public void UpdateInfo()
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

}
