using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIDamagePool : MonoBehaviour
{

    public static UIDamagePool instance;

    public List<UIDamage> UIDmgs;

    public float Speed = 50f;

    public GameObject DmgTextPrefab;

    void Awake()
    {
        instance = this;
    }

    public void ShowDamage(int Dmg)
    {
        UIDamage dmgText = UIDmgs.Find(x => x.gameObject.activeInHierarchy == false);

        if (dmgText == null)
        {
            dmgText = Instantiate(DmgTextPrefab, transform).GetComponent<UIDamage>();

            UIDmgs.Add(dmgText);
        }

        dmgText.ShowText(Dmg.ToString(), Speed);

        dmgText.gameObject.SetActive(true);
    }
}
