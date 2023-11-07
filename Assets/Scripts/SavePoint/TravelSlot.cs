using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TravelSlot : MonoBehaviour
{
    private string name;
    [SerializeField] private TextMeshProUGUI textUI;

    public void SetSlot(string name)
    {
        this.name = name;
        textUI.text = name;
    }
    
    public void FindTravelLocation()
    {
        TravelManager.instance.FastTravel(name);
    }
}
