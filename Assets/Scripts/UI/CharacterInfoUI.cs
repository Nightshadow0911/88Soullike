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

    private PlayerStatusHandler playerStatHandler;
    private PlayerStat playerMaxStat;

    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        playerStatHandler = GameManager.instance.player.GetComponent<PlayerStatusHandler>();
        playerMaxStat = playerStatHandler.GetStat();
    }
    private void Update()
    {
        UpdateStatus();

    }

    public void UpdateStatus()
    {
        levelTxt.text = $"LV.{playerMaxStat.level}";
        healthTxt.text = $"{playerStatHandler.currentHp} / {playerMaxStat.hp}";
        steminaTxt.text = $"{playerStatHandler.currentStemina}:F0 / {playerMaxStat.stemina}";
        int equipWeight = 0;
        foreach (Item ew in Equipment.instance.equipItemList)
        {
            if (ew != null)
                equipWeight += ew.Weight;
        }
        weightTxt.text = $"{equipWeight} / {playerStatHandler.currentWeight}";

        speedTxt.text = $"{playerStatHandler.currentSpeed}";

        weaponTxt.text = $"[E] {Equipment.instance.equipItemList[0]?.ItemName}";
        attackTxt.text = $"{playerStatHandler.currentDamage}";
        skillTxt.text = $"{playerStatHandler.currentSpellPower}";
        propertyTxt.text = $"{playerStatHandler.currentpropertyDamage}";
        criticalTxt.text = $"{playerStatHandler.currentCritical:F1}%";
        attackSpeedTxt.text = $"{playerStatHandler.currentDelay}";

        deffenceTxt.text = $"{playerStatHandler.currentDefense}";
        parryTimeTxt.text = $"{(playerStatHandler.currentParryTime):F2}";
        addGoodTxt.text = $"{playerStatHandler.currentSoulDrop}";

        //growPoint.text = $"포인트 : {playerStatHandler.levelPoint}";
        growHealthTxt.text = $"체력 {playerMaxStat.hp}()";
        growStemenaTxt.text = $"스테미나 {playerMaxStat.stemina}()";
        growStrTxt.text = $"힘 {playerMaxStat.strStat}()"; // 축적된 힘을 가질 변수 필요
        growDexTxt.text = $"민첩 {playerMaxStat.dexStat}()";
        growIntTxt.text = $"지력 {playerMaxStat.intStat}()";
        growLukTxt.text = $"운 {playerMaxStat.luxStat}()";
    }


    public void GrowStat(Status statName)
    {
            playerStatusHandler.GrowUpStat(1, statName);
            UpdateStatus();
    }

    public void TogglePopup()
    {
        isOpen = !isOpen;
        growthPopup.SetActive(isOpen);
    }
}
