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

    public override bool ExcuteRole(int power, SkillType type) //�������� ȿ��, �̴�θ� power�� ������� �ѵ�..
    {
        var buff = Instantiate(buffPrefab);
        pb = GameManager.instance.player.GetComponent<PlayerBuff>();

        pb.AddBuff(buff.GetComponent<Buff>());

        return true;
    }
}