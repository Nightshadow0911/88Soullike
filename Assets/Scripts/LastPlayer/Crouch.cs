using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crouch : MonoBehaviour
{
    private float yInput;
    private Vector2 normalHeight;
    public float crouchHeight;
    public LastPlayerController LastPlayerController;
    private GameManager gameManager;
    // Start is called before the first frame update
    void Start()
    {
        normalHeight = transform.localScale;
        gameManager = GameManager.Instance;
    }

    // Update is called once per frame
    void Update()
    {
        Sit();
    }

    private void Sit()
    {
        yInput = Input.GetAxisRaw("Vertical");
        if (yInput < 0 && LastPlayerController.isGrounded && Input.GetKey(KeyCode.S))
        {
            if (transform.localScale.y != crouchHeight)
            {
                transform.localScale = new Vector2(normalHeight.x, crouchHeight);
                LastPlayerController.isSitting =true;
                gameManager.playerStats.ExtraCharacterSpeed=1.5f;
                Debug.Log(" gameManager.playerStats.ExtraCharacterSpeed :" + gameManager.playerStats.ExtraCharacterSpeed);
            }
        }
        else
        {
            // 키가 눌려있지 않거나, 땐 경우에는 원래 높이로 돌아감
            if (LastPlayerController.isCeilDetected==false)
            {
                if (transform.localScale.y != normalHeight.y)
                {
                    Debug.Log("3");
                    LastPlayerController.isSitting = false;
                    LastPlayerController.canDash = true;
                    transform.localScale = normalHeight;
                    gameManager.playerStats.CharacterSpeed = gameManager.playerStats.CharacterSpeed;
                }

            }
        }
    }
}
