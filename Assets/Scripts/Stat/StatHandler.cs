using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class StatHandler : MonoBehaviour
{
   [SerializeField] public BaseStat baseStatSO;

   protected abstract void TakeDamage(int baseDamage);

   protected abstract void SetStat();
}
