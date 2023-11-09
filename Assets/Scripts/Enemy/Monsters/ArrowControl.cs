using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowControl : MonoBehaviour
{
    public int damage = 10;

    private GameManager gameManager;
    private StatusEffectsManager playerStatusManager;
    
    void Start()
    {
        gameManager = GameManager.Instance;
    }

    void Update()
    {
        transform.right = GetComponent<Rigidbody2D>().velocity;
        playerStatusManager = FindObjectOfType<StatusEffectsManager>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) //������ ���� �߰�
            
        {
            if (gameManager != null && gameManager.playerStats != null)
            {
                gameManager.playerStats.TakeDamage(damage);
                playerStatusManager.ApplyBleedingEffect(100);
                Debug.Log("�÷��̾� Ȱ ����");
            }
            else
            {
                Debug.LogError("GameManager �Ǵ� playerStats�� ã�� �� �����ϴ�.");
            }
            
            
        }
        else if (collision.CompareTag("Ground"))
        {
            Destroy(gameObject);
        }
        else if (collision.CompareTag("Wall"))
        {
            Destroy(gameObject);
        }
    }
}