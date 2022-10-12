using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class BuffData
{
    public BuffData(int id, int numberOfBuffs, float actualPrice)
    {
        BuffId = id;
        NumberOfBuffs = numberOfBuffs;
        ActualPrice = actualPrice;
    }

    public int BuffId;

    public int NumberOfBuffs;

    public float ActualPrice;

}
