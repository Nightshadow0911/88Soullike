using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeTrap : BaseGimmick
{
    private Coroutine currentCoroutine;
    private GameManager gameManager;
    private Animator animator;
    protected override void Start()
    {
        gameManager = GameManager.Instance;
        animator = GetComponent<Animator>();
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
        bool isCollision2 = mapGimmickInteraction.CollisionChecktoTagBased("Player", transform.position);
        if (isCollision2)
        {
            PlayAnimation();
            yield return StartCoroutine(mapGimmickAction.ProcessDelay(1));
            bool isCollision3 = mapGimmickInteraction.CollisionChecktoTagBased("Player", transform.position);
            if (isCollision3)
            {
                gameManager.playerStats.TakeDamage(10);
            }
            
        }
        yield return StartCoroutine(mapGimmickAction.ProcessDelay(1));
        currentCoroutine = null;
    }
    private void PlayAnimation()
    {
        animator.SetTrigger("Spike");
    }
}
