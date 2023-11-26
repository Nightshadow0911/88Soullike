using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowTrapArrow : BaseGimmick
{
    private GameManager gameManager;
    private SpriteRenderer spriteRenderer;
    public Collider2D arrowCollider;
    
    private void Start()
    {
        gameManager = GameManager.Instance;
        spriteRenderer = GetComponent<SpriteRenderer>();
        arrowCollider = GetComponent<Collider2D>();

    }
    
    private void Update()
    {
        bool isCollision = mapGimmickInteraction.CollisionChecktoTagBased("Player");
        if (isCollision)
        {
            TrapArrowAction();
        }
    }

    private void TrapArrowAction()
    {
        gameManager.playerStats.TakeDamage(10);
        Destroy(gameObject);
    }
}
