using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FAbillityStatusUI : FStatus
{

    [Header("방어능력")]
    [SerializeField] private TMP_Text deffenseValue;
    [SerializeField] private TMP_Text propertyDeffenseValue;
    [SerializeField] private TMP_Text parryTimeValue;

    [Header("특수능력")]
    [SerializeField] private TMP_Text soulDropValue;

    private void Init()
    {
        deffenseValue.text = $"{playerStatusHandler.currentDefense}";
        propertyDeffenseValue.text = $"{playerStatusHandler.currentpropertyDefense}";
        parryTimeValue.text = $"{playerStatusHandler.currentParryTime}";
        soulDropValue.text = $"{playerStatusHandler.currentSoulDrop}%";

    }
    protected override void Update()
    {
        Init();
    }
}
