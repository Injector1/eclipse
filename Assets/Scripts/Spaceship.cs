using UnityEngine;
using System;
using System.Threading.Tasks;

public class Spaceship : MonoBehaviour
{
    [SerializeField] private Vector3 rocketDirection = new Vector3(0, 1, 0);
    [SerializeField] private Rigidbody2D spaceship;
    [SerializeField] private SpriteRenderer sprite;
    [SerializeField] private SpaceshipController controller;
    
    public Animator animator;
    public SpaceshipStates State;

    public Action OnIdle;
    public Action<Vector3> OnShoot;
    public Action<float> OnRotate;
    public Action<float> OnForwardBoost;
    public Action<float> OnDecelerating;
    public int health = 100;
    public HealthIndicator healthIndicator;

    private void Awake()
    {
        spaceship = GetComponent<Rigidbody2D>();
        sprite = GetComponentInChildren<SpriteRenderer>();
        animator = GetComponent<Animator>();
        OnRotate += ChangeDirection;
        OnForwardBoost += Boost;
        OnDecelerating += Slow;
        healthIndicator.InitializeHealthBar(health);
    }

    private void FixedUpdate()
    {
        controller.OnTick();
    }

    private void ChangeDirection(float axis)
    {
        var rotation = Quaternion.AngleAxis(4 * axis, Vector3.back);
        rocketDirection = rotation * rocketDirection;
        transform.rotation *= rotation;
    }
    
    private void Slow(float axis)
    {
        TakeDamage(1); //just testing for decreasing health
        var velocity = spaceship.velocity;
        velocity -= axis * velocity / 100;
        spaceship.velocity = velocity;
    }

    private void Boost(float axis)
    {
        var f = axis * 0.05f * rocketDirection;
        var newForce = new Vector2(f.x, f.y);
        spaceship.AddForce(newForce, ForceMode2D.Impulse);
    }

    private void TakeDamage(int damage)
    {
        health -= damage;
        healthIndicator.SetHealth(health);
    }
}
