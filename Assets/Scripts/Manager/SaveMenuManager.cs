using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public struct Travel
{
    public string name;
    public int sortNumber;
    public Vector3 position;
}

public class SaveMenuManager : MonoBehaviour
{
    public static SaveMenuManager instance;
    
    [Header("메뉴")]
    public SaveMenu saveMenuUI;
    public SaveMenu levelUpUI;
    public SaveMenu fastTravelUI;
    
    [Header("빠른이동")]
    private List<Travel> travel;
    [SerializeField] private Transform slotParent;
    [SerializeField] private GameObject travelSlot;

    private void Awake()
    {
        instance = this;
        travel = new List<Travel>();
    }

    public void AddTravel(string name, int num, Vector3 position) 
    {
        Travel t = new Travel{name = name, sortNumber = num, position = position};
        travel.Add(t);
        SortList();
    }

    public void SortList()
    {
        travel = travel.OrderBy(t => t.sortNumber).ToList();
    }

    // public int FindTravel(string name)
    // {
    //     for (int i = 0; i < travel.Count; i++)
    //     {
    //         if (travel[i].name == name)
    //         {
    //             return i;
    //         }
    //     }
    //     return -1;
    // }

    // public void SetTravelList()
    // {
    //     foreach (Transform child in slotParent)
    //     {
    //         Destroy(child.gameObject);
    //     }
    //     
    //     for (int i = 0; i < travel.Count; i++)
    //     {
    //         GameObject newSlot = Instantiate(travelSlot, slotParent);
    //         newSlot.GetComponent<TravelSlot>().SetSlot(travel[i].name);
    //     }
    // }
    
    // public void FastTravel(string name)
    // {
    //     int num = FindTravel(name);
    //     if (num != -1)
    //     {
    //         GameManager.Instance.player.transform.position = travel[num].position;
    //         saveMenuUI.AllCancel();
    //     }
    // }
}
