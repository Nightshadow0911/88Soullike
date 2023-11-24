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
    public override bool ExcuteRole(int power) //아이템의 효과, 이대로면 power는 쓸모없긴 한데..
    {
        // PlayerBuff.Instance는 나중에 PlayerStat으로 들어가야함
       // PlayerBuff.Instance.AddBuff(curBuff); // Buff - BuffSO - StatName에 따라 효과가 달라짐
        var buff = Instantiate(buffPrefab);
        pb = GameManager.Instance.player.GetComponent<PlayerBuff>();
        pb.AddBuff(buff.GetComponent<Buff>());
        return true;
    }
}
