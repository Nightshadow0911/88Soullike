using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerBuff : MonoBehaviour
{    
    public static PlayerBuff Instance;
    [SerializeField] private GameObject buffUIPrefab;
    [SerializeField] private Transform buffUIHolder;
    //[SerializeField] private List<Buff> buffs = new List<Buff>();
    [SerializeField] private Dictionary<Buff, bool> buffs = new Dictionary<Buff, bool>();
    //버프 SO 넣어둔 Dictionary 추가 생성?


    public void AddBuff(Buff buff)
    {
        if (!buffs.ContainsKey(buff))
        {
            buffs.Add(buff, true); // activated this, ()
            buff.Activated(transform.GetComponent<PlayerAttack>(), () =>
            { //공격력 버프는 아직 PlayerAttack에서 하는게 맞는가? 버프로 조정되는 모든 스탯이 하나의 스크립트에 모여있어야 좋은데
                buffs.Remove(buff);
                Destroy(buff.gameObject);
                Debug.Log(buff.buff.name + " : 버프 제거됨");

            });
            Debug.Log(buff.buff.name + " : 버프 적용");
        }
        AddBuffUI(buff.buff);
    }
    // 1. PlayerStat.cs에 PlayerBuffStat 추가
    // 2. PlayerStatusHandler에서 

    public void AddBuffUI(BuffSO buffSO)
    {
        GameObject bfUI = Instantiate(buffUIPrefab, buffUIHolder);
        bfUI.GetComponent<BuffUI>().Init(buffSO.StatName, buffSO.Value, buffSO.DurTime, buffSO.BuffImage);
        bfUI.GetComponent<BuffUI>().Excute();
    }
    
}
