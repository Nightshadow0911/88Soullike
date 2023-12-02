using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    private PlayerStatusHandler playerStatusHandler;
    private PlayerStat stat;
    public LastPlayerController lastPlayerController;
    public Text healthText;
    public Slider healthSlider;
    public Slider staminaSlider;
    public Slider regainSlider;
    public Slider manaSlider;
    public Text manaText;

    private int currentHealth;
    private float currentStamina;
    private int currentMana;

    void Start()
    {
        stat = playerStatusHandler.GetStat();
        healthSlider.value = 1;
        staminaSlider.value = 1;
        regainSlider.value = 1;
        manaSlider.value = 1;

    }
    private void Awake()
    {
        playerStatusHandler = GetComponent<PlayerStatusHandler>();
    }

    void Update()
    {
        UpdateHpUI();
        UpdateStaminaUI();
        UpdateRegainHpUI();
        UpdateManaUI();
    }
    //gameManager.playerStats.characterStamina

    private void UpdateHpUI()
    {
        int maxHealth = playerStatusHandler.playerMaxStat.hp; // Max값 
        currentHealth = playerStatusHandler.GetStat().hp; //현재 값 

        healthText.text = "HP: " + currentHealth + " / " + maxHealth;
        healthSlider.value = calculateHealthPercentage(currentHealth, maxHealth);
    }

    private void UpdateStaminaUI()
    {
        //float maxStamina = playerStatusHandler.playerMaxStat.stamina;
        //currentStamina = playerStatusHandler.GetStat().stamina;
        ////currentStamina = stat.stamina;
        //Debug.Log("dkdkdkdkdkdk:" + maxStamina);
        //Debug.Log("1111:" + currentStamina);
        //staminaSlider.value = currentStamina / maxStamina;

    }

    private void UpdateManaUI()
    {
        int maxMana = playerStatusHandler.playerMaxStat.mana;
        int currentMana = playerStatusHandler.GetStat().mana;
        manaText.text = currentMana + " / " + maxMana;
        manaSlider.value = calculaterManaPercentage(currentMana, maxMana);
    }

    public void UpdateRegainHpUI()
    {
        //int maxHealth = playerStatusHandler.playerMaxStat.hp; // Max값 
        //int characterRegainHp = characterStats.characterRegainHp;
        //if (gameManager.playerStats.characterRegainHp < gameManager.playerStats.characterHp)
        //{
        //    gameManager.playerStats.characterRegainHp = gameManager.playerStats.characterHp;
        //}
        //if (gameManager.playerStats.characterHp <= 0)
        //{
        //    gameManager.playerStats.characterRegainHp = 0;
        //}
        //regainSlider.value = calculateGuardPercentage(characterRegainHp, maxHealth);
        int maxHealth = playerStatusHandler.playerMaxStat.hp; // Max값 
        int characterRegainHp = playerStatusHandler.GetStat().regainHp;
        if (characterRegainHp<maxHealth)
        {
            characterRegainHp = maxHealth;
        }
        if (maxHealth<=0)
        {
            characterRegainHp = 0;
        }
        regainSlider.value = calculateGuardPercentage(characterRegainHp, maxHealth);
    }

    float calculateHealthPercentage(int currentHealth, int maxHealth)
    {
        return (float)currentHealth / maxHealth; // Convert to float for accurate percentage
    }
    float calculateGuardPercentage(int characterRegainHp, int maxHealth)
    {
        return (float)characterRegainHp / maxHealth; // Convert to float for accurate percentage
    }
    float calculaterManaPercentage(int currentMana, int maxMana)
    {
        return (float)currentMana / maxMana;
    }
}
