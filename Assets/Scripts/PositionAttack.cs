using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionAttack : MonoBehaviour
{
    private ProjectileManager projectileManager;
 
    private void Start()
    {
        projectileManager = ProjectileManager.instance;
    }

    public void CreateProjectile(Vector2 position, PositionAttackData data)
    {
        projectileManager.ActivePositionAttack(position, data);
    }

    public void CreateMultipleProjectile(Vector2 position, PositionAttackData data)
    {
        StartCoroutine(MultipleProjectile(position, data));
    }

    private IEnumerator MultipleProjectile(Vector2 position, PositionAttackData data)
    {
        projectileManager.ActivePositionAttack(position, data);
        int count = data.numberofPositionAttack;
        if (!data.bothSide)
        {
            for (int i = 1; i < count; i++)
            {
                yield return YieldCache.WaitForSeconds(data.delayTime);
                projectileManager.ActivePositionAttack(position + data.followUpDirection * i, data);
            }
        }
        else
        {
            for (int i = 1; i < count; i++)
            {
                yield return YieldCache.WaitForSeconds(data.delayTime);
                projectileManager.ActivePositionAttack(position + data.followUpDirection * i, data);
                projectileManager.ActivePositionAttack(position + -data.followUpDirection * i, data);
                count--;
            }
        }
        yield return null;
    }
}
