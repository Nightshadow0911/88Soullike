using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowControl : MonoBehaviour
{
    public int damage = 10; // 화살이 입힐 데미지 값입니다.

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        transform.right = GetComponent<Rigidbody2D>().velocity;
    }

    // 화살이 다른 오브젝트와 충돌했을 때 호출되는 메서드입니다.
    void OnCollisionEnter2D(Collision2D collision)
    {
        // 충돌한 오브젝트가 "Player" 태그가 있는지 확인합니다.
        if (collision.gameObject.CompareTag("Player"))
        {
            // 플레이어가 어떤 형태의 체력 시스템을 가지고 있는지 가정합니다.
            //PlayerHealth playerHealth = collision.gameObject.GetComponent<PlayerHealth>();

            //if (playerHealth != null)
            {
                // 플레이어에게 데미지를 입힙니다.
                //playerHealth.TakeDamage(damage);
            }

            // 화살을 플레이어에게 명중한 후, 화살을 파괴합니다.
            Destroy(gameObject);
        }
        else
        {
            // "Player" 이외의 다른 오브젝트에 화살이 명중하면 화살을 파괴합니다.
            Destroy(gameObject);
        }
    }
}