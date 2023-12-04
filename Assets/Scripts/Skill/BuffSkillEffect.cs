using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.XR.Haptics;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "Scriptable", menuName = "Scriptable/SkillEft/BuffEft")]
public class BuffSkillEffect : SkillEffect
{
    [SerializeField] private Transform buffPrefab;

    private PlayerBuff pb;

    public override bool ExcuteRole(int power, SkillType type) //아이템의 효과, 이대로면 power는 쓸모없긴 한데..
    {
        var buff = Instantiate(buffPrefab);
        pb = GameManager.Instance.player.GetComponent<PlayerBuff>();

        pb.AddBuff(buff.GetComponent<Buff>());

        return true;
    }
}