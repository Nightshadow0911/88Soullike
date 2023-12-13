using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FTravelUI : MonoBehaviour
{
    public static FTravelUI instance;

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
