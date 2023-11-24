using System;
using System.Collections;
using UnityEngine;

public class BrokenPlatform : MonoBehaviour
{
    private AudioSource audioSource;
    private SpriteRenderer spriteRenderer;
    public Collider2D platformCollider;
    
    private Coroutine currentCoroutine;
    
    private MapGimmickAction mapGimmickAction;
    private MapGimmickInteraction mapGimmickInteraction;

    void Start() // 컴포넌트 가져오기 나중에 스크립트들은 따로 매니저를 두는 싱글톤을 둘까 생각중
    {
        audioSource = GetComponent<AudioSource>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        platformCollider = GetComponent<Collider2D>();
        mapGimmickAction = GetComponent<MapGimmickAction>();
        mapGimmickInteraction = GetComponent<MapGimmickInteraction>();
    }

    private void Update()
    {
        bool isCollision = mapGimmickInteraction.CollisionChecktoTagBased("Player"); // MapGimmickInteraction에서 가져온 상호작용 체크
        if (isCollision && currentCoroutine == null)
        { 
            currentCoroutine = StartCoroutine(PerformCollisionAction(audioSource, spriteRenderer, platformCollider));
        }
    }

    private IEnumerator PerformCollisionAction(AudioSource audioSource, SpriteRenderer spriteRenderer, Collider2D collider2D) //MapGimmickAction에서 가져온 함수들 모음
    {
        yield return StartCoroutine(mapGimmickAction.ProcessDelay(1));
        mapGimmickAction.ToggleSpriteAndCollider(spriteRenderer, collider2D, false); 
        mapGimmickAction.PlaySound(audioSource);
        yield return StartCoroutine(mapGimmickAction.ProcessDelay(3));
        mapGimmickAction.ToggleSpriteAndCollider(spriteRenderer, collider2D, true);
        currentCoroutine = null; //Update이기때문에 강제적으로 코루틴 없애주기
    }
}