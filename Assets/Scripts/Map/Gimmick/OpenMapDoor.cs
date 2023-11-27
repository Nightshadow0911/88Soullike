using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenMapDoor : BaseGimmick
{
    public GameObject MovedDoor;
    private Coroutine currentCoroutine;
    public Collider2D DoorCollider;
    
    void Start()
    {
        DoorCollider = GetComponent<Collider2D>();
        base.Start();
    }
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            bool isCollision = mapGimmickInteraction.CollisionChecktoTagBased("Player", transform.position);
            if (isCollision)
            {
                currentCoroutine = StartCoroutine(OpenTheDoorCoroutine());
            }
        }
    }

    private IEnumerator OpenTheDoorCoroutine()
    {
        GameObject moveableObject = MovedDoor;
        if (moveableObject != null)
        {
            mapGimmickAction.MoveInDirection(moveableObject.transform, Vector2.up, 1.0f);
            mapGimmickAction.ToggleCollider(DoorCollider, false);
            yield return StartCoroutine(mapGimmickAction.ProcessDelay(3));
            mapGimmickAction.ToggleObjectSetActive(MovedDoor, false);
            currentCoroutine = null;
        }
    }
}
