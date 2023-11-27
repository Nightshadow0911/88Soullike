using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowTrapArrow : BaseGimmick
{
    private GameManager gameManager;
    private SpriteRenderer spriteRenderer;
    public Collider2D arrowCollider;
    private int moveforce = 30;
    private bool isshooting = false;
    
    private void Start()
    {
        gameManager = GameManager.Instance;
        spriteRenderer = GetComponent<SpriteRenderer>();
        arrowCollider = GetComponent<Collider2D>();
        base.Start();
    }
    
    private void Update()
    {
        mapGimmickAction.MoveInDirection(transform, Vector2.right, moveforce * Time.deltaTime);
        bool isCollision = mapGimmickInteraction.CollisionChecktoTagBased("Player", transform.position);
        if (isCollision)
        {
            TrapArrowAction();
        }
        
        bool isLayer = mapGimmickInteraction.CollisionChecktoLayerBased("Wall", transform.position);
        if (isLayer)
        {
            Destroy(gameObject);
        }

    }

    private void TrapArrowAction()
    {
        gameManager.playerStats.TakeDamage(10);
        Destroy(gameObject);
    }
}
