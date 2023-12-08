using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;
using Slider = UnityEngine.UI.Slider;

public class BossHealthBar : MonoBehaviour
{
    [SerializeField] private Slider healthBar;
    [SerializeField] private TextMeshProUGUI textUI;
    [SerializeField] private string bossName;
    private EnemyStatusHandler handler;
    private int maxHealth;

    private void Awake()
    {
        handler = GetComponent<EnemyStatusHandler>();
        handler.OnDamage += ChangeHealth;
        textUI.text = bossName;
    }

    private void Start()
    {
        maxHealth = handler.GetStat().hp;
        ChangeHealth();
    }

    private void ChangeHealth()
    {
        healthBar.value = (float)handler.currentHp / maxHealth;
    }
}
