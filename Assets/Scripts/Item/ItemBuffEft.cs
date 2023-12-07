using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Scriptable", menuName = "Scriptable/ItemEft/BuffEft")]
public class ItemBuffEft : ItemEffect
{
    [SerializeField] private Transform buffPrefab;

    private PlayerBuff pb;

    private void Awake()
    {
        //ps = GameManager.Instance.playerStats;
    }
    public override bool ExcuteRole(int power) //�������� ȿ��, �̴�θ� power�� ������� �ѵ�..
    {
        // PlayerBuff.Instance�� ���߿� PlayerStat���� ������
       // PlayerBuff.Instance.AddBuff(curBuff); // Buff - BuffSO - StatName�� ���� ȿ���� �޶���
        var buff = Instantiate(buffPrefab);
        pb = GameManager.instance.player.GetComponent<PlayerBuff>();
        pb.AddBuff(buff.GetComponent<Buff>());
        return true;
    }
}
