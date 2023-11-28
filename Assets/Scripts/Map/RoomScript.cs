using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomScript : MonoBehaviour, IRoomObserver
{
    public int roomNumber;
    public int[] connectedRooms;
    
    private void Start()
    {
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
   
    
}

