using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBuff : MonoBehaviour
{
    // 플레이어 스탯에 붙여줄 버프시스템 이후 삭제
    
    public static PlayerBuff Instance;
    //[SerializeField] private List<Buff> buffs = new List<Buff>();
    [SerializeField] private Dictionary<Buff, bool> buffs = new Dictionary<Buff, bool>();

    public void AddBuff(Buff buff)
    {
        if (!buffs.ContainsKey(buff))
        {
            buffs.Add(buff, true); // activated this, ()
            buff.Activated(transform.GetComponent<CharacterStats>(), () =>
            {
                buffs.Remove(buff);
                Destroy(buff.gameObject);
                Debug.Log(buff.buff.name + " : 버프 제거됨");

            });
            Debug.Log(buff.buff.name + " : 버프 적용");
        } else
        {
            //적용 실패 => amount를 돌려줄것인가? Item.Use 부분에서 막아야하나?
        }

        /*if (!buffs.Contains(buff))
        {
            buffs.Add(buff); // activated this, ()
            buff.Activated(transform.GetComponent<CharacterStats>(), () =>
            {
                buffs.Remove(buff);
                Destroy(buff.gameObject);
                Debug.Log(buff.buff.name + " : 버프 제거됨");

            });
        }
        Debug.Log(buff.buff.name + " : 버프 적용");*/
    }
    
}
