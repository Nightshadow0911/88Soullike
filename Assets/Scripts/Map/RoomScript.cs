using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomScript : MonoBehaviour, IRoomObserver
{
    public int roomNumber;
    public int[] connectedRooms;
    public GameObject startRoom;

    private void Start()
    {
        GameManager.instance.PlayerDeath += StartRoomCreate;
        RoomManager roomManager = RoomManager.Instance;
        roomManager.RegisterObserver(this);
    }

    public void OnPlayerEnteredRoom(int playerRoom)
    {
        if (playerRoom == roomNumber)
        {
            RoomManager.Instance.ActivateRooms(roomNumber, connectedRooms);
        }
    }

    public void StartRoomCreate()
    {
        if (!startRoom.activeSelf)
            startRoom.SetActive(true);
    }
}

