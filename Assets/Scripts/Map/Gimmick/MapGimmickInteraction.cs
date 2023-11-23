using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGimmickInteraction : MonoBehaviour
{
    public bool CollisionChecktoTagBased(string tag) //체크를 위해 bool로 설정
    {
        Collider2D[] colliders = Physics2D.OverlapBoxAll(transform.position, transform.localScale, 0f); //콜라이더 가져옴
        foreach (Collider2D collider in colliders)
        {
            if (collider.CompareTag(tag)) // 태그 확인
            {
                return true; //bool로 설정해놨기때문에 true반환
            }
        }
        return false;
    }
}
