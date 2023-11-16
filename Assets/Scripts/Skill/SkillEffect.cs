using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SkillEffect : ScriptableObject
{
    public abstract bool ExcuteRole(int power, SkillType type); // 스킬별 다른 효과
}
