using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.InputSystem.HID;

public class MapManager : MonoBehaviour
{
    private LayerMask roomLayerMask;
    private float raycastDistance = 10f;
    public List<GameObject> roomNames;
    public GameObject playerTransform;
    
    void Update()
    {
        Camera mainCamera = Camera.main;
        Ray ray = new Ray(mainCamera.transform.position, mainCamera.transform.forward);
        RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, raycastDistance);
        Debug.DrawRay(ray.origin, ray.direction*raycastDistance, Color.red);
        
        if (hit.collider != null)
        {
            Debug.Log("11");
            GameObject hitRoom = hit.collider.gameObject;
            string roomName = GetRoomName(hitRoom);
            Debug.Log("11 : " + roomName);
            RoomSwitcher(roomName);
            
        }
    }
    void AbleRooms(string roomName)
    {
        GameObject room = roomNames.Find(r => GetRoomName(r) == roomName);
        if (roomName != roomName)
        {
            room.SetActive(true);
        }
    }

    void DisableRooms(string roomName)
    {
        GameObject room = roomNames.Find(r => GetRoomName(r) == roomName);
        if (roomName != roomName)
        {
            room.SetActive(false);
        }
    }

    string GetRoomName(GameObject room)
    {
        return room.name;
    }

    void RoomSwitcher(string roomName)
    {
        string roomName1;
        string roomName2;
        switch (roomName)
        {
            case "Room_01_01":
                roomName1 = "Room_01_02";
                roomName2 = "Room_01(0102)_03";
                AbleRooms(roomName1);
                DisableRooms(roomName2);
                break;
        }
    }
    
}
