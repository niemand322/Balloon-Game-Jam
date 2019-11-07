using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    Rigidbody2D body;
    SpriteRenderer playerSprite;

    float horizontal;
    float vertical;
    float moveLimiter = 0.7f;

    public float runSpeedVertical = 2f;
    public float runSpeedHorizontal = 1.4f;

    void Start()
    {
        body = GetComponent<Rigidbody2D>();
        playerSprite = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        horizontal = Input.GetAxisRaw("Horizontal");
        vertical = Input.GetAxisRaw("Vertical");
        if(horizontal == -1)
        {
            playerSprite.flipX = true;
        }
        else if(horizontal == 1)
        {
            playerSprite.flipX = false;
        }
    }

    void FixedUpdate()
    {
        if (horizontal != 0 && vertical != 0)
        {
            horizontal *= moveLimiter;
            vertical *= moveLimiter;
        }

        body.velocity = new Vector2(horizontal * runSpeedHorizontal, vertical * runSpeedVertical);
    }
}
