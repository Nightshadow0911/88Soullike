using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
/// <summary>
/// 데미지 출력
/// </summary>
public class Damage : MonoBehaviour
{
    public GameObject damageTextPrefab;
    public GameObject healthTextPrefab;
    public Canvas gameCanvas;
    public GameObject playerUI;
    private void Awake()
    {
        playerUI = FindObjectOfType<Canvas>().gameObject;
    }

    private void OnEnable()
    {
        PlayerEvents.enemyDamaged+=(EnemyTookDamage);
    }
    private void OnDisable()
    {
        PlayerEvents.enemyDamaged-=(EnemyTookDamage);
    }

    public void EnemyTookDamage(GameObject enemy, int damageReceived)
    {
        Vector3 spawnPosition = Camera.main.WorldToScreenPoint(enemy.transform.position);
        spawnPosition.y += 100f;
        TMP_Text tmpText = Instantiate(damageTextPrefab, spawnPosition, Quaternion.identity, gameCanvas.transform)
            .GetComponent<TMP_Text>();

        tmpText.text = damageReceived.ToString();
    }
}
