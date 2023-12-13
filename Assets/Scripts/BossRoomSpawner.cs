using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BossRoomSpawner : BaseGimmick
{
    public GameObject boss;
    private int dBSpawnCount = 0;
    public GameObject door1;
    public Collider2D door1Collider;
    private SpriteRenderer door1sprite;
    public GameObject door2;
    public Collider2D door2Collider;
    private SpriteRenderer door2sprite;
    private Coroutine currentCoroutine;
    public GameObject bossUI;

    public AudioClip bossClip;

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
        if (collision.gameObject.CompareTag("Player") && dBSpawnCount == 0) //�÷��̾ �����ϸ� ���� Ȱ��ȭ
        {
            boss.SetActive(true);
            bossUI.SetActive(true);
            dBSpawnCount++;
            DoorClose();

            SoundManager.instance.ChangeBGMAudio(bossClip);
        }
    }
    
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player")) //�÷��̾ �����ϸ� ���� Ȱ��ȭ
        {
            SoundManager.instance.ChangeOriginalBGMAudio();
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
        dBSpawnCount = 0;

        SoundManager.instance.ChangeOriginalBGMAudio();
    }
}
