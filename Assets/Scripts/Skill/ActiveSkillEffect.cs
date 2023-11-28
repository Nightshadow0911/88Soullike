using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

[CreateAssetMenu(fileName = "Scriptable", menuName = "Scriptable/SkillEft/ActiveEft")]
public class ActiveSkillEffect : SkillEffect
{
    [SerializeField] private bool type; // false 근거리, true 원거리
    public override bool ExcuteRole(int power, SkillType type) // power를 기반으로 피해를 줌
    {
        // power만큼 피해를 가진다.
        // type에 따라 속성피해를 입힌다.
        if (this.type)
        {
            // 앞으로 때리기
            // instantiate 오브젝트 생성
            // 오브젝트 앞으로 이동하게 하고
            // 한번 실행후 파괴
        }
        else
        {
            // 앞으로 날아가기
            // instantiate 오브젝트 생성
            // 오브젝트 앞으로 이동하게 하고
            // 디스트로이 데미지 주면서 파괴되기, 시간으로 파괴
        }


        return true;
    }
    void add()
    {

    }

    IEnumerator ShootArrowInArc()
    {
        yield return null;
    }

    IEnumerator ShootStraightArrow()
    {
        yield return null;
    }
}
