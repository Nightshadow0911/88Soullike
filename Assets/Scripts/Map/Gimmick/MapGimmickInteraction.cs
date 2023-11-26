using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGimmickInteraction : MonoBehaviour
{
    public bool CollisionChecktoTagBased(string tag, Vector2 position) //체크를 위해 bool로 설정
    {
        Collider2D[] colliders = Physics2D.OverlapBoxAll(position, transform.localScale, 0f); //콜라이더 가져옴
        foreach (Collider2D collider in colliders)
        {
            Debug.Log("Collided with: " + collider.gameObject.name);

            if (collider.CompareTag(tag)) // 태그 확인
            {
                Debug.Log("Collision detected with tag: " + tag);
                return true; //bool로 설정해놨기때문에 true반환
            }
        }

        return false;
    }
};
