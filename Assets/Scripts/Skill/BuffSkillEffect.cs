using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Scriptable", menuName = "Scriptable/SkillEft/BuffEft")]
public class BuffSkillEffect : SkillEffect
{
    //버프로 올릴 플레이어 스탯, 패시브와 달리 최종 공격력, 모든 피해 감소 등 극적인 효과를 준다.
    [SerializeField] private string selectStat;
    public override bool ExcuteRole(int power, SkillType type)
    {
        switch (selectStat)
        {
            case "Attack":
                //GameManager.Instance.playerStats.NormalAttackDamage += power;
                break;
            case "Defense":
                //GameManager.Instance.playerStats.CharacterDefense += power;
                break;
        }


        //power에 따라 
        return true;
    }
}
