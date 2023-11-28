using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootObject : RangedAttackController
{
    protected override void Update()
    {
       base.Update();
        currentDuration += Time.deltaTime;

        if (currentDuration > attackData.duration) {
            DestroyProjectile(transform.position);
        }
        rigid.velocity = direction * attackData.speed;
    }
}
