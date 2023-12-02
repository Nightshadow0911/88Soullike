using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelUpUI : MonoBehaviour
{
    public Text[] statTexts; // 스텟에 대응하는 텍스트 배열
    public Text levelText;
    public Text pointsText;
    public Button levelUpButton;

    public PlayerStat playerStat;
    private PlayerStatusHandler playerStatusHandler;

    public enum StatType // 여기서 public으로 선언
    {
        Health,
        Stemina,
        Str,
        Dex,
        Int,
        Lux,
        None
    }

    private StatType selectedStat; // 선택한 스텟을 나타내는 열거형 변수

    private void Start()
    {
        if (playerStat != null)
        {
            playerStatusHandler = GetComponent<PlayerStatusHandler>(); // 또는 다른 방식으로 PlayerStatusHandler를 가져와서 할당

            if (playerStatusHandler != null)
            {
                levelUpButton.onClick.AddListener(LevelUp);
                UpdateUI();
            }
            else
            {
                Debug.LogError("PlayerStatusHandler 스크립트가 연결되지 않았습니다.");
            }
        }
        else
        {
            Debug.LogError("PlayerStat 스크립트가 연결되지 않았습니다.");
        }
    }

    private void UpdateUI()
    {
        for (int i = 0; i < statTexts.Length; i++)
        {
            statTexts[i].text = statTexts[i].name + ": " + GetStatValue((StatType)i);
        }
        levelText.text = "Level: " + playerStat.level;
        pointsText.text = "Points: " + playerStat.levelPoint;
    }

    public void LevelUp()
    {
        if (selectedStat != StatType.None)
        {
            if (playerStatusHandler.GrowUpStat(1, ConvertToStatus(selectedStat)))
            {
                UpdateUI();
            }
        }
        Debug.Log("버튼은 눌림");
    }

    private Status ConvertToStatus(StatType statType)
    {
        switch (statType)
        {
            case StatType.Health:
                return Status.Health;
            case StatType.Stemina:
                return Status.Stemina;
            case StatType.Str:
                return Status.Str;
            case StatType.Dex:
                return Status.Dex;
            case StatType.Int:
                return Status.Int;
            case StatType.Lux:
                return Status.Lux;
            default:
                Debug.Log("statType이 정해지지 못했습니다.");
                return Status.Health;
        }
    }

    public void SetSelectedStat(int statIndex)
    {
        // 사용자가 선택한 스텟을 설정
        selectedStat = (StatType)statIndex;
        Debug.Log("Selected Stat: " + selectedStat);
    }

    private int GetStatValue(StatType statType)
    {
        switch (statType)
        {
            case StatType.Health:
                return playerStat.healthStat;
            case StatType.Stemina:
                return playerStat.staminaStat;
            case StatType.Str:
                return playerStat.strStat;
            case StatType.Dex:
                return playerStat.dexStat;
            case StatType.Int:
                return playerStat.intStat;
            case StatType.Lux:
                return playerStat.luxStat;
            default:
                return 0;
        }
    }
}
