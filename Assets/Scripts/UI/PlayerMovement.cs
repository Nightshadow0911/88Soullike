using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f; // 플레이어 이동 속도

    void Update()
    {
        // 키 입력을 받아서 플레이어를 움직입니다.
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        Vector3 moveDirection = new Vector3(horizontalInput, 0f, verticalInput).normalized;
        Vector3 moveAmount = moveDirection * moveSpeed * Time.deltaTime;

        transform.Translate(moveAmount);
    }
}