using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menu : MonoBehaviour
{
    public GameObject menuUI;
    public GameObject travelUI;
    public GameObject levelUpUI;

    public void Rest()
    {
        // 플레이어 체력, 스테미나, 회복물약 충전
    }
    
    public void TravelMenu()
    {
        travelUI.SetActive(true);
    }

    public void LevelupMenu()
    {
        levelUpUI.SetActive(true);
    }

    public void AllCancel()
    {
        menuUI.SetActive(false);
        travelUI.SetActive(false);
        //levelUpUI.SetActive(false);
    }

    public void BackMainMenu()
    {
        travelUI.SetActive(false);
        //levelUpUI.SetActive(false);
    }
}
