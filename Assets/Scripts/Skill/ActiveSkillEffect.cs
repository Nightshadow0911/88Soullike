using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Scriptable", menuName = "Scriptable/SkillEft/ActiveEft")]
public class ActiveSkillEffect : SkillEffect
{
    [SerializeField] private bool type; // false 근거리, true 원거리
    public override bool ExcuteRole(int power, SkillType type) // power를 기반으로 피해를 줌
    {
        // power만큼 피해를 가진다.
        // type에 따라 속성피해를 입힌다.
        return true;
    }
}
