using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenMapDoor : BaseGimmick
{
    public List<GameObject> MovedDoors;
    private Coroutine currentCoroutine;
    public List<Collider2D> DoorCollider;
    public List<Rigidbody2D> DoorRigidBody;
    
    protected override void Start()
    {
        DoorCollider = new List<Collider2D>();
        foreach (var moveDoor in MovedDoors)
        {
            Collider2D doorCollider = moveDoor.GetComponent<Collider2D>();
            DoorCollider.Add((doorCollider));
        }
        base.Start();
    }
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            bool isCollision = mapGimmickInteraction.CollisionChecktoTagBased("Player", transform.position);
            Debug.Log("문열림");
            if (isCollision)
            {
                
                currentCoroutine = StartCoroutine(OpenTheDoorCoroutine());
            }
        }
    }

    private IEnumerator OpenTheDoorCoroutine()
    {
        for (int i = 0; i < MovedDoors.Count; i++)
        {
            GameObject moveableObject = MovedDoors[i];
            Collider2D doorCollider = DoorCollider[i];
            Rigidbody2D doorRigidbody = DoorRigidBody[i];
            if (moveableObject != null)
            {
                doorRigidbody.bodyType = RigidbodyType2D.Dynamic;
                mapGimmickAction.MoveInDirection(moveableObject.transform, Vector2.up, 1.0f);
                mapGimmickAction.ToggleCollider(doorCollider, false);
                yield return StartCoroutine(mapGimmickAction.ProcessDelay(3));
                mapGimmickAction.ToggleObjectSetActive(moveableObject, false);
                
            }
        }
        currentCoroutine = null;
    }
}
