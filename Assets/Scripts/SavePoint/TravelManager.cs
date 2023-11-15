using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;


public class TravelManager : MonoBehaviour
{
    public static TravelManager instance;
    private bool isTravelReady = false;

    private void Awake()
    {
        instance = this;
    }

    public void CallTravelEvent()
    {
        isTravelReady = isTravelReady ? false : true;
    }
}
