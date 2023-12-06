using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FStatus : MonoBehaviour
{
    [SerializeField] protected PlayerStatusHandler playerStatusHandler;
    [SerializeField] protected PlayerStat playerMaxStat;
    [SerializeField] protected PlayerStat playerBaseStat;
    [SerializeField] protected PlayerStat playerGrowStat;
    [SerializeField] protected Inventory inven;

    private void Awake()
    {
        playerStatusHandler = GameManager.instance.player.GetComponent<PlayerStatusHandler>();
        playerMaxStat = playerStatusHandler.GetStat();
        playerBaseStat = playerStatusHandler.baseStatSO;
        playerGrowStat = playerStatusHandler.growStatSO;
        inven = Inventory.instance;
      
    }
}
