using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowControl : MonoBehaviour
{
    public int damage = 10;

    private GameManager gameManager;
    
    void Start()
    {
        gameManager = GameManager.Instance;
    }

    void Update()
    {
        transform.right = GetComponent<Rigidbody2D>().velocity;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) //데미지 공식 추가
            
        {
            if (gameManager != null && gameManager.playerStats != null)
            {
                gameManager.playerStats.TakeDamage(damage);
                Debug.Log("플레이어 활 맞음");
            }
            else
            {
                Debug.LogError("GameManager 또는 playerStats를 찾을 수 없습니다.");
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