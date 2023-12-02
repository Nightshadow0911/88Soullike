using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimationController : MonoBehaviour
{
    private Animator anim;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    public void AnimationTrigger(string name)
    {
        anim.SetTrigger(name);
    }

    public void AnimationBool(string name, bool value)
    {
        anim.SetBool(name, value);
    }
}
