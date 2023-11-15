using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapUI : MonoBehaviour
{
    [SerializeField]private TravelPoint[] trs = new TravelPoint[4];
    [SerializeField]private Transform trPointHolder;

    private void OnEnable()
    {
        int trCount = trPointHolder.childCount;
        for (int i = 0; i < trCount; i++)
        {
            trs[i] = trPointHolder.GetChild(i).GetComponent<TravelPoint>();
            trs[i].CloseText();
        }
    }
}
