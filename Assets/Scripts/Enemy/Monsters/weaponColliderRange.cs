using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class weaponColliderRange : MonoBehaviour
{
    public int damage = 10;
    public int test = 0;
    private GameManager gameManager;
    private Particle particle;
    private LastPlayerController lastPlayerController;
    private PlayerAttack playerAttack;


    void Start()
    {
        gameManager = GameManager.Instance;
        lastPlayerController = gameManager.lastPlayerController;
        playerAttack = gameManager.playerAttack;
        particle = GameManager.Instance.GetComponent<Particle>();
        
    }

    void Update()
    {
      playerAttack.CheckDeffense();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
            {
                if (gameManager != null && gameManager.playerStats != null)
                {
                    if (playerAttack.isParrying) //패링 일때 
                    {
                        particle.GuardEffect();
                        gameManager.playerStats.TakeDamage(damage *0);
                    }

                else if (playerAttack.monsterToPlayerDamage == true) //가드와 일반 상태 일때 
                    {
                    if (playerAttack.isGuarding)
                        {
                            particle.DamagedEffect();
                            gameManager.playerStats.TakeDamage(damage/2);
                        }
                    else
                        {
                            particle.DamagedEffect();
                            gameManager.playerStats.TakeDamage(damage);
                            gameManager.playerStats.ApplyPoisonStatus(5, 3, 50);
                        }
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

