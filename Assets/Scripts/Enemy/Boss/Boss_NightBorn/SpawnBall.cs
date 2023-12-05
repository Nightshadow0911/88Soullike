using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnBall : PositionAttackController
{
    [SerializeField] private GameObject monster;

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        return;
    }

    protected override void DestroyProjectile()
    {
        Instantiate(monster, transform.position + (Vector3.down), Quaternion.identity);
        Boss_NightBorn.spiritNum++;
        base.DestroyProjectile();
    }
}
