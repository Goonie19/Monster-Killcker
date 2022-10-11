using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class BuffData
{
    public BuffData(int id, int numberOfBuffs)
    {
        BuffId = id;
        NumberOfBuffs = numberOfBuffs;
    }

    public int BuffId;

    public int NumberOfBuffs;

    public float ActualPrice;

}
