using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FMainStatusUI : FStatus
{

    [Header("기본능력")]
    [SerializeField] private TMP_Text playerName;
    [SerializeField] private TMP_Text levelValue;
    [SerializeField] private TMP_Text haveSoulValue;
    [SerializeField] private TMP_Text needSoulValue;
    [SerializeField] private TMP_Text healthStatValue;
    [SerializeField] private TMP_Text steminaStatValue;
    [SerializeField] private TMP_Text strStatValue;
    [SerializeField] private TMP_Text dexStatValue;
    [SerializeField] private TMP_Text intStatValue;
    [SerializeField] private TMP_Text luxStatValue;

    int needSoul = 0;

    private void Init()
    {
        playerName.text = "기사";
        levelValue.text = $"Lv.{playerMaxStat.level}";
        haveSoulValue.text = $"{inven.SoulCount:N0}";
        
        needSoul = (int)(100 * Mathf.Pow(1.1f, playerMaxStat.level - 1));
        needSoulValue.text = $"{needSoul:N0}";

        healthStatValue.text = $"{playerMaxStat.healthStat}";
        steminaStatValue.text = $"{playerBaseStat.steminaStat + playerGrowStat.steminaStat}";
        strStatValue.text = $"{playerBaseStat.strStat + playerGrowStat.strStat}";
        dexStatValue.text = $"{playerBaseStat.dexStat + playerGrowStat.dexStat}";
        intStatValue.text = $"{playerBaseStat.intStat + playerGrowStat.intStat}";
        luxStatValue.text = $"{playerBaseStat.luxStat + playerGrowStat.luxStat}";
    }
    protected override void Update()
    {
        Init();
    }

}
