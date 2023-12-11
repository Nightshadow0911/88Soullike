using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerBuff : MonoBehaviour
{    
    public static PlayerBuff Instance;
    public PlayerStatusHandler playerStatusHandler;
    [SerializeField] private GameObject buffUIPrefab;
    [SerializeField] private Transform buffUIHolder;
    //[SerializeField] private List<Buff> buffs = new List<Buff>();
    [SerializeField] private Dictionary<Buff, bool> buffs = new Dictionary<Buff, bool>();
    //���� SO �־�� Dictionary �߰� ����?

    private void Start()
    {
        playerStatusHandler = GameManager.instance.player.GetComponent<PlayerStatusHandler>();
    }


    public void AddBuff(Buff buff)
    {
        if (!buffs.ContainsKey(buff))
        {
            buffs.Add(buff, true); // activated this, ()

            buff.Activated(playerStatusHandler, () =>
            { //���ݷ� ������ ���� PlayerAttack���� �ϴ°� �´°�? ������ �����Ǵ� ��� ������ �ϳ��� ��ũ��Ʈ�� ���־�� ������
                buffs.Remove(buff);
                Destroy(buff.gameObject); 
            });
        }
        AddBuffUI(buff.buff);
    }
    // 1. PlayerStat.cs�� PlayerBuffStat �߰�
    // 2. PlayerStatusHandler���� 

    public void AddBuffUI(BuffSO buffSO)
    {
        GameObject bfUI = Instantiate(buffUIPrefab, buffUIHolder);
        bfUI.GetComponent<BuffUI>().Init(buffSO.StatName, buffSO.Value, buffSO.DurTime, buffSO.BuffImage);
        bfUI.GetComponent<BuffUI>().Excute();
    }
    
}
