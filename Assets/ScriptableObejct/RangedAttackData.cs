using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="RangedAttackData", menuName ="RangedAttack/Data", order = 0)]
public class RangedAttackData : ScriptableObject
{
    [Header("Ranged Attack Data")] 
    public string tag;
    public int power;
    public float speed;
    public float duration;
    public float angle;
    public LayerMask target;
}
