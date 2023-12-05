using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Scriptable", menuName = "Scriptable/ItemEft/HealthEft")]
public class ItemHealingEft : ItemEffect
{
    public int healingPoint = 0;
    private PlayerStatusHandler playerStatusHandler;
    private PlayerStat playerMaxStat; 

    public override bool ExcuteRole(int power) //�������� ȿ��
    {
        playerStatusHandler = GameManager.Instance.player.GetComponent<PlayerStatusHandler>();
        playerMaxStat = playerStatusHandler.GetStat();
        healingPoint = (int)Mathf.Ceil(playerMaxStat.hp * 0.4f);

        playerStatusHandler.currentHp += healingPoint;

        if(playerStatusHandler.currentHp >= playerMaxStat.hp)
        {
            playerStatusHandler.currentHp = playerMaxStat.hp;
        }
        return true;
    }
}
