using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BleedingStatusEffect : MonoBehaviour
{
    public int damagePerTick = 100; // 출혈의 초당 피해량
    public float duration = 1f; // 출혈 상태이상 지속 시간
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
            ApplyBleedingEffect();
        }
    }

    private void ApplyBleedingEffect()
    {
        Debug.Log("22");
    }

    private void RemoveEffect()
    {
        Destroy(gameObject);
    }
}
