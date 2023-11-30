using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "NewBaseStat", menuName = "Stats/BaseStat")]
public class BaseStat : ScriptableObject
{
   public int characterMaxHP;
   public int characterDamage;
   public int characterDefense;
   public int propertyDamage;
   public int propertyDefense;
   
}
