using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Scriptable", menuName = "Scriptable/ItemEft/BuffEft")]
public class ItemButtEft : ItemEffect
{
    private string selectStat;
    private CharacterStats ps;
    public override bool ExcuteRole(int power) //아이템의 효과
    {
        ps = GameManager.Instance.playerStats;
        switch(selectStat)
        {
            case "HP":

                break;
            case "ST":

                break;
            case "STR":

                break;
            case "DEX":

                break;
            case "LUK":

                break;
            case "INT":

                break;
        }

        return true;
    }
}
