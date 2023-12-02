using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class StatHandler : MonoBehaviour
{
   [SerializeField] protected BaseStat baseStatSO;

   protected virtual void Awake()
   {
      SetStat();
   }

   public abstract void TakeDamage(int baseDamage);

   protected abstract void SetStat();
}
