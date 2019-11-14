using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VIDE_Data;

public class Flip : MonoBehaviour
{
    Rigidbody2D body;
    SpriteRenderer playerSprite;
    float horizontal;
    float vertical;
    public bool flipSprite = true;

    void Start()
    {
        body = GetComponent<Rigidbody2D>();
        playerSprite = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (VD.isActive)
        {
            return;
        }

        horizontal = Input.GetAxisRaw("Horizontal");
        vertical = Input.GetAxisRaw("Vertical");
        if (horizontal == -1)
        {
            playerSprite.flipX = flipSprite;
        }
        else if (horizontal == 1)
        {
            playerSprite.flipX = !flipSprite;
        }
    }
}
