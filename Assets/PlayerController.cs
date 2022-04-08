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
		var rotation = Quaternion.AngleAxis(Input.GetAxis("Horizontal"), Vector3.back);

		rocketDirection = rotation * rocketDirection;
		transform.rotation *= rotation;
	}

	private void Slow()
	{
		spaceship.velocity -= (spaceship.velocity / 100);
	}

	private void Boost()
	{
		var velocity = spaceship.GetPointVelocity(spaceship.velocity);
		var v = (velocity.x * velocity.x + velocity.y + velocity.y);

		if (v < 2)
			spaceship.AddForce(Input.GetAxis("Vertical") * 0.01f * rocketDirection, ForceMode2D.Impulse);
	}
}
