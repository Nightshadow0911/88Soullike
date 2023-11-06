using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorRemove : MonoBehaviour
{
    public GameObject player; 
    public List<GameObject> removeDoors;
    public float attackDistance = 2.0f;

    private void Update()
    {
        float distance = Vector3.Distance(player.transform.position, transform.position);

        if (distance <= attackDistance) 
        {
            if (Input.GetMouseButtonDown(0)) 
            {
                foreach (var door in removeDoors)
                {
                    if (door != null)
                    {
                        door.SetActive(false);
                    }
                }
            }
        }
    }
}