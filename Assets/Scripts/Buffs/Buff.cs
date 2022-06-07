using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


public abstract class Buff : ScriptableObject
{

    public string BuffName;
    [TextArea(3,5)]
    public string BuffDescription;

    public int HeadsToUnlock;

    public bool OneUseBuff;
    public bool Unlocked;
    public bool Acquired;

    [HideIf("OneUseBuff")]
    public int NumberOfBuffs;

    public abstract void ApplyBuff();

}
