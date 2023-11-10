using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
/// <summary>
/// 데미지 출력
/// </summary>
public class UiManager : MonoBehaviour
{
    public GameObject damageTextPrefab;
    public GameObject healthTextPrefab;
    public Canvas gameCanvas;

    private void Awake()
    {
        gameCanvas = FindObjectOfType<Canvas>();

    }

    private void OnEnable()
    {
        PlayerEvents.playerDamaged+=(PlayerTookDamage);
        PlayerEvents.playerHealed += (PlayerHealed);
        
    }
    private void OnDisable()
    {

        PlayerEvents.playerDamaged-=(PlayerTookDamage);
        PlayerEvents.playerHealed-=(PlayerHealed);
    }

    public void PlayerTookDamage(GameObject player, int damageReceived)
    {
        Vector3 spawnPosition = Camera.main.WorldToScreenPoint(player.transform.position);
        spawnPosition.y += 100f;
        TMP_Text tmpText = Instantiate(damageTextPrefab, spawnPosition, Quaternion.identity, gameCanvas.transform)
            .GetComponent<TMP_Text>();

        tmpText.text = damageReceived.ToString();
    }
    public void PlayerHealed(GameObject player, int healthRestored)
    {
        Vector3 spawnPosition = Camera.main.WorldToScreenPoint(player.transform.position);

        TMP_Text tmpText = Instantiate(damageTextPrefab, spawnPosition, Quaternion.identity, gameCanvas.transform)
            .GetComponent<TMP_Text>();

        tmpText.text = healthRestored.ToString();
    }
}
