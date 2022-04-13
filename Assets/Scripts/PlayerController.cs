using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Threading.Tasks;

public class PlayerController : MonoBehaviour
{
    Animator animator;
    [SerializeField] private Transform gun;
    [SerializeField] private GameObject bullet;
    [SerializeField] private Vector3 rocketDirection = new Vector3(0, 1, 0);
    [SerializeField] private Rigidbody2D spaceship;
    [SerializeField] private SpriteRenderer sprite;

    private void Awake()
    {
        spaceship = GetComponent<Rigidbody2D>();
        sprite = GetComponentInChildren<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }

    async private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            animator.SetBool("isShooting", true);
            Instantiate(bullet, gun.position, transform.rotation);
            await Task.Delay(500);
            animator.SetBool("isShooting", false);
        }

        if (Input.GetButton("Horizontal"))
        {
            animator.SetBool("isMoving", false);
            ChangeDirection();
        }

        else if (Input.GetButton("Vertical"))
        {
            if (Input.GetAxis("Vertical") < 0)
            {
                animator.SetBool("isMoving", false);
                Slow();
            }
            else
            {
                animator.SetBool("isMoving", true);
                Boost();
            }
        }
        else
        {
            animator.SetBool("isMoving", false);
        }

        //animator.SetBool("isShooting", false);
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
