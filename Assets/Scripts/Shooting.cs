using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooting : MonoBehaviour
{
    [SerializeField] private RangedAttackData rangedData;
    [SerializeField] private Transform attackPosition;
    
    public void ShootArrow(int numberOfArrows, Vector2 dir)
    {
        float dataAngle = rangedData.angle;
        // 각도 아래로 내림
        float minAngle = -(numberOfArrows / 2f) * dataAngle + 0.5f * dataAngle;
        for (int i = 0; i < numberOfArrows; i++)
        {
            float angle = minAngle + dataAngle * i;
            ProjectileManager.instance.CreateProjectile(
                attackPosition.position,
                RotateVector2(dir, angle),
                rangedData
            );
        }
    }
    
    private Vector2 RotateVector2(Vector2 v, float degree) {
        return Quaternion.Euler(0, 0, degree) * v;
    }
}
