using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Scriptable", menuName = "Scriptable/ItemEft/HealthEft")]
public class ItemHealingEft : ItemEffect
{
    public int healingPoint = 0;
    public override bool ExcuteRole(int power) //아이템의 효과
    {
        healingPoint = power;
        GameManager.Instance.playerStats.characterHp += healingPoint;
        if(GameManager.Instance.playerStats.characterHp >= GameManager.Instance.playerStats.MaxHP)
        {
            GameManager.Instance.playerStats.characterHp = GameManager.Instance.playerStats.MaxHP;
        }

        return true;
    }
}
