using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class LevelUpUI : MonoBehaviour
{
    public Text[] statTexts; // 스텟에 대응하는 텍스트 배열
    public Text levelText;
    public Text pointsText;
    public Button levelUpButton;

    public CharacterStats characterStats;

    private StatType selectedStat; // 선택한 스텟을 나타내는 열거형 변수

    
    public enum StatType
    {
        HP,
        Stamina,
        STR,
        Dexterity,
        Intelligence,
        Lux,
        None
    }

    private void Start()
    {
        if (characterStats != null)
        {
            UpdateUI();
            levelUpButton.onClick.AddListener(LevelUp);
        }
        else
        {
            Debug.LogError("CharacterStats 스크립트가 연결되지 않았습니다.");
        }
    }

    private void UpdateUI()
    {
        for (int i = 0; i < statTexts.Length; i++)
        {
            statTexts[i].text = statTexts[i].name + ": " + GetStatValue((StatType)i);
        }
        levelText.text = "Level: " + characterStats.Level;
        pointsText.text = "Points: " + characterStats.Points;
    }

    public void LevelUp()
    {
        if (selectedStat != StatType.None) // 선택한 스텟이 None이 아니면
        {
            if (characterStats.TryLevelUp(selectedStat.ToString()))
            {
                UpdateUI();
            }
        }
        Debug.Log("Level Up Button Clicked");
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
            case StatType.HP:
                return characterStats.GrowHP;
            case StatType.STR:
                return characterStats.GrowStr;
            case StatType.Stamina:
                return characterStats.GrowStemina;
            case StatType.Dexterity:
                return characterStats.GrowDex;
            case StatType.Intelligence:
                return characterStats.GrowInt;
            case StatType.Lux:
                return characterStats.GrowLux;
            default:
                return 0;
        }
    }
}
