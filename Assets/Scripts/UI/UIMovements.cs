using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIMovements : MonoBehaviour
{
    public Animator Anim;

    public void Move()
    {
        Anim.SetTrigger("Deploy");
    }
}
