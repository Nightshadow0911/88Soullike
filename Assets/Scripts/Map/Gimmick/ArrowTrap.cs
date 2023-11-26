using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowTrap : BaseGimmick
{
    public GameObject trapArrowPrefab;
    public Transform arrowSpawnPoint;
    private Coroutine currentCoroutine;

    private void Start()
    {

    }

    private void Update()
    {

        bool isCollision = mapGimmickInteraction.CollisionChecktoTagBased("Player");
        Debug.Log("11" + isCollision);
        if (isCollision && currentCoroutine == null)
        {
            Debug.Log("22");
            // currentCoroutine = StartCoroutine(ArrowTrapAction());
        }
    }

    // private IEnumerator ArrowTrapAction()
    // {
    //     GameObject trapArrow = Instantiate(trapArrowPrefab, arrowSpawnPoint.position, arrowSpawnPoint.rotation);
    //     mapGimmickAction.ToggleSpriteAndCollider(trapArrow.GetComponent<SpriteRenderer>(), trapArrow.GetComponent<Collider2D>(), true);
    //     mapGimmickAction.MoveInDirection(arrowSpawnPoint.right, 10.0f);
    //     yield return StartCoroutine(mapGimmickAction.ProcessDelay(5));
    //     currentCoroutine = null;
    // }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("22");
        }
    }
}
