using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatFunctioParent : MonoBehaviour
{
   public bool TakeDamage(int damage, int currentHP)
   {
      currentHP -= damage;
      if (currentHP <= 0)
      {
         return true;
      }
      return false;
   }
}

