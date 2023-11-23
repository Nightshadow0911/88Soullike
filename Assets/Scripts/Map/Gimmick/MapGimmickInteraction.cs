using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGimmickInteraction : MonoBehaviour
{
    public bool CollisionChecktoTagBased(string tag)
    {
        Collider2D[] colliders = Physics2D.OverlapBoxAll(transform.position, transform.localScale, 0f);
        foreach (Collider2D collider in colliders)
        {
            if (collider.CompareTag(tag))
            {
                return true;
            }
        }
        return false;
    }
}
