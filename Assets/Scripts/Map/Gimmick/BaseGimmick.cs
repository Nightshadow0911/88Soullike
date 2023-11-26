using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseGimmick : MonoBehaviour
{
    protected MapGimmickAction mapGimmickAction;
    protected MapGimmickInteraction mapGimmickInteraction;

    protected virtual void Start()
    {
        mapGimmickAction = FindAnyObjectByType<MapGimmickAction>().GetComponent<MapGimmickAction>();
        Debug.Log("11 :" +mapGimmickAction);
        mapGimmickInteraction = FindAnyObjectByType<MapGimmickInteraction>().GetComponent<MapGimmickInteraction>();
        Debug.Log("11 :" +mapGimmickInteraction);
    }
}
