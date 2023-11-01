using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileManager : MonoBehaviour
{
    public static ProjectileManager instance;
    private ObjectPool objectPool;

    private void Awake()
    {
        instance = this;
        objectPool = GetComponent<ObjectPool>();
    }

    public void CreateProjectile(Vector2 startPosition, Vector2 direction, RangedAttackData attackData) {
        GameObject obj = objectPool.SpawnFromPool(attackData.tag);
    
        obj.transform.position = startPosition;
        RangedAttackController attackController = obj.GetComponent<RangedAttackController>();
        attackController.InitializeAttack(direction, attackData);
    
        obj.SetActive(true);
    }

    public void InsertObjectPool(ObjectPool.Pool pool)
    {
        objectPool.InsertToPool(pool);
    }
    
    public void DeleteObjectPool(string tag)
    {
        objectPool.DeleteToPool(tag);
    }
}
