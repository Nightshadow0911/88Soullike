using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="EnemyStats", menuName ="EnemyStats/DefaultEnemy",order = 0)]
public class EnemyStats : ScriptableObject
{
    [Header("Enemy Stats")]
    public int speed;
    public int damage;
    public float delay;
    public LayerMask target;
}