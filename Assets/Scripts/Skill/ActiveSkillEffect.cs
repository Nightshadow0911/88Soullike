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
    public override bool ExcuteRole(int power, SkillType type) 
    {
        skillPosition = GameManager.Instance.playerAttack.attackPoint.transform.position;
        GameObject go = Instantiate(skillPrefab, skillPosition, Quaternion.identity);

        return true;
    }

}
