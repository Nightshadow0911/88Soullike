using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Stat", menuName = "Scriptable/Stat")]
public class StatScriptableObject : ScriptableObject
{
    public int maxHP;
    public int attackDamage;
    public int defense;
    public int moveSpeed;
    public int propertyDamage;
    public int propertyDefense;
}

