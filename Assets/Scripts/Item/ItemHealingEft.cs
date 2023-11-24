using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Scriptable", menuName = "Scriptable/ItemEft/HealthEft")]
public class ItemHealingEft : ItemEffect
{
    public int healingPoint = 0;
    private CharacterStats ps;

    private void Awake()
    {
        ps = GameManager.Instance.playerStats;
    }
    public override bool ExcuteRole(int power) //아이템의 효과
    {
        ps = GameManager.Instance.playerStats;
        healingPoint = (int)Mathf.Ceil(ps.MaxHP * 0.8f);

        ps.characterHp += healingPoint;

        if(ps.characterHp >= ps.MaxHP)
        {
            ps.characterHp = ps.MaxHP;
        }
        Debug.Log(healingPoint);

        return true;
    }
}
