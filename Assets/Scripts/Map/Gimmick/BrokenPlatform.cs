using System;
using System.Collections;
using UnityEditor.Callbacks;
using UnityEngine;

public class BrokenPlatform : BaseGimmick
{
    private AudioSource audioSource;
    private SpriteRenderer spriteRenderer;
    public Collider2D platformCollider;

    private Coroutine currentCoroutine;
    

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        platformCollider = GetComponent<Collider2D>();
        base.Start();
    }

    private void Update()
    {
            bool isCollision = mapGimmickInteraction.CollisionChecktoTagBased("Player", transform.position);
            //Debug.Log("Box Position: " + transform.position);
            //Debug.Log("Box Size: " + transform.localScale);
            if (isCollision && currentCoroutine == null)
            {
                currentCoroutine = StartCoroutine(PerformCollisionAction());
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