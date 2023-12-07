using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FMapUI : MonoBehaviour
{
    public FMapUI instance;
    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
    }
}
