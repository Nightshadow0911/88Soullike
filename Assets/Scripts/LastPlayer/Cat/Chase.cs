using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chase : MonoBehaviour
{
    public Transform player;
    private Animator anim;
    private Rigidbody2D rb;
    private float moveSpeed = 3f;
    private LastPlayerController lastPlayerController;
    private bool canWalk = true;
    private bool canRun;

    void Start()
    {
        lastPlayerController = FindObjectOfType<LastPlayerController>();
    }
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        player = GameManager.Instance.player.transform;
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        CatFaceWay();
        AnimatorController();

    }
    private void Move()
    {
        // 플레이어와 적 레이어 간의 충돌을 무시
        Physics2D.IgnoreLayerCollision(gameObject.layer, LayerMask.NameToLayer("Enemy"));
        Physics2D.IgnoreLayerCollision(gameObject.layer, LayerMask.NameToLayer("Player"));

        // 플레이어와 고양이 사이의 방향 벡터 계산
        Vector2 direction = player.position - transform.position;
        if (lastPlayerController.isGrounded==true)
        {
            // 플레이어와 고양이의 y축 거리가 -5에서 10 사이인 경우
            if (-5 < direction.y && direction.y < 10)
            {
                // x축 거리가 17 이상이면 순간이동
                if (Mathf.Abs(direction.x) >= 17)
                {
                    transform.position = player.position;
                }
                else if (Mathf.Abs(direction.x) >= 4)
                {
                    // 플레이어와의 거리가 4 이상이면 뛰기
                    MoveWithSpeed(5);
                    canWalk = false;
                    canRun = true;
                }
                else if (Mathf.Abs(direction.x) > 1.5 && Mathf.Abs(direction.x) <= 4)
                {
                    // 거리가 2에서 4 사이이면 걷기
                    MoveWithSpeed(2);
                    canWalk = true;
                    canRun = false;
                }
                else
                {
                    // 그 외의 경우는 정지
                    MoveWithSpeed(0);
                    canWalk = false;
                    canRun = false;
                }
            }
            else
            {
                // y축 거리가 범위를 벗어난 경우 순간이동
                transform.position = player.position;
            }
        }
        else
        {
            MoveWithSpeed(0);
            canWalk = false;
            canRun = false;
        }

    }

    // 이동 속도에 따라 고양이 이동을 처리하는 함수
    private void MoveWithSpeed(float speed)
    {
        canRun = speed > 0;
        canWalk = speed > 0;
        moveSpeed = speed;

        // 이동 방향을 정규화하여 이동
        Vector2 moveDirection = (player.position - transform.position).normalized;
        transform.Translate(moveDirection * moveSpeed * Time.deltaTime);
    }



    void CatFaceWay()
    {
        Vector2 direction = player.position - transform.position;

        if (direction.x < 0)
        {
            transform.localScale = new Vector3(-1.3f, 1.3f, 1);
        }
        else
        {
            transform.localScale = new Vector3(1.3f, 1.3f, 1);
        }
    }
    public void AnimatorController()
    {
        anim.SetBool("canWalk", canWalk);
        anim.SetBool("canRun", canRun);
    }
}
