using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    private PlayerStatusHandler playerStatusHandler;
    private PlayerStat PlayerStat;
    private PlayerStat maxStat;
    private Inventory inven;
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
        inven = Inventory.instance;
        healthSlider.value = 1;
        staminaSlider.value = 1;
        regainSlider.value = 1;
        manaSlider.value = 1;
        maxStat = playerStatusHandler.GetStat();
    }

    void Update()
    {
        UpdateHpUI();
        UpdateStaminaUI();
        UpdateRegainHpUI();
        UpdateManaUI();
        SoulText();
    }

    private void SoulText()
    {
        StringBuilder soulStringBuilder = CreateStringBuilder("SOUL : ", inven.SoulCount.ToString("N0"));
        soulText.text = soulStringBuilder.ToString();
    }

    private StringBuilder CreateStringBuilder(params string[] values)
    {
        StringBuilder stringBuilder = new StringBuilder();
        foreach (string value in values)
        {
            stringBuilder.Append(value);
        }
        return stringBuilder;
    }

    private void UpdateHpUI()
    {
        int currentHealth = playerStatusHandler.currentHp;
        int maxHealth = maxStat.hp;
        healthText.text = CreateString($"HP: {currentHealth} / {maxHealth}");
        healthSlider.value = CalculatePercentage(currentHealth, maxHealth);
    }

    private void UpdateStaminaUI()
    {
        float currentStamina = playerStatusHandler.currentStemina;
        float maxStamina = maxStat.stemina;
        staminaSlider.value = currentStamina / maxStamina;
    }

    private void UpdateManaUI()
    {
        int currentMana = playerStatusHandler.currentMana;
        int maxMana = maxStat.mana;
        manaText.text = CreateString($"{currentMana} / {maxMana}");
        manaSlider.value = CalculatePercentage(currentMana, maxMana);
    }

    public void UpdateRegainHpUI()
    {
        int characterRegainHp = playerStatusHandler.currentRegainHp;
        int maxHealth = maxStat.hp;
        int currentHealth = playerStatusHandler.currentHp;

        if (characterRegainHp < currentHealth)
        {
            characterRegainHp = currentHealth;
        }
        if (currentHealth <= 0)
        {
            characterRegainHp = 0;
        }
        regainSlider.value = CalculatePercentage(characterRegainHp, maxHealth);
    }

    private string CreateString(params string[] values)
    {
        return string.Concat(values);
    }

    private float CalculatePercentage(float currentValue, float maxValue)
    {
        return currentValue / maxValue;
    }
}
