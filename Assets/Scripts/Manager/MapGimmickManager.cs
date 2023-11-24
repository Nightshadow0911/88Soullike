using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGimmickManager : MonoBehaviour
{
    private static MapGimmickManager instance;
    private MapGimmickAction mapGimmickAction;

    private void Awake()
    {
        mapGimmickAction = new MapGimmickAction();
    }

    public static MapGimmickManager Instance
    {
        get
        {
            if (instance == null)
            {
                Debug.LogError("MapGimmickManager instance is null!");
            }
            return instance;
        }
    }
    public MapGimmickAction GetMapGimmickActionInstance()
    {
        return mapGimmickAction;
    }
}
