using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Scriptable", menuName = "Scriptable/ItemEft/HealthEft")]
public class ItemHealingEft : ItemEffect
{
    public int healingPoint = 0;
    private PlayerStatusHandler playerStatusHandler;
    private PlayerStat playerStat; //
    private PlayerStat playerMaxStat; //

    public override bool ExcuteRole(int power) //�������� ȿ��
    {
        playerStatusHandler = GameManager.Instance.player.GetComponent<PlayerStatusHandler>();
        playerStat = playerStatusHandler.GetStat();
        playerMaxStat = playerStatusHandler.GetMaxStat();
        healingPoint = (int)Mathf.Ceil(playerMaxStat.hp * 0.4f);

        playerStat.hp += healingPoint;

        if(playerStat.hp >= playerMaxStat.hp)
        {
            playerStat.hp = playerMaxStat.hp;
        }
        return true;
    }
}
