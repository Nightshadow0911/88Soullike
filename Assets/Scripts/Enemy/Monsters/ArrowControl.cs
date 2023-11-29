using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.Controls;
using UnityEngine.UIElements;

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
        if (collision.gameObject.CompareTag("Player")) // 레이어가 "Player"인지 확인
        {
            if (gameManager != null && gameManager.playerStats != null)
            {
                gameManager.playerStats.TakeDamage(damage);
                //playerStatusManager.ApplyBleedingEffect(100);
                Debug.Log("플레이어에게 데미지를 입혔습니다.");
                StartCoroutine(DisableArrow());
            }
            else
            {
                Debug.LogError("GameManager 또는 playerStats가 null입니다.");
            }
        }
        else if (collision.gameObject.layer == LayerMask.NameToLayer("Ground")) //땅에 부딪히면 파괴
        {
            StartCoroutine(DisableArrow());
        }
        IEnumerator DisableArrow()
        {
            
            yield return YieldCache.WaitForSeconds(3f);
            Destroy(gameObject);
        }
    }
}