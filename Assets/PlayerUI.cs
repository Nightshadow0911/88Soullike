using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    private GameManager gameManager;
    public CharacterStats characterStats;
    public LastPlayerController lastPlayerController;
    public Text healthText;
    public Slider healthSlider;
    public Slider staminaSlider;
    public Slider regainSlider;
    public Slider manaSlider;
    public Text manaText;
    void Start()
    {
        gameManager = GameManager.Instance;
        healthSlider.value = 1;
        staminaSlider.value = 1;
        regainSlider.value = 1;
        manaSlider.value = 1;
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
        int maxHealth = characterStats.MaxHP;
        int currentHealth = characterStats.characterHp;

        healthText.text = "HP: " + currentHealth + " / " + maxHealth;
        healthSlider.value = calculateHealthPercentage(currentHealth, maxHealth);
    }
    private void UpdateStaminaUI()
    {
        float maxStamina = characterStats.MaxStemina;
        float currentStamina = characterStats.characterStamina;
        staminaSlider.value = currentStamina / maxStamina;
    }

    private void UpdateManaUI()
    {
        int maxMana = characterStats.MaxMana;
        int currentMana = characterStats.characterMana;
        manaText.text = currentMana + " / " + maxMana;
        manaSlider.value = calculaterManaPercentage(currentMana, maxMana);
    }

    public void UpdateRegainHpUI()
    {
        int maxHealth = characterStats.MaxHP;
        int characterRegainHp = characterStats.characterRegainHp;
        if (gameManager.playerStats.characterRegainHp < gameManager.playerStats.characterHp)
        {
            gameManager.playerStats.characterRegainHp = gameManager.playerStats.characterHp;
        }
        if (gameManager.playerStats.characterHp <= 0)
        {
            gameManager.playerStats.characterRegainHp = 0;
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
