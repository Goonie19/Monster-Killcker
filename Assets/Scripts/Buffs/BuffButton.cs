using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BuffButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {
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

    public Transform HoverPosition;

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

        if(ally.GetPrice() % 1 == 0)
            ExperienceText.text = ally.GetPrice().ToString();
        else
            ExperienceText.text = string.Format("{0:0.00}", ally.GetPrice());

        GetComponent<Button>().onClick.RemoveAllListeners();

        GetComponent<Button>().onClick.AddListener(() =>
        {
            if (GameManager.Instance.TenShopMode)
                BuyTenAllies();
            else
                ally.BuyAlly();
        });
        GetComponent<Button>().onClick.AddListener(AudioManager.Instance.PlayBuySound);

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

        if(b.GetPrice() % 1 == 0)
            ExperienceText.text = b.GetPrice().ToString();
        else
            ExperienceText.text = string.Format("{0:0.00}", b.GetPrice());

        GetComponent<Button>().onClick.RemoveAllListeners();

        GetComponent<Button>().onClick.AddListener(() =>
        {
            if (GameManager.Instance.TenShopMode)
                BuyTenBuffs();
            else
                b.ApplyBuff();
        });
        GetComponent<Button>().onClick.AddListener(AudioManager.Instance.PlayBuySound);

        
        _associatedBuff = b;

    }

    //You can buy allies and buffs in packs of 10.
    public void BuyTenBuffs()
    {
        int i = 0;

        while(i < 10 && ((_associatedBuff is MonsterBuff && _associatedBuff.GetPrice() <= PlayerManager.Instance.ActualHeads) ||
            PlayerManager.Instance.ActualExperience >= _associatedBuff.GetPrice() && !(_associatedBuff is MonsterBuff)))
        {
            _associatedBuff.ApplyBuff();

            ++i;
        }
    }

    public void BuyTenAllies()
    {
        int i = 0;

        while (i < 10)
        {
            _associatedAlly.BuyAlly();

            ++i;
        }
    }

    public void UpdateInfo(bool attack = false)
    {
        if(Type == ButtonType.Buff)
        {
            IconRender.sprite = _associatedBuff.Icon;
            IconShadow.sprite = _associatedBuff.Icon;

            if(!GameManager.Instance.TenShopMode) // Muestra la compra de 10 en 10 o de 1 en 1
            {
                if (_associatedBuff.GetPrice() % 1 == 0)
                    ExperienceText.text = _associatedBuff.GetPrice().ToString();
                else
                    ExperienceText.text = string.Format("{0:0.00}", _associatedBuff.GetPrice());
            }else
            {
                if (_associatedBuff.GetTenPrice() % 1 == 0)
                    ExperienceText.text = _associatedBuff.GetTenPrice().ToString();
                else
                    ExperienceText.text = string.Format("{0:0.00}", _associatedBuff.GetTenPrice());
            }

            BuffName.text = _associatedBuff.BuffName;
            if (NumberOfBuffs)
                NumberOfBuffs.text = "x" + _associatedBuff.NumberOfBuffs.ToString();

        } else
        {
            IconRender.sprite = _associatedAlly.Icon;
            IconShadow.sprite = _associatedAlly.Icon;

            if(!GameManager.Instance.TenShopMode)
            {
                if (_associatedAlly.GetPrice() % 1 == 0)
                    ExperienceText.text = _associatedAlly.GetPrice().ToString();
                else
                    ExperienceText.text = string.Format("{0:0.00}", _associatedAlly.GetPrice());
            } else
            {
                if (_associatedAlly.GetTenPrice() % 1 == 0)
                    ExperienceText.text = _associatedAlly.GetTenPrice().ToString();
                else
                    ExperienceText.text = string.Format("{0:0.00}", _associatedAlly.GetTenPrice());
            }

            BuffName.text = _associatedAlly.AllyName;
            if (NumberOfBuffs)
                NumberOfBuffs.text = "x" + _associatedAlly.NumberOfAllies.ToString();
            if (attack)
                TakeDamageAnim();

        }

    }

    public void CheckInteractable()
    {
        if (!GameManager.Instance.TenShopMode)
            CheckDefaultInteractable();
        else
        {
            if(Type == ButtonType.Buff)
            {
                if (_associatedBuff.OneUseBuff)
                    CheckDefaultInteractable();
                else
                    CheckTenModeInteractable();
            } else
            {
                CheckTenModeInteractable();
            }
            

        }
        
    }

    void CheckDefaultInteractable()
    {
        if (Type == ButtonType.Buff)
        {
            if ((PlayerManager.Instance.ActualHeads <= _associatedBuff.GetPrice() && _associatedBuff is MonsterBuff) ||
            PlayerManager.Instance.ActualExperience <= _associatedBuff.GetPrice() && !(_associatedBuff is MonsterBuff))
                GetComponent<Button>().interactable = false;
            else
                GetComponent<Button>().interactable = true;

        }
        else
        {
            if (PlayerManager.Instance.ActualExperience <= _associatedAlly.GetPrice())
                GetComponent<Button>().interactable = false;
            else
                GetComponent<Button>().interactable = true;

        }
    }
   
    void CheckTenModeInteractable()
    {
        if (Type == ButtonType.Buff)
        {
            if ((PlayerManager.Instance.ActualHeads <= _associatedBuff.GetTenPrice() && _associatedBuff is MonsterBuff) ||
            PlayerManager.Instance.ActualExperience <= _associatedBuff.GetTenPrice() && !(_associatedBuff is MonsterBuff))
                GetComponent<Button>().interactable = false;
            else
                GetComponent<Button>().interactable = true;

        }
        else
        {
            if (PlayerManager.Instance.ActualExperience <= _associatedAlly.GetTenPrice())
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
        {
            UIManager.Instance.SpawnInfoPanel(_associatedBuff);
            UIManager.Instance.buffInfoPanel.transform.position = HoverPosition.position;
        }
        else
        {
            UIManager.Instance.SpawnInfoPanel(_associatedAlly);
            UIManager.Instance.allyInfoPanel.transform.position = HoverPosition.position;
        }
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
                if(GameManager.Instance.GetAlliesHovers())
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

    public void OnPointerExit(PointerEventData eventData)
    {
        OnButton(false);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        OnButton(true);
    }

    #endregion


}
