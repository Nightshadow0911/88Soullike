using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterKnockBack : MonoBehaviour
{
    [SerializeField]
    private float knockbackSpeedX, knockbackSpeedY,knockbackDuration, knockbackDeathSpeedX, knockbackDeathSpeedY,deathTorque;
    private float knockbackStart; 
    private bool knockback;
    private Rigidbody2D rb;

    private GameManager gameManager;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        gameManager = GameManager.Instance;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Knockback()
    {
        knockback = true;
        knockbackStart = Time.time;
        rb.velocity = new Vector2(knockbackSpeedX * gameManager.lastPlayerController.facingDirection, knockbackSpeedY);
    }
    public void CheckKnockback()
    {
        if (Time.time>= knockbackStart + knockbackDuration && knockback)
        {
            knockback = false;
            rb.velocity = new Vector2(0.0f, rb.velocity.y);
        }
    }

}
