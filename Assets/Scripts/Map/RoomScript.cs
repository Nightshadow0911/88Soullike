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
            hasPlayerEntered = true;
            
            switch (roomNumber)
            {
                case 1:
                    RoomManager.Instance.ActivateRooms(roomNumber, roomNumber,2, 13, 14);
                    break;
                case 2:
                    RoomManager.Instance.ActivateRooms(roomNumber, roomNumber, 1, 3, 13);
                    break;
                case 3:
                    RoomManager.Instance.ActivateRooms(roomNumber, roomNumber, 2, 4, 5, 13);
                    break;
                case 4:
                    RoomManager.Instance.ActivateRooms(roomNumber, roomNumber, 3, 5, 8, 18, 20);
                    break;
                case 5:
                    RoomManager.Instance.ActivateRooms(roomNumber, roomNumber, 3, 4, 6, 7, 8);
                    break;
                case 6:
                    RoomManager.Instance.ActivateRooms(roomNumber, roomNumber, 5, 7);
                    break;
                case 7:
                    RoomManager.Instance.ActivateRooms(roomNumber, roomNumber, 5, 6, 8, 9);
                    break;
                case 8:
                    RoomManager.Instance.ActivateRooms(roomNumber, roomNumber, 4, 5, 7, 9);
                    break;
                case 9:
                    RoomManager.Instance.ActivateRooms(roomNumber, roomNumber, 7, 8, 10, 19, 20);
                    break;
                case 10:
                    RoomManager.Instance.ActivateRooms(roomNumber, roomNumber, 11, 12);
                    break;
                case 11:
                    RoomManager.Instance.ActivateRooms(roomNumber, roomNumber, 10, 12);
                    break;
                case 12:
                    RoomManager.Instance.ActivateRooms(roomNumber, roomNumber, 11);
                    break;
                case 13:
                    RoomManager.Instance.ActivateRooms(roomNumber, roomNumber, 1, 2, 3, 14, 15, 16, 17);
                    break;
                case 14:
                    RoomManager.Instance.ActivateRooms(roomNumber, roomNumber, 1, 13, 15);
                    break;
                case 15:
                    RoomManager.Instance.ActivateRooms(roomNumber, roomNumber, 13, 14, 16);
                    break;
                case 16:
                    RoomManager.Instance.ActivateRooms(roomNumber, roomNumber, 13, 15, 17);
                    break;
                case 17:
                    RoomManager.Instance.ActivateRooms(roomNumber, roomNumber, 16, 18);
                    break;
                case 18:
                    RoomManager.Instance.ActivateRooms(roomNumber, roomNumber, 4, 17, 19, 20);
                    break;
                case 19:
                    RoomManager.Instance.ActivateRooms(roomNumber, roomNumber, 9, 18, 20);
                    break;
                case 20:
                    RoomManager.Instance.ActivateRooms(roomNumber, roomNumber, 4, 9, 18, 19);
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
