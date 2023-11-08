using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoisonStatusEffect : MonoBehaviour
{
    public int damagePerTick = 100; // 독의 초당 피해량
    public float duration = 10f; // 독 상태이상 지속 시간
    private float timeElapsed;
    private int accumulation; // 상태 이상의 축적치

    private void Update()
    {
        timeElapsed += Time.deltaTime;

        if (timeElapsed >= duration)
        {
            RemoveEffect();
        }
    }

    public void ApplyEffect(int PropetyDamage)
    {
        accumulation += damagePerTick;

        if (accumulation >= 100) // 축적치가 100 이상이면 상태이상 발동
        {
            ApplyPoisonEffect();
        }
    }

    private void ApplyPoisonEffect()
    {
        Debug.Log("11");
    }

    private void RemoveEffect()
    {
        
        Destroy(gameObject);
    }
}
