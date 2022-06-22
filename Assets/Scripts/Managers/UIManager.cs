using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{

    public static UIManager Instance;

    [Title("Monster")]
    public MonsterBehaviour MonsterToClick;

    [Title("Buff Display")]
    public Transform MonsterBuffContentDisplay;
    public Transform AlliesBuffContentDisplay;

    [Title("Text References")]
    public TextMeshProUGUI ExperienceDisplayText;
    public TextMeshProUGUI HeadsDisplayText;

    [Title("Button Prefabs")]
    public GameObject AllyButton;
    public GameObject AllyButtonOneUse;
    public GameObject MonsterButton;
    public GameObject MonsterButtonOneUse;

    [Title("List of Buffs")]
    public List<BuffButton> AllyButtons;
    public List<BuffButton> UnlockedPlayerBuffs;
    public List<BuffButton> UnlockedAllyBuffs;
    public List<BuffButton> UnlockedMonsterBuffs;

    private void Awake()
    {
        Instance = this;
    }

    #region INSTANTIATE BUTTONS

    public void InstantiateAlly(AllyType ally)
    {
        GameObject b;

        b = Instantiate(AllyButton, AlliesBuffContentDisplay);

        b.GetComponent<BuffButton>().Setup(ally);
        AllyButtons.Add(b.GetComponent<BuffButton>());
    }

    public void InstantiateAllyButton(AllyBuff buff)
    {
        GameObject b;

        if (buff.OneUseBuff)
            b = Instantiate(AllyButtonOneUse, AlliesBuffContentDisplay);
        else
            b = Instantiate(AllyButton, AlliesBuffContentDisplay);
        
        b.GetComponent<BuffButton>().Setup(buff);
        UnlockedAllyBuffs.Add(b.GetComponent<BuffButton>());
    }

    public void InstantiateMonsterButton(MonsterBuff buff)
    {
        GameObject b;

        if (buff.OneUseBuff)
            b = Instantiate(MonsterButtonOneUse, MonsterBuffContentDisplay);
        else
            b = Instantiate(MonsterButton, MonsterBuffContentDisplay);

        b.GetComponent<BuffButton>().Setup(buff);
        UnlockedMonsterBuffs.Add(b.GetComponent<BuffButton>());
    }

    public void InstantiatePlayerBuffButton(PlayerBuff buff)
    {
        GameObject b;

        if (buff.OneUseBuff)
            b = Instantiate(AllyButtonOneUse, AlliesBuffContentDisplay);
        else
            b = Instantiate(AllyButton, AlliesBuffContentDisplay);

        b.GetComponent<BuffButton>().Setup(buff);
        UnlockedPlayerBuffs.Add(b.GetComponent<BuffButton>());

    }

    #endregion

    #region UPDATE BUTTONS

    public void UpdateAllyInfo(int Id)
    {
        AllyButtons.Find((x) => x.GetBuffId() == Id).UpdateInfo();
    }

    public void UpdateAllyButtoninfo(int Id)
    {
        UnlockedAllyBuffs.Find((x) => x.GetBuffId() == Id).UpdateInfo();
    }

    public void UpdatePlayerButtoninfo(int Id)
    {
        UnlockedPlayerBuffs.Find((x) => x.GetBuffId() == Id).UpdateInfo();
    }

    public void UpdateMonsterButtoninfo(int Id)
    {
        UnlockedMonsterBuffs.Find((x) => x.GetBuffId() == Id).UpdateInfo();
    }

    #endregion

    #region DELETE BUTTONS

    public void DeleteAlly(int Id)
    {
        int ListIndex = AllyButtons.FindIndex((x) => x.GetBuffId() == Id);
        BuffButton b = AllyButtons[ListIndex];
        AllyButtons.RemoveAt(ListIndex);
        Destroy(b.gameObject);
    }

    public void DeletePlayerBuff(int Id)
    {
        int ListIndex = UnlockedPlayerBuffs.FindIndex((x) => x.GetBuffId() == Id);
        BuffButton b = UnlockedPlayerBuffs[ListIndex];
        UnlockedPlayerBuffs.RemoveAt(ListIndex);
        Destroy(b.gameObject);
    }

    public void DeleteAllyBuff(int Id)
    {
        int ListIndex = UnlockedAllyBuffs.FindIndex((x) => x.GetBuffId() == Id);
        BuffButton b = UnlockedAllyBuffs[ListIndex];
        UnlockedAllyBuffs.RemoveAt(ListIndex);
        Destroy(b.gameObject);
    }

    public void DeleteMonsterBuff(int Id)
    {
        int ListIndex = UnlockedMonsterBuffs.FindIndex((x) => x.GetBuffId() == Id);
        BuffButton b = UnlockedMonsterBuffs[ListIndex];
        UnlockedMonsterBuffs.RemoveAt(ListIndex);
        Destroy(b.gameObject);
    }

    public void CheckButtonInteraction() {
        foreach (BuffButton b in AllyButtons)
            b.CheckInteractable();

        foreach (BuffButton b in UnlockedAllyBuffs)
            b.CheckInteractable();

        foreach (BuffButton b in UnlockedPlayerBuffs)
            b.CheckInteractable();

        foreach (BuffButton b in UnlockedMonsterBuffs)
            b.CheckInteractable();
    }

    #endregion
}
