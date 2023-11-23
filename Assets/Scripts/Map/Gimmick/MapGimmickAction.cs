using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGimmickAction : MonoBehaviour
{
    private bool isDeactivated = false;

    public void PlaySound(AudioSource audioSource)
    {
        audioSource.Play();
    }

    public void ToggleSpriteAndCollider(SpriteRenderer spriteRenderer, Collider2D collider2D, bool isActive)
    {
        spriteRenderer.enabled = isActive;
        collider2D.enabled = isActive;
    }

    public void ToggleCollider(Collider2D collider2D, bool isActive)
    {
        collider2D.enabled = isActive;
    }

    public IEnumerator ProcessDelay(int time)
    {
        yield return new WaitForSeconds(time);
    }
}
