using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestAnimation : MonoBehaviour
{
    private Animator anim;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            TestStart();
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            StartAnimation();
        }
        
        if (Input.GetKeyDown(KeyCode.E))
        {
            StopAnimation();
        }
    }

    private void TestStart()
    {
        anim.SetTrigger("Groggy");
    }
    
    private void StopAnimation()
    {
        anim.speed = 0f;
    }

    private void StartAnimation()
    {
        anim.speed = 1f;
    }
}
