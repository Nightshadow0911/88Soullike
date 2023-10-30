using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class weaponColliderRange : MonoBehaviour
{
    public int damage = 10;

    private GameManager gameManager;


    void Start()
    {
        gameManager = GameManager.Instance;
    }

    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) //데미지 공식 추가

        {
            if (gameManager != null && gameManager.playerStats != null)
            {
                gameManager.playerStats.TakeDamage(damage);
            }
            else
            {
                Debug.LogError("GameManager 또는 playerStats를 찾을 수 없습니다.");
            }
        }
            Destroy(gameObject);
            Debug.Log("플레이어 공격받음");
    }
}

