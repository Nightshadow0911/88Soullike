using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    private void Update()
    {
        Debug.DrawRay(transform.position, Vector3.left * 2f, Color.red);
    }
}
