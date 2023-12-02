using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.XR.Haptics;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "Scriptable", menuName = "Scriptable/SkillEft/BuffEft")]
public class BuffSkillEffect : SkillEffect
{
    //버프로 올릴 플레이어 스탯, 패시브와 달리 최종 공격력, 모든 피해 감소 등 극적인 효과를 준다.
    [SerializeField] private Transform buffPrefab;

    private PlayerBuff pb;

    private void Awake()
    {
        //ps = GameManager.Instance.playerStats;
    }
    public override bool ExcuteRole(int power, SkillType type) //아이템의 효과, 이대로면 power는 쓸모없긴 한데..
    {
        // PlayerBuff.Instance는 나중에 PlayerStat으로 들어가야함
        // PlayerBuff.Instance.AddBuff(curBuff); // Buff - BuffSO - StatName에 따라 효과가 달라짐
        var buff = Instantiate(buffPrefab);
        pb = GameManager.Instance.player.GetComponent<PlayerBuff>();
        pb.AddBuff(buff.GetComponent<Buff>());
        return true;
    }
}




