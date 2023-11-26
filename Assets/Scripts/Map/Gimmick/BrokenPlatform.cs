using System;
using System.Collections;
using UnityEditor.Callbacks;
using UnityEngine;

public class BrokenPlatform : BaseGimmick
{
    private AudioSource audioSource;
    private SpriteRenderer spriteRenderer;
    public Collider2D platformCollider;
    private float delayTime = 1f;

    [SerializeField] private MapGimmickAction MGA;
    [SerializeField] private MapGimmickInteraction MGI;

    private Coroutine currentCoroutine;
    

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        platformCollider = GetComponent<Collider2D>();
        base.Start();
        MGA = mapGimmickAction;
        MGI = mapGimmickInteraction;
    }

    private void Update()
    {
        if (mapGimmickInteraction != null && mapGimmickAction != null)
        {
            Debug.Log("Ani");
            Collider2D[] colliders = Physics2D.OverlapBoxAll(transform.position, transform.localScale, 0f);
            foreach (Collider2D collider in colliders)
            {
                Debug.Log("Collided with11: " + collider.gameObject.name);
            }
            bool isCollision = mapGimmickInteraction.CollisionChecktoTagBased("Player");
            if (isCollision && currentCoroutine == null)
            {
                currentCoroutine = StartCoroutine(PerformCollisionAction());
            }
        }
    }

    private IEnumerator PerformCollisionAction()
    {
        yield return StartCoroutine(mapGimmickAction.ProcessDelay(1));
        mapGimmickAction.ToggleSpriteAndCollider(spriteRenderer, platformCollider, false);
        Debug.Log("11");
        mapGimmickAction.PlaySound(audioSource);
        yield return StartCoroutine(mapGimmickAction.ProcessDelay(3));
        mapGimmickAction.ToggleSpriteAndCollider(spriteRenderer, platformCollider, true);
        currentCoroutine = null;
    }
}