using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerController : MonoBehaviour
{
    Animator animator;

    [SerializeField]
    private Vector3 rocketDirection = new Vector3(0, 1, 0);
    private Rigidbody2D spaceship;
    private SpriteRenderer sprite;

    private void Awake()
    {
        spaceship = GetComponent<Rigidbody2D>();
        sprite = GetComponentInChildren<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (Input.GetButton("Horizontal"))
        {
            animator.SetBool("isMoving", false);
            ChangeDirection();
        }

        if (Input.GetButton("Vertical"))
        {
            if (Input.GetAxis("Vertical") < 0)
            {
                //animator.SetBool("isShooting", true);
                animator.SetBool("isMoving", false);
                Slow();
            }
            else
            {
                animator.SetBool("isMoving", true);
                Boost();
            }
        }
    }

    private void ChangeDirection()
    {
        var rotation = Quaternion.AngleAxis(Input.GetAxis("Horizontal")/2, Vector3.back);
        
        rocketDirection = rotation * rocketDirection;
        transform.rotation *= rotation;
    }
    
    private void Slow()
    {
        var velocity = spaceship.velocity;
        velocity -= (velocity / 100);
        
        spaceship.velocity = velocity;
    }

    private void Boost()
    {
        var f = Input.GetAxis("Vertical") * 0.01f * rocketDirection;
        var newForce = new Vector2(f.x, f.y);

        if ((newForce + spaceship.GetPointVelocity(spaceship.velocity)).magnitude < 0.8)
        {
            spaceship.AddForce(newForce, ForceMode2D.Impulse);
        }
        else
        {
            ChangeDirection();
        }
    }
}
