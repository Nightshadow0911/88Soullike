using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    private PlayerStatusHandler playerStatusHandler;
    public LastPlayerController lastPlayerController;
    public Text healthText;
    public Slider healthSlider;
    public Slider staminaSlider;
    public Slider regainSlider;
    public Slider manaSlider;
    public Text manaText;
    void Start()
    {
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
        int maxHealth = 100;
        int currentHealth = 100;//playerStatusHandler.GetStat().hp;
        healthText.text = "HP: " + currentHealth + " / " + maxHealth;
        healthSlider.value = calculateHealthPercentage(currentHealth, maxHealth);
    }
    private void UpdateStaminaUI()
    {
        float maxStamina = 100;
        float currentStamina = 100;//playerStatusHandler.GetStat().stemina;
        staminaSlider.value = currentStamina / maxStamina;
    }

    private void UpdateManaUI()
    {
        int maxMana = 4;
        int currentMana = 4;
        manaText.text = currentMana + " / " + maxMana;
        manaSlider.value = calculaterManaPercentage(currentMana, maxMana);
    }

    public void UpdateRegainHpUI()
    {
        int maxHealth = 100;
        int characterRegainHp = 100;//playerStatusHandler.GetStat().regainHp;
        if (playerStatusHandler.GetStat().regainHp < playerStatusHandler.GetStat().hp)
        {
            playerStatusHandler.GetStat().regainHp = playerStatusHandler.GetStat().hp;
        }
        if (playerStatusHandler.GetStat().hp <= 0)
        {
            playerStatusHandler.GetStat().regainHp = 0;
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
