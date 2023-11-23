using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGimmickAction : MonoBehaviour
{

    public void PlaySound(AudioSource audioSource)  //사운드 픞레이
    {
        audioSource.Play();
    }

    public void ToggleSpriteAndCollider(SpriteRenderer spriteRenderer, Collider2D collider2D, bool isActive) // 스프라이트 및 콜라이더 활성화/비활성화
    {
        spriteRenderer.enabled = isActive;
        collider2D.enabled = isActive;
    }

    public void ToggleCollider(Collider2D collider2D, bool isActive) // 콜라이더만 활성화/비활성화
    {
        collider2D.enabled = isActive;
    }

    public IEnumerator ProcessDelay(int time) //딜레이 주기
    {
        yield return new WaitForSeconds(time);
    }
}
