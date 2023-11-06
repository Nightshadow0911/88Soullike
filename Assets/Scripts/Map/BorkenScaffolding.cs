using System;
using System.Collections;
using UnityEngine;

public class BrokenScaffolding : MonoBehaviour
{
    public AudioClip breakSound;
    private AudioSource audioSource;

    private SpriteRenderer spriteRenderer;
    private Collider2D collider2D;

    private bool isDeactivated = false;
    private float delayInSeconds = 1.0f;
    private Coroutine currentCoroutine;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = breakSound;

        spriteRenderer = GetComponent<SpriteRenderer>();
        collider2D = GetComponent<Collider2D>();
    }

    private void Update()
    {
        Collider2D[] colliders = Physics2D.OverlapBoxAll(transform.position, transform.localScale, 0f);
        foreach (Collider2D collider in colliders)
        {
            if (collider.CompareTag("Player") && !isDeactivated)
            {
                if (currentCoroutine != null)
                {
                    StopCoroutine(currentCoroutine);
                }
                currentCoroutine = StartCoroutine(ProcessCollision());
                break;
            }
        }
    }

    private IEnumerator ProcessCollision()
    {
        isDeactivated = true;
        yield return new WaitForSeconds(1.0f);
        ToggleSpriteAndCollider(false);
        PlaySound();
        yield return new WaitForSeconds(3.0f);
        ToggleSpriteAndCollider(true);
        isDeactivated = false;
    }


    private void PlaySound()
    {
        audioSource.Play();
    }

    private void ToggleSpriteAndCollider(bool isActive)
    {
        spriteRenderer.enabled = isActive;
        collider2D.enabled = isActive;
    }


}