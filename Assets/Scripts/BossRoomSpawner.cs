using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BossRoomSpawner : BaseGimmick
{
    public GameObject deathBringer;
    private int dBSpawnCount;
    public GameObject door1;
    public Collider2D door1Collider;
    private SpriteRenderer door1sprite;
    public GameObject door2;
    public Collider2D door2Collider;
    private SpriteRenderer door2sprite;
    private Coroutine currentCoroutine;


    protected override void Start()
    {
        door1sprite = door1.GetComponent<SpriteRenderer>();
        door2sprite = door2.GetComponent<SpriteRenderer>();
        dBSpawnCount = 0;
        deathBringer.SetActive(false);
        base.Start();
    }
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player")&& dBSpawnCount == 0) //플레이어가 입장하면 보스 활성화
        {
            deathBringer.SetActive(true);
            dBSpawnCount++;
            DoorClose();
        }
    }

    private void DoorClose()
    {
        mapGimmickAction.ToggleSpriteAndCollider(door1sprite, door1Collider , true);
        mapGimmickAction.ToggleSpriteAndCollider(door2sprite, door2Collider , true);
    }
}
