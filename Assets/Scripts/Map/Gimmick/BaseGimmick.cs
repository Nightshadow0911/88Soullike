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
        mapGimmickInteraction = FindAnyObjectByType<MapGimmickInteraction>().GetComponent<MapGimmickInteraction>();
    }
}
