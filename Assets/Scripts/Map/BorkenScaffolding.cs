using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BorkenScaffolding : MonoBehaviour
{
    public AudioClip breakSound;
    private AudioSource audioSource;
    private float delayInSeconds = 1.0f;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = breakSound;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            StartCoroutine(DEActiveDelay(delayInSeconds));
            PlaySound();
            StartCoroutine(ActiveDelay(delayInSeconds));
        }
    }
    private IEnumerator DEActiveDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        gameObject.SetActive(false);
    }
    private IEnumerator ActiveDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        gameObject.SetActive(true);
    }
    void PlaySound()
    {
        audioSource.Play();
    }
}
