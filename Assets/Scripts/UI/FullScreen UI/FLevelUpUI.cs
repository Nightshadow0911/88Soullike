using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using TMPro;
using UnityEngine;

public class FLevelUpUI : MonoBehaviour
{
    public static FLevelUpUI instance;

    [SerializeField] private int needSoul;
    [SerializeField] private TMP_Text needSoulValue;
    [SerializeField] private int haveSoul;
    [SerializeField] private TMP_Text haveSoulValue;
    [SerializeField] private PlayerStatusHandler playerStatusHandler;
    [SerializeField] private PlayerStat playerMaxStat;
    [SerializeField] private Inventory inven;

    [Header("스탯 Text")]
    [SerializeField] private TMP_Text healthStatTxt;
    [SerializeField] private TMP_Text steminaStatTxt;
    [SerializeField] private TMP_Text strStatTxt;
    [SerializeField] private TMP_Text dexStatTxt;
    [SerializeField] private TMP_Text intStatTxt;
    [SerializeField] private TMP_Text luxStatTxt;

    [Header("스탯 Value")]
    [SerializeField] private int healthStat;
    [SerializeField] private int steminaStat;
    [SerializeField] private int strStat;
    [SerializeField] private int dexStat;
    [SerializeField] private int intStat;
    [SerializeField] private int luxStat;

    [SerializeField] private int[] points;
    [SerializeField] private int sumPoint;


    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;

    }
    private void Start()
    {
        playerStatusHandler = GameManager.instance.player.GetComponent<PlayerStatusHandler>();
        playerMaxStat = playerStatusHandler.GetStat();
        inven = Inventory.instance;
        points = new int[6];
        Init();
    }
    void OnDisable()
    {
        Init();
    }

    public void Init()
    {
        sumPoint = 0;
        for (int i = 0; i < points.Length; i++)
        {
            points[i] = 0;
        }

        healthStat = playerMaxStat.healthStat;
        steminaStat = playerMaxStat.steminaStat;
        strStat = playerMaxStat.strStat;
        dexStat = playerMaxStat.dexStat;
        intStat = playerMaxStat.intStat;
        luxStat = playerMaxStat.luxStat;

        healthStatTxt.text = $"{healthStat}";
        steminaStatTxt.text = $"{steminaStat}";
        strStatTxt.text = $"{strStat}";
        dexStatTxt.text = $"{dexStat}";
        intStatTxt.text = $"{intStat}";
        luxStatTxt.text = $"{luxStat}";

        UpdateNeedSoul();

        haveSoul = inven.SoulCount;
        haveSoulValue.text = $"{inven.SoulCount:N0}";
    }

    public void IncreaseStat(int statIndex)
    {
        // if (needSoul > haveSoul) return;
        points[statIndex]++;
        sumPoint++;

        UpdateStat(statIndex);
    }
    public void DecreaseStat(int statIndex)
    {
        if ((sumPoint <= 0) || (points[statIndex] <= 0)) return;

        points[statIndex]--;
        sumPoint--;

        UpdateStat(statIndex);
    }
    public void UpdateStat(int statIndex)
    {
        switch (statIndex)
        {
            case 0:
                healthStatTxt.text = $"{healthStat + points[statIndex]}";
                break;
            case 1:
                steminaStatTxt.text = $"{steminaStat + points[statIndex]}";
                break;
            case 2:
                strStatTxt.text = $"{strStat + points[statIndex]}";
                break;
            case 3:
                dexStatTxt.text = $"{dexStat + points[statIndex]}";
                break;
            case 4:
                intStatTxt.text = $"{intStat + points[statIndex]}";
                break;
            case 5:
                luxStatTxt.text = $"{luxStat + points[statIndex]}";
                break;
        }
        UpdateNeedSoul();
    }

    void UpdateNeedSoul()
    {
        needSoul = (int)(100 * Mathf.Pow(1.1f, (playerStatusHandler.currentLevel + sumPoint) - 1));
        needSoulValue.text = $"{needSoul:N0}";
    }
    
    public void SubmitLevelUp()
    {

        if(inven.SoulCount >= needSoul)
        {
            playerStatusHandler.currentLevel += sumPoint;
            inven.SoulCount -= needSoul;

            for(int i = 0; i < points.Length; i++)
            {
                if (points[i] != 0)
                {
                    Status status = (Status)Enum.Parse(typeof(Status), i.ToString());
                    playerStatusHandler.GrowUpStat(points[i], status); 
                   
                    //playerStatusHandler.GrowUpStat(points[i], Status.);
                    UpdateStat(i);
                }
            }

            
        } else
        {
            Debug.Log("소울이 부족합니다.");
        }

    }

}
