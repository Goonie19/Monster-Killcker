using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


public abstract class Buff : ScriptableObject
{
    public int Id;

    public Sprite Icon;

    public string BuffName;
    [TextArea(3,5)]
    public string BuffDescription;

    
    public int HeadsToUnlock;

    public bool OneUseBuff;

    [ShowIf("OneUseBuff")]
    public bool Acquired;

    [HideIf("OneUseBuff")]
    public int NumberOfBuffs;

    protected bool _unlocked = false;

    [ContextMenu("Lock")]
    public void SetUnlockedToFalse()
    {
        _unlocked = false;
    }

    public abstract void Unlock();

    public abstract void ApplyBuff();

}
