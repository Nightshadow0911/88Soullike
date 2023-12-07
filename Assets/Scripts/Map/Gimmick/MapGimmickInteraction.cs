using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MapGimmickInteraction : MonoBehaviour
{
    public bool CollisionChecktoTagBased(string tag, Vector2 position) //체크를 위해 bool로 설정
    {
        Collider2D[] colliders = Physics2D.OverlapBoxAll(position, transform.localScale, 0f); //콜라이더 가져옴
        foreach (Collider2D collider in colliders)
        {

            if (collider.CompareTag(tag)) // 태그 확인
            {
                return true; //bool로 설정해놨기때문에 true반환
            }
        }

        return false;
    }

    public void CollisionCheckToPlayerTakeDamage(string tag, Vector2 position, int damage)
    {
        PlayerStatusHandler playerHandler = new PlayerStatusHandler();
        Collider2D[] colliders = Physics2D.OverlapBoxAll(position, transform.localScale, 0f);
        foreach (Collider2D collider in colliders)
        {
            if (collider.CompareTag(tag))
            {
                playerHandler = collider.GetComponent<PlayerStatusHandler>();
                playerHandler.TakeTrueDamage(damage);
                
            }
        }
        
    }

    public bool CollisionChecktoLayerBased(string layerName, Vector2 position)
    {
        Collider2D[] colliders = Physics2D.OverlapBoxAll(position, transform.localScale, 0f);
        foreach (Collider2D collider in colliders)
        {
            if (collider.gameObject.layer == LayerMask.NameToLayer(layerName))
            {
                return true;
            }
        }
        return false;
    }
};
