using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Scriptable", menuName = "Scriptable/ItemEft/EquipEft")]
public class ItemEquipEft : ItemEffect
{

    public bool type; // true -> weapon, false -> armor
    public int upgradePower = 0;
    public override bool ExcuteRole(int power) //아이템의 효과
    {
        upgradePower = power;
        if (type)
        {
            Debug.Log("Player Atk Add : " + upgradePower);
        }
        else
        {
            Debug.Log("Player Def Add : " + upgradePower);
        }
        return true;
    }

}
