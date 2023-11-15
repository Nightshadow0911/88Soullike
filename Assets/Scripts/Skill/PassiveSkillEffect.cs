using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Scriptable", menuName = "Scriptable/SkillEft/PassiveEft")]
public class PassiveSkillEffect : SkillEffect
{
    // 패시브로 올릴 플레이어 스탯
    [SerializeField] private string selectStat;
    // 다른 특수효과도 괜찮음 like 속성 저항
    public override bool ExcuteRole(int power, SkillType type)
    {
        for (int i = 0; i < power; i++) // 플레이어 스탯을 한번에 증가시키는 메서드가 없어서 이렇게 구현
        {
            GameManager.Instance.playerStats.TryLevelUp(selectStat);

        }

        return true;
    }
}
