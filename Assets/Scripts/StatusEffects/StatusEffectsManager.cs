using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusEffectsManager : MonoBehaviour
{
    public PoisonStatusEffect poisonEffectPrefab;
    public BleedingStatusEffect bleedingEffectPrefab;

    private PoisonStatusEffect currentPoisonEffect;
    private BleedingStatusEffect currentBleedingEffect;

    // 플레이어나 적의 공격에 의해 호출되는 메서드
    public void ApplyPoisonEffect(int PoisonDamage)
    {
        if (currentPoisonEffect == null)
        {
            currentPoisonEffect = Instantiate(poisonEffectPrefab, transform);
            currentPoisonEffect.ApplyEffect(PoisonDamage);
        }
    }

    public void ApplyBleedingEffect(int BleedingDamage)
    {
        if (currentBleedingEffect == null)
        {
            currentBleedingEffect = Instantiate(bleedingEffectPrefab, transform);
            currentBleedingEffect.ApplyEffect(BleedingDamage);
        }
    }

    // 상태 이상 효과의 제거
    public void RemovePoisonEffect()
    {
        if (currentPoisonEffect != null)
        {
            Destroy(currentPoisonEffect.gameObject);
            currentPoisonEffect = null;
        }
    }

    public void RemoveBleedingEffect()
    {
        if (currentBleedingEffect != null)
        {
            Destroy(currentBleedingEffect.gameObject);
            currentBleedingEffect = null;
        }
    }
}
