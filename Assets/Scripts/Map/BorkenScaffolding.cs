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
        // 1. 1초 대기
        yield return new WaitForSeconds(1.0f);

        // 2. ToggleSpriteAndCollider(false);
        ToggleSpriteAndCollider(false);

        // 3. PlaySound();
        PlaySound();

        // 4. 1초 후 스프라이트와 콜라이더2D 다시 활성화
        yield return new WaitForSeconds(3.0f);
        ToggleSpriteAndCollider(true);
        isDeactivated = false;
    }


    private void PlaySound()
    {
        // 사운드 재생
        audioSource.Play();
    }

    private void ToggleSpriteAndCollider(bool isActive)
    {
        spriteRenderer.enabled = isActive;
        collider2D.enabled = isActive;
    }


}