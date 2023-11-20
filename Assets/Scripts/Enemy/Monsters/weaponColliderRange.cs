using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        particle = GameManager.Instance.GetComponent<Particle>();
    }

    void Update()
    {
      lastPlayerController.CheckInput();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
            {
                if (gameManager != null && gameManager.playerStats != null)
                {
                    if (playerAttack.canTakeDamage==false)
                    {
                    particle.GuardEffect();
                    Debug.Log("Block");
                    gameManager.playerStats.TakeDamage(test);
                    }
                    else if (playerAttack.canTakeDamage == true)
                    {
                    particle.DamagedEffect(); //�ǰ� ����Ʈ ����
                    Debug.Log("Hit");
                     Debug.Log("MonsterToPlayerAttack");
                     gameManager.playerStats.TakeDamage(damage);
                     gameManager.playerStats.ApplyPoisonStatus(5, 3, 50);
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

