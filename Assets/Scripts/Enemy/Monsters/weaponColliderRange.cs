using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class weaponColliderRange : MonoBehaviour
{
    public int damage = 10;
    public int test = 0;
    private GameManager gameManager;
    private LastPlayerController lastPlayerController;
    public bool mtoP = true;

    void Start()
    {
        gameManager = GameManager.Instance;
    }

    void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
            {
                if (gameManager != null && gameManager.playerStats != null)
                {
                    if (mtoP)
                    {
                        Debug.Log("ffffff");
                        gameManager.playerStats.TakeDamage(test);
                    }
                    if (!mtoP)
                    {
                        Debug.Log("3");
                        gameManager.playerStats.TakeDamage(damage);
                        gameManager.playerStats.ApplyPoisonStatus(5, 3, 50);
                        Debug.Log("MonsterToPlayerAttack");
                        Destroy(gameObject);
                    }
                }
                else
                {
                    Debug.Log("4");
                    Debug.LogError("GameManagerOrPlayerStatsNOFIND.");
                }
            }
    }
    
}

