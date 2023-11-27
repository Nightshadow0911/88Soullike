using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeTrap : BaseGimmick
{
    private Coroutine currentCoroutine;
    private GameManager gameManager;
    void Start()
    {
        gameManager = GameManager.Instance;
        base.Start();
    }


    void Update()
    {
        bool isCollision = mapGimmickInteraction.CollisionChecktoTagBased("Player", transform.position);
        if (isCollision && currentCoroutine == null)
        {
            currentCoroutine = StartCoroutine(SpikeDamage());
        }
    }

    private IEnumerator SpikeDamage()
    {
        yield return StartCoroutine(mapGimmickAction.ProcessDelay(1));
        bool isCollision2 = mapGimmickInteraction.CollisionChecktoTagBased("Player", transform.position);
        if (isCollision2)
        {
            gameManager.playerStats.TakeDamage(10);
        }

        currentCoroutine = null;
    }
}
