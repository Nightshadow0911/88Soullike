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
    
    public void ResetPosition(Transform transform) //위치 초기화
    {
        transform.position = Vector2.zero; 
    }

    public void ResetPower(Rigidbody2D rb)
    {
        rb.velocity = Vector2.zero;
    }
    public void MoveInDirection(Vector2 direction, float force)
    {
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.AddForce(direction.normalized * force, ForceMode2D.Impulse);
        }
    }
    
    
}
