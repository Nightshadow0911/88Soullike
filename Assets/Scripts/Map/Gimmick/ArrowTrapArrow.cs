using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowTrapArrow : BaseGimmick
{
    private GameManager gameManager;
    private SpriteRenderer spriteRenderer;
    public Collider2D arrowCollider;
    public int moveforce = 30;
    public Vector2 direction;
    public PlayerStatusHandler playerHandlerArrowTrap;
    
    
    protected override void Start()
    {
        gameManager = GameManager.instance;
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
        isLayer = mapGimmickInteraction.CollisionChecktoLayerBased("Enemy", transform.position);
        if (isLayer)
        {
            Destroy(gameObject);
        }

    }

    private void TrapArrowAction()
    {
        mapGimmickInteraction.CollisionCheckToPlayerTakeDamage("Player",transform.position, 10 );
        Destroy(gameObject);
    }
}
