using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Travel
{
    public string name;
    public int travelNumber;
    public Vector3 position;
}

public class TravelManager : MonoBehaviour
{
    public TravelManager instance;
    private List<Travel> travel;

    private void Awake()
    {
        instance = this;
    }

    public void AddTravel(string name, int num, Vector3 position) 
    {
        if (travel.Contains(item => item.name.Eq))
    }
}
