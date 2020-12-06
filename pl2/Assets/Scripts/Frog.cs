﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Frog : Enemy
{
    [SerializeField] private float leftCap;
    [SerializeField] private float rightCap;

    [SerializeField] private float jumpLength = 4f;
    [SerializeField] private float jumpHeihgt = 5f;

    [SerializeField] private LayerMask ground;

    private Rigidbody2D rb;
    private Collider2D coll;
    private bool facingLeft = true;


    protected override void Start()
    {
        base.Start();
        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<Collider2D>();
    }

    private void Update()
    {
        if (anim.GetBool("Jumping"))
        {
            if (rb.velocity.y < .1)
            {
                anim.SetBool("Falling", true);
                anim.SetBool("Jumping", false);
            }
        }
        if (coll.IsTouchingLayers(ground) && anim.GetBool("Falling"))
        {
            anim.SetBool("Falling", false);
        }

    }

    private void Movement()
    {
        if (facingLeft)
        {
            if (transform.position.x > leftCap)
            {
                //make sure sprite is fasing right location, if not then make it face it
                if (transform.localScale.x != 1)
                {
                    transform.localScale = new Vector3(1, 1);
                }
                if (coll.IsTouchingLayers(ground))
                {
                    //jump
                    rb.velocity = new Vector2(-jumpLength, jumpHeihgt);
                    anim.SetBool("Jumping", true);
                }

            }
            else
            {
                facingLeft = false;
            }
        }
        else
        {
            if (transform.position.x < rightCap)
            {
                //make sure sprite is fasing right location, if not then make it face it
                if (transform.localScale.x != -1)
                {
                    transform.localScale = new Vector3(-1, 1);
                }
                if (coll.IsTouchingLayers(ground))
                {
                    //jump
                    rb.velocity = new Vector2(jumpLength, jumpHeihgt);
                    anim.SetBool("Jumping", true);
                }

            }
            else
            {
                facingLeft = true;
            }
        }
    }

}
