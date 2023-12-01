using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chase : MonoBehaviour
{
    public Transform player;
    private Animator anim;
    private Rigidbody2D rb;
    private float moveSpeed = 3f;
    private GameManager gameManager;
    private bool facingRight;

    private bool canWalk = true;
    private bool canRun;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        player = GameManager.Instance.player.transform;
        anim= GetComponent<Animator>();

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
        Physics2D.IgnoreLayerCollision(gameObject.layer, LayerMask.NameToLayer("Enemy"));
        Physics2D.IgnoreLayerCollision(gameObject.layer, LayerMask.NameToLayer("Player"));

        Vector2 direction = player.position - transform.position;

        if (-5 < direction.y && direction.y < 10)
        {
            if (Mathf.Abs(direction.x) >= 17)
            {
                transform.position = player.position;
            }
            else if (Mathf.Abs(direction.x) >= 4)
            {
                canRun = true;
                canWalk = false;
                moveSpeed = 5; // 플레이어와 같은값 
                Vector2 moveDirection = direction.normalized;
                transform.Translate(moveDirection * moveSpeed * Time.deltaTime);
            }
            else if (Mathf.Abs(direction.x) > 2 && Mathf.Abs(direction.x) <= 4)
            {
                canRun = false;
                canWalk = true;
                moveSpeed = 2;
                Vector2 moveDirection = direction.normalized;
                transform.Translate(moveDirection * moveSpeed * Time.deltaTime);
            }
            else
            {
                moveSpeed = 0;
                canWalk = false;
                canRun = false;
                Vector2 moveDirection = direction.normalized;
                transform.Translate(moveDirection * moveSpeed * Time.deltaTime);
            }
        }
        else
        {
            transform.position = player.position;
        }
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
