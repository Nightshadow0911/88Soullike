using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BossRoomSpawner : BaseGimmick
{
    public GameObject boss;
    private int dBSpawnCount;
    public GameObject door1;
    public Collider2D door1Collider;
    private SpriteRenderer door1sprite;
    public GameObject door2;
    public Collider2D door2Collider;
    private SpriteRenderer door2sprite;
    private Coroutine currentCoroutine;
    public GameObject bossUI;

    protected override void Start()
    {
        GameManager.instance.PlayerDeath += ResetBoss;
        door1sprite = door1.GetComponent<SpriteRenderer>();
        door2sprite = door2.GetComponent<SpriteRenderer>();
        boss.SetActive(false);
        bossUI.SetActive(false);
        base.Start();
    }

    private void Update()
    {
        if (boss == null)
        {
            mapGimmickAction.ToggleSpriteAndCollider(door1sprite, door1Collider, false);
            mapGimmickAction.ToggleSpriteAndCollider(door2sprite, door2Collider , false);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player")) //플레이어가 입장하면 보스 활성화
        {
            boss.SetActive(true);
            bossUI.SetActive(true);
            dBSpawnCount++;
            DoorClose();
        }
    }

    private void DoorClose()
    {
        mapGimmickAction.ToggleSpriteAndCollider(door1sprite, door1Collider , true);
        mapGimmickAction.ToggleSpriteAndCollider(door2sprite, door2Collider , true);
    }

    private void ResetBoss()
    {
        boss.SetActive(false);
        bossUI.SetActive(false);
        mapGimmickAction.ToggleSpriteAndCollider(door1sprite, door1Collider , false);
        mapGimmickAction.ToggleSpriteAndCollider(door2sprite, door2Collider , false);
    }
}
