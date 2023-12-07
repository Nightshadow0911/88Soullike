using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RoomManager : MonoBehaviour
{
    private static RoomManager instance;
    private int maxPlayerEnteredRoomCalls = 100;  
    private List<IRoomObserver> observers = new List<IRoomObserver>();
                                                     
    [SerializeField]
    public List<RoomScript> rooms = new List<RoomScript>();
    

    public static RoomManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<RoomManager>();
                if (instance == null)
                {
                    GameObject managerObject = new GameObject("RoomManager");
                    instance = managerObject.AddComponent<RoomManager>();
                }
            }
            return instance;
        }
    }
   


    public void RegisterObserver(IRoomObserver observer)
    {
        if (!observers.Contains(observer))
        {
            observers.Add(observer);
        }
    }

    public void UnregisterObserver(IRoomObserver observer)
    {
        if (observers.Contains(observer))
        {
            observers.Remove(observer);
        }
    }

    public void PlayerEnteredRoom(int roomNumber, int depth = 0)
    {
        if (depth > maxPlayerEnteredRoomCalls)
        {
            Debug.LogError("PlayerEnteredRoom calls exceeded the limit. Aborting.");
            return;
        }

        foreach (var observer in observers)
        {
            observer.OnPlayerEnteredRoom(roomNumber);
        }
    }
    public void ActivateRooms(int roomNumber, params int[] roomIndexesToActivate)
    {
        // 전체 방 비활성화
        foreach (var room in rooms)
        {
            if (!roomIndexesToActivate.Contains(room.roomNumber))
            {
                room.gameObject.SetActive(false);
            }
        }

        // 특정 방들 활성화
        foreach (var index in roomIndexesToActivate)
        {
            int actualIndex = index - 1;
            if (actualIndex >= 0 && actualIndex < rooms.Count)
            {
                rooms[actualIndex].gameObject.SetActive(true);
            }
        }
    }
    
}

public interface IRoomObserver
{
    void OnPlayerEnteredRoom(int roomNumber);
}



