using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenMapDoor : BaseGimmick
{
    public List<GameObject> MovedDoors;
    private Coroutine currentCoroutine;
    public List<Collider2D> DoorCollider;
    public List<Rigidbody2D> DoorRigidBody;
    public SpriteRenderer Img_Render;
    public Sprite Sprite01;

    [SerializeField] private CinemachineVirtualCamera cameraOne;
    [SerializeField] private CinemachineVirtualCamera cameraTwo;
    [SerializeField] private float waitTime;

    private bool isWork = false;
 
    
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
        if (Input.GetKeyDown(KeyCode.E) && !isWork)
        {
            OpenDoor();
        }
    }

    private IEnumerator OpenTheDoorCoroutine()
    {
        List<Coroutine> moveCoroutines = new List<Coroutine>();

        for (int i = 0; i < MovedDoors.Count; i++)
        {
            GameObject moveableObject = MovedDoors[i];
            Collider2D doorCollider = DoorCollider[i];
            Rigidbody2D doorRigidbody = DoorRigidBody[i];

            if (moveableObject != null)
            {
                Coroutine moveCoroutine = StartCoroutine(MoveDoorCoroutine(moveableObject, doorCollider, doorRigidbody));
                moveCoroutines.Add(moveCoroutine);
            }
        }
        
        foreach (Coroutine coroutine in moveCoroutines)
        {
            yield return coroutine;
        }
        currentCoroutine = null;
    }
    private IEnumerator MoveDoorCoroutine(GameObject moveableObject, Collider2D doorCollider, Rigidbody2D doorRigidbody)
    {
        doorRigidbody.bodyType = RigidbodyType2D.Dynamic;
        mapGimmickAction.MoveInDirection(moveableObject.transform, Vector2.up, 1.0f);
        mapGimmickAction.ToggleCollider(doorCollider, false);
        yield return StartCoroutine(mapGimmickAction.ProcessDelay(3));
        mapGimmickAction.ToggleObjectSetActive(moveableObject, false);
    }

    private void OpenDoor()
    {
        CameraManager.instance.CutSenceCamera(cameraOne, cameraTwo, waitTime);
        Img_Render.sprite = Sprite01;
        bool isCollision = mapGimmickInteraction.CollisionChecktoTagBased("Player", transform.position);
        if (isCollision)
        {
            currentCoroutine = StartCoroutine(OpenTheDoorCoroutine());
        }

        isWork = true;
    }
}
