using System;
using System.Collections;
using UnityEngine;

public class BrokenPlatform : MonoBehaviour
{
    private AudioSource audioSource;
    private SpriteRenderer spriteRenderer;
    public Collider2D platformCollider;

    private bool isDeactivated = false;
    private Coroutine currentCoroutine;
    private MapGimmickAction mapGimmickAction;
    private MapGimmickInteraction mapGimmickInteraction;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        platformCollider = GetComponent<Collider2D>();
        mapGimmickAction = GetComponent<MapGimmickAction>();
        mapGimmickInteraction = GetComponent<MapGimmickInteraction>();
    }

    private void Update()
    {
        bool isCollision = mapGimmickInteraction.CollisionChecktoTagBased("Player");
        if (isCollision && currentCoroutine == null)
        { 
            currentCoroutine = StartCoroutine(PerformCollisionAction(audioSource, spriteRenderer, platformCollider));
        }
    }

    private IEnumerator PerformCollisionAction(AudioSource audioSource, SpriteRenderer spriteRenderer, Collider2D collider2D)
    {
        yield return StartCoroutine(mapGimmickAction.ProcessDelay(1));
        mapGimmickAction.ToggleSpriteAndCollider(spriteRenderer, collider2D, false); 
        mapGimmickAction.PlaySound(audioSource);
        yield return StartCoroutine(mapGimmickAction.ProcessDelay(3));
        mapGimmickAction.ToggleSpriteAndCollider(spriteRenderer, collider2D, true);
        currentCoroutine = null;
    }
}