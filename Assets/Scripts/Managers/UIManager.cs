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

    private void Awake()
    {
        Instance = this;
    }
}
