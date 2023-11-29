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
    
    public void MoveInDirection(Transform transform, Vector2 direction, float force) //방향으로 이동
    {
        Rigidbody2D rb = transform.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.AddForce(direction.normalized * force, ForceMode2D.Impulse);
        }
    }

    public void MoveTransform(Transform transform, Vector2 direction, float force)
    {
        Vector2 movement = direction.normalized * force;
        transform.Translate(movement*Time.deltaTime);
    }

    public void ToggleObjectSetActive(GameObject target, bool isActive)
    {
        if (target != null)
        {
            target.SetActive(isActive);
        }
    }
    
    public GameObject SpawnObject(GameObject prefab, Vector3 position, Quaternion rotation)
    {
        GameObject newObject = Instantiate(prefab, position, rotation);
        return newObject;
    }

}
