using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FBasicStatusUI : FStatus
{
    [Header("일반능력")]
    [SerializeField] private TMP_Text healthValue;
    [SerializeField] private TMP_Text manaValue;
    [SerializeField] private TMP_Text steminaValue;
    [SerializeField] private TMP_Text weightValue;
    [SerializeField] private TMP_Text regainValue;
    [SerializeField] private TMP_Text moveSpeedValue;

    [Header("공격능력")]
    [SerializeField] private TMP_Text attackDamageValue;
    [SerializeField] private TMP_Text spellPowerValue;
    [SerializeField] private TMP_Text propertyDamageValue;
    [SerializeField] private TMP_Text criticalRateValue;
    [SerializeField] private TMP_Text attackSpeed;


    private void Start()
    {
        Init();
    }

    private void Init()
    {
        healthValue.text = $"{playerStatusHandler.currentHp} / {playerMaxStat.hp}";
        manaValue.text = $"{playerStatusHandler.curretMana} / {playerMaxStat.mana}";
        steminaValue.text = $"{playerStatusHandler.currentStemina:F0} / {playerMaxStat.stemina}";
        weightValue.text = $"{playerStatusHandler.currentWeight} / {playerMaxStat.weight}" +
            $"({(playerStatusHandler.currentWeight / playerMaxStat.weight * 100):F1}%)";
        regainValue.text = $"{playerStatusHandler.curretRegainHp}";
        moveSpeedValue.text = $"{playerStatusHandler.currentSpeed}";

        attackDamageValue.text = $"{playerStatusHandler.currentDamage}";
        spellPowerValue.text = $"{playerStatusHandler.currentSpellPower}";
        propertyDamageValue.text = $"{playerStatusHandler.currentpropertyDamage}";
        criticalRateValue.text = $"{playerStatusHandler.currentCritical*100:F0}%";
        attackSpeed.text = $"{playerStatusHandler.currentDelay}";

    }

    protected override void Update()
    {
        Init();
    }
}
