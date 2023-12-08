using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{

    private PlayerStatusHandler playerStatusHandler;
    private PlayerStat PlayerStat;
    private PlayerStat maxStat;
    public Text healthText;
    public Slider healthSlider;
    public Slider staminaSlider;
    public Slider regainSlider;
    public Slider manaSlider;
    public Text manaText;
    public Text soulText;
    private int soul;

    void Start()
    {
        playerStatusHandler = GameManager.instance.player.GetComponent<PlayerStatusHandler>();
        healthSlider.value = 1;
        staminaSlider.value = 1;
        regainSlider.value = 1;
        manaSlider.value = 1;
        maxStat = playerStatusHandler.GetStat();
        soul = Inventory.instance.SoulCount;
    }
 
    void Update()
    {

        UpdateHpUI();
        UpdateStaminaUI();
        UpdateRegainHpUI();
        UpdateManaUI();
        SoulText();
    }
    //gameManager.playerStats.characterStamina

    private void SoulText()
    {
        soulText.text = "SOUL : " + soul;
    }
    private void UpdateHpUI()
    {
        int currentHealth = playerStatusHandler.currentHp;// current값 
        int maxHealth = maxStat.hp; // max 값
        if (currentHealth>100)
        {
            currentHealth = 100;
        }
        healthText.text = "HP: " + currentHealth + " / " + maxHealth;
        healthSlider.value = calculateHealthPercentage(currentHealth, maxHealth);
    }
    private void UpdateStaminaUI()
    {
        float currentStamina = playerStatusHandler.currentStemina;
        float maxStamina = maxStat.stemina;
        staminaSlider.value = currentStamina / maxStamina;
    }

    private void UpdateManaUI()
    {
        int maxMana = playerStatusHandler.currentMana;
        int currentMana = maxStat.mana;
        manaText.text = currentMana + " / " + maxMana;
        //Debug.Log("maxMana ::" + maxMana);
        manaSlider.value = calculaterManaPercentage(currentMana, maxMana);
    }

    public void UpdateRegainHpUI()
    {
        int characterRegainHp = playerStatusHandler.currentRegainHp; //(현재값 )
        int maxHealth = maxStat.hp; // max 값
        int currentHealth = playerStatusHandler.currentHp;//(현재값 )
        //Debug.Log("currentRegainHp:" + characterRegainHp);
        if (characterRegainHp < currentHealth) 
        {
            characterRegainHp = currentHealth;
        }
        if (currentHealth <= 0)
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
