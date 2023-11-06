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

    void Start()
    {
        healthSlider.value = 1;
        staminaSlider.value = 1;
    }

    void Update()
    {
        UpdateHealthUI();
        UpdateStaminaUI();
    }


    private void UpdateHealthUI()
    {
        int maxHealth = 100;
        int currentHealth = characterStats.characterHp;
        healthText.text = "HP: " + currentHealth + " / " + maxHealth;
        healthSlider.value = calculateHealthPercentage(currentHealth, maxHealth);
    }
    private void UpdateStaminaUI()
    {
        float maxStamina = 100f;
        float currentStamina = characterStats.characterStamina;
        staminaSlider.value = currentStamina / maxStamina;
    }
    float calculateHealthPercentage(int currentHealth, int maxHealth)
    {
        return (float)currentHealth / maxHealth; // Convert to float for accurate percentage
    }
}
