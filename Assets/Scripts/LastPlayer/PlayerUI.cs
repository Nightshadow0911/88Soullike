using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    public PlayerStatusHandler PlayerStatusHandler;
    private PlayerStat PlayerStat;
    private PlayerStat MaxStat;
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
        PlayerStat = PlayerStatusHandler.GetStat();
        MaxStat = PlayerStatusHandler.GetMaxStat();
    }
    private void Awake()
    {
        PlayerStatusHandler = GameManager.Instance.player.GetComponent<PlayerStatusHandler>();
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
        int maxHealth = MaxStat.hp;
        int currentHealth = PlayerStat.hp;
        healthText.text = "HP: " + currentHealth + " / " + maxHealth;
        healthSlider.value = calculateHealthPercentage(currentHealth, maxHealth);
    }
    private void UpdateStaminaUI()
    {
        //float maxStamina = playerStatusHandler.GetMaxStat().stemina;
        //float currentStamina = playerStatusHandler.GetStat().stemina;
        float currentStamina = PlayerStat.stemina;
        float maxStamian = MaxStat.stemina;
        //Debug.Log("maxStamina ::" + maxStamina);
        //Debug.Log("currentStamina ::" + currentStamina);
        staminaSlider.value = currentStamina / maxStamian;
    }

    private void UpdateManaUI()
    {
        // int maxMana = MaxStat.mana;
        // int currentMana = PlayerStat.mana;
        // manaText.text = currentMana + " / " + maxMana;
        // manaSlider.value = calculaterManaPercentage(currentMana, maxMana);
    }

    public void UpdateRegainHpUI()
    {
        int maxHealth = MaxStat.hp;
        int characterRegainHp = PlayerStat.regainHp;
        int currentHealth = PlayerStat.hp;
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
