using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllyManager : MonoBehaviour
{

    public static AllyManager Instance;

    public List<AllyType> allies;

    private void Awake()
    {
        Instance = this;
    }

    public void BuyAlly(int AllyId)
    {
        allies.Find((x) => x.AllyId == AllyId).NumberOfAllies++;
    }
}
