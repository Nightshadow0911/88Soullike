using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MonsterStat", menuName = "Monsters/")]
public class MonsterStat : ScriptableObject
{
    public int monsterHP;
    public int damage;
}