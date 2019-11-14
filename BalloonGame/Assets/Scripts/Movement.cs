using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VIDE_Data;

public class Movement : MonoBehaviour
{
    Rigidbody2D body;

    float horizontal;
    float vertical;
    float moveLimiter = 0.7f;

    public float runSpeedVertical = 2f;
    public float runSpeedHorizontal = 1.4f;

    void Start()
    {
        body = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (VD.isActive)
        {
            return;
        }

        horizontal = Input.GetAxisRaw("Horizontal");
        vertical = Input.GetAxisRaw("Vertical");
    }

    void FixedUpdate()
    {
        if (VD.isActive)
        {
            body.velocity = Vector2.zero;
            return;
        }

        if (horizontal != 0 && vertical != 0)
        {
            horizontal *= moveLimiter;
            vertical *= moveLimiter;
        }

        body.velocity = new Vector2(horizontal * runSpeedHorizontal, vertical * runSpeedVertical);
    }
}
