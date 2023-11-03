using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorRemove : MonoBehaviour
{
    public GameObject removeDoor;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0)) // 좌클릭 확인
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            // Ray가 "Player" 태그를 가진 오브젝트와 "A" 오브젝트와 충돌한 경우
            if (Physics.Raycast(ray, out hit) && (hit.collider.CompareTag("Player") || hit.collider.CompareTag("A")))
            {
                Debug.Log("11");
                // "B" 오브젝트를 제거
                if (removeDoor != null)
                {
                    Destroy(removeDoor);
                }
            }
        }
    }
}