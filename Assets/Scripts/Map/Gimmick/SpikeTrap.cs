using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeTrap : BaseGimmick
{
    private Coroutine currentCoroutine;
    private GameManager gameManager;
    private Animator animator;
    public PlayerStatusHandler playerHandlerspikeTrap;
    [SerializeField]
    private AudioSource spikeTrapAudioSource;
    protected override void Start()
    {
        gameManager = GameManager.instance;
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
            spikeTrapAudioSource.Play();
            mapGimmickInteraction.CollisionCheckToPlayerTakeDamage("Player",transform.position, 10 );
            
        }
        yield return StartCoroutine(mapGimmickAction.ProcessDelay(1));
        currentCoroutine = null;
    }
    private void PlayAnimation()
    {
        animator.SetTrigger("Spike");
    }
}
