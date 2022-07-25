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
    public Transform BuffContentDisplay;
    public Transform AlliesContentDisplay;

    [Title("Text References")]
    public TextMeshProUGUI ExperienceDisplayText;
    public TextMeshProUGUI HeadsDisplayText;

    [Title("Button Prefabs")]
    public GameObject AllyButton;

    public GameObject AllyBuffButton;
    public GameObject AllyBuffButtonOneUse;

    public GameObject PlayerBuffButton;
    public GameObject PlayerBuffButtonOneUse;

    public GameObject MonsterBuffButton;
    public GameObject MonsterBuffButtonOneUse;

    [Title("Buffs Info Panel")]
    public AllyInfoPanel allyInfoPanel;
    public BuffInfoPanel buffInfoPanel;

    [Title("List of Buffs")]
    public List<BuffButton> AllyButtons;
    public List<BuffButton> UnlockedPlayerBuffs;
    public List<BuffButton> UnlockedAllyBuffs;
    public List<BuffButton> UnlockedMonsterBuffs;

    [Title("Monster Information UI Elements")]
    public TextMeshProUGUI MonsterMaximumHealth;
    public TextMeshProUGUI MonsterDropingHeads;
    public TextMeshProUGUI MonstersLifePercentageAddedToExp;

    [Title("Player Information UI Elements")]
    public TextMeshProUGUI PlayerBaseDamageText;
    public TextMeshProUGUI PlayerDamageMultiplierText;
    public TextMeshProUGUI PlayerTotalDamageText;

    [Title("Conversations UI Elements")]
    public GameObject DialoguePanel;
    public TextMeshProUGUI DialogueText;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        UpdateInfoPanels();
    }

    public void SpawnInfoPanel(Buff b)
    {

        buffInfoPanel.Setup(b);

        buffInfoPanel.gameObject.SetActive(true);
    }

    public void SpawnInfoPanel(AllyType a)
    {
        allyInfoPanel.Setup(a);

        allyInfoPanel.gameObject.SetActive(true);
    }

    public void UpdateInfoPanels()
    {
        MonsterMaximumHealth.text = MonsterManager.Instance.GetHealth().ToString();
        MonsterDropingHeads.text = MonsterManager.Instance.GetHeads().ToString();
        MonstersLifePercentageAddedToExp.text = (MonsterManager.Instance.HealthPercentageExp * 100).ToString() + "%";

        PlayerBaseDamageText.text = PlayerManager.Instance.BaseDamage.ToString();
        PlayerDamageMultiplierText.text = PlayerManager.Instance.DamageMultiplier.ToString();
        PlayerTotalDamageText.text = (PlayerManager.Instance.DamageMultiplier * PlayerManager.Instance.BaseDamage).ToString();

    }

    #region INSTANTIATE BUTTONS

    public void InstantiateAlly(AllyType ally)
    {
        GameObject b;

        b = Instantiate(AllyButton, AlliesContentDisplay);

        b.GetComponent<BuffButton>().Setup(ally);
        AllyButtons.Add(b.GetComponent<BuffButton>());
    }

    public void InstantiateAllyButton(AllyBuff buff)
    {
        GameObject b;

        if (buff.OneUseBuff)
            b = Instantiate(AllyBuffButtonOneUse, BuffContentDisplay);
        else
            b = Instantiate(AllyBuffButton, BuffContentDisplay);
        
        b.GetComponent<BuffButton>().Setup(buff);
        UnlockedAllyBuffs.Add(b.GetComponent<BuffButton>());
    }

    public void InstantiateMonsterButton(MonsterBuff buff)
    {
        GameObject b;

        if (buff.OneUseBuff)
            b = Instantiate(MonsterBuffButtonOneUse, BuffContentDisplay);
        else
            b = Instantiate(MonsterBuffButton, BuffContentDisplay);

        b.GetComponent<BuffButton>().Setup(buff);
        UnlockedMonsterBuffs.Add(b.GetComponent<BuffButton>());
    }

    public void InstantiatePlayerBuffButton(PlayerBuff buff)
    {
        GameObject b;

        if (buff.OneUseBuff)
            b = Instantiate(PlayerBuffButtonOneUse, BuffContentDisplay);
        else
            b = Instantiate(PlayerBuffButton, BuffContentDisplay);

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
