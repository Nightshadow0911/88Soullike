using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewBaseStat", menuName = "Stats/BaseStat", order = 0)]
public class BaseStat : ScriptableObject
{
   [Header("Base Stats")]
   public int characterMaxHP;
   public int characterDamage;
   public int characterDefense;
   public int propertyDamage;
   public int propertyDefense;
}
