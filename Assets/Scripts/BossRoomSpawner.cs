using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BossRoomSpawner : MonoBehaviour
{
    public GameObject deathBringer;
    private int dBSpawnCount;
    public GameObject Door;

    private void Start()
    {
        deathBringer.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player")&& dBSpawnCount == 0) //플레이어가 입장하면 보스 활성화
        {
            deathBringer.SetActive(true);
            dBSpawnCount++;
            

        }
    }
}
