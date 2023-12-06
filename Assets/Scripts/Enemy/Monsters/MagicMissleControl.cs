using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicMissleControl : MonoBehaviour
{
    public int damage = 10;

    private GameManager gameManager;
    private StatusEffectsManager playerStatusManager;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameManager.instance;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))

        {
            if (gameManager != null && gameManager.playerStats != null)
            {
                gameManager.playerStats.TakeDamage(damage);
            }
        }
        else if (collision.CompareTag("Ground"))
        {
            gameObject.SetActive(false);
        }
        else if (collision.CompareTag("Wall"))
        {
            gameObject.SetActive(false);
        }
    }
}
