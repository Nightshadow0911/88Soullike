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

    private void Start()
    {
        Init();
    }

    private void Init()
    {
        playerName.text = "기사";
        levelValue.text = $"{playerMaxStat.level}";
        haveSoulValue.text = $"{inven.SoulCount}";
        //needSoulValue.text = $"{}"; => 현재 growStat을 얼마나 올렸냐에 따라 달라짐, 수식 적용(100개 * 몇번?) 한번 적용할때마다 10퍼센트씩 상승?
        healthStatValue.text = $"{playerBaseStat.healthStat + playerGrowStat.healthStat}";
        steminaStatValue.text = $"{playerBaseStat.steminaStat + playerGrowStat.steminaStat}";
        strStatValue.text = $"{playerBaseStat.strStat + playerGrowStat.strStat}";
        dexStatValue.text = $"{playerBaseStat.dexStat + playerGrowStat.dexStat}";
        intStatValue.text = $"{playerBaseStat.intStat + playerGrowStat.intStat}";
        luxStatValue.text = $"{playerBaseStat.luxStat + playerGrowStat.luxStat}";
    }
}
