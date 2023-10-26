using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Scriptable", menuName = "Scriptable/ItemEft/HealthEft")]
public class ItemHealingEft : ItemEffect
{
    public int healingPoint = 0;
    public override bool ExcuteRole(int power) //�������� ȿ��
    {
        healingPoint = power;
        Debug.Log("PlayerHp Add : "+ healingPoint); 
        return true;
    }
}
