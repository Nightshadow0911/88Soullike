using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Boss2 : MonoBehaviour
{
    [SerializeField] protected EnemyStats stats;
    [SerializeField] protected Transform targetPosition;
    [SerializeField] protected LayerMask targetLayer;
    [SerializeField] protected GameObject dangerSign;

    protected float currentTime = 0f;
    protected bool isAttackReady = true;
}
