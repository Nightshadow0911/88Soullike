using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.VirtualTexturing;
using static UnityEngine.RuleTile.TilingRuleOutput;

[CreateAssetMenu(fileName = "Scriptable", menuName = "Scriptable/SkillEft/ActiveEft")]
public class ActiveSkillEffect : SkillEffect
{
    [SerializeField] private bool type; // false 근거리, true 원거리
    public GameObject skillPrefab; // false 근거리, true 원거리
    public Vector3 skillPosition;
    public override bool ExcuteRole(int power, SkillType type) // power를 기반으로 피해를 줌
    {
        Debug.Log("스킬이펙트");
        // power만큼 피해를 가진다.
        // type에 따라 r근거리/ 원거리 나뉨
        GameObject go = Instantiate(skillPrefab, skillPosition, Quaternion.identity);

        return true;
    }

}
