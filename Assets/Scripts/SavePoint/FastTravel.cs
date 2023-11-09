using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public struct Travel
{
    public string name;
    public int sortNumber;
    [HideInInspector]
    public Vector3 position;
}

public class FastTravel : MonoBehaviour
{
    private List<Travel> travels;
    [SerializeField] private Transform slotParent;
    [SerializeField] private GameObject travelSlot;

    private void Awake()
    {
        travels = new List<Travel>();
    }
    
    public void InputTravel(Travel travel)
    {
        if (FindTravel(name) != -1)
            return;
        travels.Add(travel);
        travels = travels.OrderBy(t => t.sortNumber).ToList();
        SetTravelList();
    }

    public int FindTravel(string name)
    {
        for (int i = 0; i < travels.Count; i++)
        {
            if (travels[i].name == name)
            {
                return i;
            }
        }
        return -1;
    }

    public void SetTravelList()
    {
        foreach (Transform slot in slotParent)
        {
            Destroy(slot.gameObject);
        }
        
        for (int i = 0; i < travels.Count; i++)
        {
            GameObject newSlot = Instantiate(travelSlot, slotParent);
            newSlot.GetComponent<TravelSlot>().SetSlot(travels[i]);
        }
    }
}
