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

    public void InstantiateAllyButton(AllyBuff buff)
    {
        if(buff.OneUseBuff)
        {
            GameObject b = Instantiate(AllyButtonOneUse, AlliesBuffContentDisplay);
            b.GetComponent<BuffButton>().Setup(buff);

        } else
        {
            GameObject b = Instantiate(AllyButton, AlliesBuffContentDisplay);
            b.GetComponent<BuffButton>().Setup(buff);
        }
    }

    public void InstantiateMonsterButton(MonsterBuff buff)
    {
        if(buff.OneUseBuff)
        {
            //GameObject b = Instantiate
        }
    }

    public void InstantiatePlayerBuffButton(PlayerBuff buff)
    {
        if (buff.OneUseBuff)
        {
            GameObject b = Instantiate(AllyButtonOneUse, AlliesBuffContentDisplay);
            b.GetComponent<BuffButton>().Setup(buff);

        }
        else
        {
            GameObject b = Instantiate(AllyButton, AlliesBuffContentDisplay);
            b.GetComponent<BuffButton>().Setup(buff);
        }

    }
}
