using DG.Tweening;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{

    public static UIManager Instance;

    [Title("Number Formats")]
    public List<string> NumberFormat;

    [Title("FadePanel")]
    public Image FadePanel;
    public float FadeTime = 2f;
    public UnityEvent OnFadeOut;

    [Title("Monster")]
    public MonsterBehaviour MonsterToClick;
    [Title("Boss")]
    public BossBehaviour BossToClick;

    public TextMeshProUGUI BossTimer;

    [Title("Boss Appearing Parameters")]
    public GameObject BossAppearButton;
    public GameObject BossAppearingPanel;
    public Image BossAppearingImage;
    public TextMeshProUGUI BossSpeakingText;

    [Title("Buff Display")]
    public Transform BuffContentDisplay;
    public Transform AlliesContentDisplay;

    [Title("Money References")]
    public Image ExperienceIcon;
    public Image HeadsIcon;
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

    [Title("Buffs Info Panel Hover")]
    public AllyInfoPanel allyInfoPanel;
    public BuffInfoPanel buffInfoPanel;
    public HoverPanel hoverPanel;

    [Title("List of Buffs")]
    public List<BuffButton> AllyButtons;
    public List<BuffButton> UnlockedPlayerBuffs;
    public List<BuffButton> UnlockedAllyBuffs;
    public List<BuffButton> UnlockedMonsterBuffs;

    [Title("Monster Information UI Elements")]
    public GameObject MonsterMaximumHealthObject;
    public TextMeshProUGUI MonsterMaximumHealthValue;
    public GameObject MonsterDropingHeadsObject;
    public TextMeshProUGUI MonsterDropingHeadsValue;
    public GameObject MonsterLifePercentageAddedToExpObject;
    public TextMeshProUGUI MonsterExpDrop;

    [Title("Boss Information UI Elements")]
    public GameObject BossMaxHealthObject;
    public TextMeshProUGUI BossMaxHealthValue;
    public GameObject BossLifeToGetExpObject;
    public TextMeshProUGUI BossLifeToGetExpValue;
    public GameObject BossDamageTakenObject;
    public TextMeshProUGUI BossDamageTakenValue;

    [Title("Player Information UI Elements")]
    public TextMeshProUGUI PlayerBaseDamageText;
    public TextMeshProUGUI AlliesDamageText;
    public TextMeshProUGUI PlayerTotalDamageText;

    [Title("Conversations UI Elements")]
    public GameObject DialoguePanel;
    public TextMeshProUGUI DialogueText;

    [Title("Health Info for monster")]
    public Image fillHealthImage;
    public TextMeshProUGUI CurrentHealthText;

    [Title("Pause Panel")]
    public List<Button> ButtonsToAssignSound;
    public Toggle StatsHoverToggle;
    public GameObject StatsHoverCheckmark;
    public Toggle AlliesHoverToggle;
    public GameObject AlliesHoverCheckmark;
    public Toggle WindowModeToggle;
    public GameObject WindowModeCheckmark;

    public TMP_Dropdown ResolutionDropdown;

    public Slider MasterVolumeSlider;
    public Slider MusicVolumeSlider;
    public Slider SFXVolumeSlider;
    


    private void Awake()
    {
        Instance = this;
    }

    void OnEnable()
    {
        GameManager.Instance.OnTenShopModeChanged.AddListener(CheckButtonInteraction);
        GameManager.Instance.OnTenShopModeChanged.AddListener(UpdateAllButtons);
    }

    void OnDisable()
    {
        GameManager.Instance.OnTenShopModeChanged.RemoveListener(CheckButtonInteraction);
        GameManager.Instance.OnTenShopModeChanged.RemoveListener(UpdateAllButtons);
    }

    private void Start()
    {
        MonsterManager.Instance.OnParametersInitialized.AddListener(UpdateInfoPanels);
        Sequence sq = DOTween.Sequence();

        sq.Append(FadePanel.DOFade(0f, FadeTime).SetEase(Ease.Linear));

        sq.Play();

        bool stats = GameManager.Instance.GetStatHovers();
        bool allies = GameManager.Instance.GetAlliesHovers();
        bool window = GameManager.Instance.GetWindowMode();
        
        StatsHoverToggle.isOn = stats;
        StatsHoverCheckmark.SetActive(stats);

        AlliesHoverToggle.isOn = allies;
        AlliesHoverCheckmark.SetActive(allies);

        WindowModeToggle.isOn = window;
        WindowModeCheckmark.SetActive(window);

        ResolutionDropdown.ClearOptions();

        List<string> resOptions = new List<string>();
        foreach(Vector2Int res in GameManager.Instance.Resolutions)
        {
            resOptions.Add((res.x + " x " + res.y).ToString());
        }

        foreach (Button b in ButtonsToAssignSound)
            b.onClick.AddListener(() => { AudioManager.Instance.PlayClickButtonSound(); });

        ResolutionDropdown.AddOptions(resOptions);
        ResolutionDropdown.value = GameManager.Instance.GetResolutionIndex();

        WindowModeToggle.onValueChanged.AddListener(GameManager.Instance.SetFullScreen);
        StatsHoverToggle.onValueChanged.AddListener(GameManager.Instance.ShowStatHovers);
        AlliesHoverToggle.onValueChanged.AddListener(GameManager.Instance.ShowAlliesHovers);
        ResolutionDropdown.onValueChanged.AddListener(GameManager.Instance.SetScreenResolution);

        sq.OnComplete(() => {
            FadePanel.raycastTarget = false;
            OnFadeOut?.Invoke();
        });
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

    public void ChangeInfoPanel()
    {
        if(BossManager.Instance.InBossFight)
        {
            BossMaxHealthObject.SetActive(true);
            BossLifeToGetExpObject.SetActive(true);
            BossDamageTakenObject.SetActive(true);
            MonsterDropingHeadsObject.SetActive(false);
            MonsterLifePercentageAddedToExpObject.SetActive(false);
            MonsterMaximumHealthObject.SetActive(false);
        } else
        {
            BossMaxHealthObject.SetActive(false);
            BossLifeToGetExpObject.SetActive(false);
            BossDamageTakenObject.SetActive(false);
            MonsterDropingHeadsObject.SetActive(true);
            MonsterLifePercentageAddedToExpObject.SetActive(true);
            MonsterMaximumHealthObject.SetActive(true);
        }
    }

    #region FADING METHODS

    public void FadeToMainMenu()
    {
        FadePanel.raycastTarget = true;
        AudioManager.Instance.StopMusic();
        Sequence sq = DOTween.Sequence();

        sq.Append(FadePanel.DOFade(1f, FadeTime).SetEase(Ease.Linear));

        sq.Play();

        sq.OnComplete(() => {
            GameManager.Instance.ChangeToMenuScene();
        });
    }

    #endregion

    #region INSTANTIATE BUTTONS

    public void InstantiateAlly(AllyType ally)
    {
        GameObject b;

        b = Instantiate(AllyButton, AlliesContentDisplay);
        AudioManager.Instance.PlayUnlockedSound();

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

        AudioManager.Instance.PlayUnlockedSound();

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

        AudioManager.Instance.PlayUnlockedSound();

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

        AudioManager.Instance.PlayUnlockedSound();
        b.GetComponent<BuffButton>().Setup(buff);
        UnlockedPlayerBuffs.Add(b.GetComponent<BuffButton>());

    }

    #endregion

    #region UPDATE BUTTONS

    public void UpdateAllButtons()
    {
        foreach(BuffButton b in AllyButtons)
            b.UpdateInfo();

        foreach (BuffButton b in UnlockedPlayerBuffs)
            b.UpdateInfo();

        foreach (BuffButton b in UnlockedAllyBuffs)
            b.UpdateInfo();

        foreach (BuffButton b in UnlockedMonsterBuffs)
            b.UpdateInfo();
    }

    public void UpdateAllyInfo(int Id, bool attack = false)
    {
        AllyButtons.Find((x) => x.GetBuffId() == Id)?.UpdateInfo(attack);
    }

    public void UpdateAllyButtoninfo(int Id)
    {
        UnlockedAllyBuffs.Find((x) => x.GetBuffId() == Id)?.UpdateInfo();
    }

    public void UpdatePlayerButtoninfo(int Id)
    {
        UnlockedPlayerBuffs.Find((x) => x.GetBuffId() == Id)?.UpdateInfo();
    }

    public void UpdateMonsterButtoninfo(int Id)
    {
        UnlockedMonsterBuffs.Find((x) => x.GetBuffId() == Id)?.UpdateInfo();
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

    #region INFO UPDATING

    public void UpdateInfoPanels()
    {
        if(!BossManager.Instance.InBossFight)
        {
            MonsterMaximumHealthValue.text = SimplifyNumber(MonsterManager.Instance.GetHealth());
            MonsterDropingHeadsValue.text = SimplifyNumber(MonsterManager.Instance.GetHeads());
            MonsterExpDrop.text = SimplifyNumber(MonsterManager.Instance.GetExperience());
        } else
        {
            BossMaxHealthValue.text = SimplifyNumber(BossManager.Instance.GetMaxHealth());
            if (BossManager.Instance.GetGoalCompleted() != null)
                BossLifeToGetExpValue.text = SimplifyNumber(BossManager.Instance.GetGoalCompleted().DamageGoal);
            else
                BossLifeToGetExpValue.text = "none";
            BossDamageTakenValue.text = SimplifyNumber(BossToClick.GetDamageTaken());
        }

        PlayerBaseDamageText.text = SimplifyNumber(PlayerManager.Instance.BaseDamage);
        //PlayerDamageMultiplierText.text = PlayerManager.Instance.DamageMultiplier.ToString();
        //PlayerTotalDamageText.text = PlayerManager.Instance.BaseDamage.ToString();

    }

    public void UpdateHealthBar(float actualHealth)
    {
        if(BossManager.Instance.InBossFight)
        {
            fillHealthImage.fillAmount = actualHealth / BossManager.Instance.GetMaxHealth();
            CurrentHealthText.text = string.Format("{0:0.00}", actualHealth) + "/" + BossManager.Instance.GetMaxHealth().ToString();
        } else {
            fillHealthImage.fillAmount = actualHealth / MonsterManager.Instance.GetHealth();
            CurrentHealthText.text = string.Format("{0:0.00}", actualHealth) + "/" + MonsterManager.Instance.GetHealth().ToString();
        }
    }

    #endregion

    public string SimplifyNumber(float number)
    {
        string numberString;
        if (number % 1 == 0)
            numberString = number.ToString();
        else
            numberString = string.Format("{0:0.00}", number);

        int i = 0;

        while(i < NumberFormat.Count && number > 1000)
        {
            number /= 1000f;
            numberString = string.Format("{0:0.000}", number) + NumberFormat[i];

            i++;
        }

        return numberString;
    }

}
