using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowTrap : BaseGimmick
{
    public GameObject trapArrowPrefab;
    public Transform arrowSpawnPoint;
    private Coroutine currentCoroutine;

    protected override void Start()
    {
        base.Start();
    }

    private void Update()
    {

        bool isCollision = mapGimmickInteraction.CollisionChecktoTagBased("Player", transform.position);
        Debug.Log("check :" + isCollision);
        if (isCollision && currentCoroutine == null)
        {
            Debug.Log("22");
             currentCoroutine = StartCoroutine(ArrowTrapAction());
        }
    }

    private IEnumerator ArrowTrapAction()
    {
        GameObject trapArrow = Instantiate(trapArrowPrefab, arrowSpawnPoint.position, arrowSpawnPoint.rotation);
        yield return StartCoroutine(mapGimmickAction.ProcessDelay(5));
        currentCoroutine = null;
    }
    
}
