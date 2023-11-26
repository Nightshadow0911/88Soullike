using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KMKTest : MonoBehaviour
{
    public float distance;
    private void Update()
    {
        Debug.DrawRay(transform.position, Vector3.left * distance, Color.red);

        if (Input.GetKeyDown(KeyCode.Z))
        {
           
        }
    }
}
