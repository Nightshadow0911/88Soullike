using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomTrigger : MonoBehaviour
{
    public int roomNumber;
    private bool hasEntered = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !hasEntered)
        {
            RoomManager.Instance.PlayerEnteredRoom(roomNumber);
            hasEntered = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player") && hasEntered)
        {
            hasEntered = false;
        }
    }
}
