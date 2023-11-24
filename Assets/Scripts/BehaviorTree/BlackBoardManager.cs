using System;
using System.Collections;
using System.Collections.Generic;
using BehaviorTree;
using UnityEngine;

public class BlackBoardManager : MonoBehaviour
{
    public static BlackBoardManager instance;
    private Dictionary<string, object> dataContext = new Dictionary<string, object>();

    private void Awake()
    {
        instance = this;
    }

    public void SetData(string key, object value)
    {
        dataContext[key] = value;
    }

    public object GetData(string key)
    {
        object value = null;
        if (dataContext.TryGetValue(key, out value))
            return value;
        
        return value;
    }

    public bool ClearDate(string key)
    {
        if (dataContext.ContainsKey(key))
        {
            dataContext.Remove(key);
            return true;
        }
        return false;
    }
}
