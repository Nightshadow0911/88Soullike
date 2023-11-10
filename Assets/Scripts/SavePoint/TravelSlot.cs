using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TravelSlot : MonoBehaviour
{
    private Travel travel;
    [SerializeField] private TextMeshProUGUI textUI;

    public void SetSlot(Travel travel)
    {
        this.travel = travel;
        textUI.text = travel.name;
    }
    
    public void FastTravel()
    {
        SaveMenuManager.instance.AllDeActiveMenu();
        GameManager.Instance.player.transform.position = travel.position;
    }
}
