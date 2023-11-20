using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class RoomScript : MonoBehaviour, IRoomObserver
{
    public int roomNumber;
    public int[] connectedRooms;
    private bool hasPlayerEntered = false;

    private void Start()
    {
        RoomManager roomManager = RoomManager.Instance;
        roomManager.RegisterObserver(this);
    }

    public void OnPlayerEnteredRoom(int playerRoom)
    {
        if (!hasPlayerEntered && playerRoom == roomNumber)
        {
            Debug.Log($"Room {roomNumber} - Player entered room {playerRoom}");
            hasPlayerEntered = true;
            
            switch (roomNumber)
            {
                case 1:
                    Debug.Log("room1="+roomNumber);
                    RoomManager.Instance.ActivateRooms(roomNumber, 1,2, 11, 12);
                    break;
                case 2:
                    Debug.Log("room2="+roomNumber);
                    RoomManager.Instance.ActivateRooms(roomNumber, 1, 2, 11, 3);
                    break;
                // 다른 케이스도 필요에 따라 추가
                default:

                    break;
            }
            hasPlayerEntered = true;
        }
        else if (hasPlayerEntered && playerRoom != roomNumber)
        {
            hasPlayerEntered = false;
        }
    }
   
    
}
