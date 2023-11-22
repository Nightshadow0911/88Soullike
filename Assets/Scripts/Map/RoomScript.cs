using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomScript : MonoBehaviour, IRoomObserver
{
    public int roomNumber;
    public int[] connectedRooms;
    private bool hasPlayerEntered = false;
    
     private Dictionary<int, int[]> roomConnections = new Dictionary<int, int[]>
    {
        {1, new[] {1, 2, 13, 14}},
        {2, new[] {1, 2, 3, 13}},
        {3, new[] {2, 3, 4, 5, 13}},
        {4, new[] {3, 4, 5, 8, 20}},
        {5, new[] {3, 4, 5, 6, 7, 8}},
        {6, new[] {5, 6, 7}},
        {7, new[] {5, 6, 7, 8, 9}},
        {8, new[] {4, 5, 7, 8, 9}},
        {9, new[] {7, 8, 9, 10, 19, 20}},
        {10, new[] {9, 10, 11, 12}},
        {11, new[] {10, 11, 12}},
        {12, new[] {11, 12}},
        {13, new[] {1, 2, 3, 13, 14, 15, 16, 17}},
        {14, new[] {1, 13, 14, 15}},
        {15, new[] {13, 14, 15, 16}},
        {16, new[] {13, 15, 16, 17}},
        {17, new[] {16, 17, 18}},
        {18, new[] {4, 17, 18, 19, 20}},
        {19, new[] {9, 18, 19, 20}},
        {20, new[] {4, 9, 18, 19, 20}}
    };

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
            RoomManager.Instance.ActivateRooms(roomNumber, roomConnections[roomNumber]);
        }
        else if (hasPlayerEntered && playerRoom != roomNumber)
        {
            hasPlayerEntered = false;
        }
    }
   
    
}
