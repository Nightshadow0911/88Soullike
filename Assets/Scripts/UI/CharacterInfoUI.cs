using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class CharacterInfoUI : MonoBehaviour
{
    public PlayerStatusHandler playerStatusHandler;
    
    public static CharacterInfoUI instance;
    public GameObject growPopupBtn;

    public GameObject growthPopup;
    private bool isOpen = false;

    [Header("General ability")]
    [SerializeField] private TMP_Text levelTxt;
    //[SerializeField] private TMP_Text expTxt; // 경험치 %(현재 경험치 / 경험치 통)
    [SerializeField] private TMP_Text healthTxt; // 현재체력 / 최대체력
    [SerializeField] private TMP_Text steminaTxt; // 현재 스템 / 최대 스템 
    [SerializeField] private TMP_Text weightTxt; // 현재 무게 / 무게 제한
    [SerializeField] private TMP_Text speedTxt; // 이동속도

    [Header("Attack ability")]
    [SerializeField] private TMP_Text weaponTxt; //장착무기 이름(가진 무기의 이름)
    [SerializeField] private TMP_Text attackTxt; // 공격력
    [SerializeField] private TMP_Text skillTxt; // 주문력
    [SerializeField] private TMP_Text propertyTxt; // 속성공격력
    [SerializeField] private TMP_Text criticalTxt; // 치명타율
    [SerializeField] private TMP_Text attackSpeedTxt; // 공속

    [Header("Deffence / Special")]
    [SerializeField] private TMP_Text deffenceTxt; // 방어력
    [SerializeField] private TMP_Text parryTimeTxt; // 패링가능시간
    [SerializeField] private TMP_Text addGoodTxt; // 아이템 획득량

    [Header("growStat")]
    [SerializeField] private TMP_Text growPoint;
    [SerializeField] private TMP_Text growHealthTxt;
    [SerializeField] private TMP_Text growStemenaTxt;
    [SerializeField] private TMP_Text growStrTxt;
    [SerializeField] private TMP_Text growDexTxt;
    [SerializeField] private TMP_Text growIntTxt;
    [SerializeField] private TMP_Text growLukTxt;

    private CharacterStats playerStat;

    private void Awake()
    {
        instance = this;
        playerStat = GameManager.Instance.playerStats;
        
}
    private void Update()
    {
        UpdateStatus();

    }

    public void UpdateStatus()
    {
        levelTxt.text = $"LV.{playerStat.Level} ({(100*((float)playerStat.curExp / playerStat.maxExp)):F1}%)"; // ()안에 {(현재 경험치 / 경험치 통):F1}
        healthTxt.text = $"{playerStat.characterHp} / {playerStat.MaxHP}";
        steminaTxt.text = $"{(int)Math.Floor(playerStat.characterStamina)} / {playerStat.MaxStemina}";
        int equipWeight = 0;
        foreach(Item ew in Equipment.instance.equipItemList)
        {
            if(ew != null)
            equipWeight += ew.Weight;
        }
        weightTxt.text = $"{equipWeight} / {playerStat.CharacterWeight}";

        speedTxt.text = $"{playerStat.moveState}";

        weaponTxt.text = $"[E] {Equipment.instance.equipItemList[0]?.ItemName}";
        attackTxt.text = $"{playerStat.NormalAttackDamage}";
        skillTxt.text = $"{playerStat.NormalSkillDamage}";
        propertyTxt.text = $"{playerStat.PropertyDamage}";
        criticalTxt.text = $"{playerStat.Critical:F1}%";
        attackSpeedTxt.text = $"{playerStat.AttackSpeed}";

        deffenceTxt.text = $"{playerStat.CharacterDefense}";
        parryTimeTxt.text = $"{(playerStat.ParryTime):F2}";
        addGoodTxt.text = $"{playerStat.AddGoods}";

        growPoint.text = $"포인트 : {playerStat.Points}";
        growHealthTxt.text = $"체력 {playerStat.MaxHP}({playerStat.GrowHP})";
        growStemenaTxt.text = $"스테미나 {playerStat.MaxStemina}({playerStat.GrowStemina})";
        growStrTxt.text = $"힘 {playerStat.MaxStr}({playerStat.GrowStr})"; // 축적된 힘을 가질 변수 필요
        growDexTxt.text = $"민첩 {playerStat.MaxDex}({playerStat.GrowDex})";
        growIntTxt.text = $"지력 {playerStat.MaxInt}({playerStat.GrowInt})";
        growLukTxt.text = $"운 {playerStat.MaxLuk}({playerStat.GrowLux})";
    }

    
    //민열님 여기 간단하게 바꾸긴했는데 확실하게확인해셔야할거같아서 냅둘게요
    //주석 풀으셔도 playerStatusHandlerd 설정안되어있어요.
    // public void GrowStat(string statName)
    // {
    //     Status status = ConvertToStatus(statName);
    //     if (status != Status.None)
    //     {
    //         playerStatusHandler.GrowUpStat(1, status);
    //         UpdateStatus();
    //     }
    //     else
    //     {
    //         Debug.LogError($"Invalid stat name: {statName}");
    //     }
    // }
    //
    // private Status ConvertToStatus(string statName)
    // {
    //     switch (statName)
    //     {
    //         case "Health":
    //             return Status.Health;
    //         case "Stemina":
    //             return Status.Stemina;
    //         case "Str":
    //             return Status.Str;
    //         case "Dex":
    //             return Status.Dex;
    //         case "Int":
    //             return Status.Int;
    //         case "Luk":
    //             return Status.Lux;
    //         default:
    //             return Status.None;
    //     }
    // }
    public void TogglePopup()
    {
        isOpen = !isOpen;
        growthPopup.SetActive(isOpen);
    }
}
