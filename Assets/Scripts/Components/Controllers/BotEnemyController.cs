using System;
using UnityEditor;
using UnityEngine;


public class BotEnemyController : MonoBehaviour
{
    private Spaceship spaceship;
    private Spaceship player;

    public void Awake()
    {
        spaceship = GetComponent<Spaceship>();
    }

    public void Start()
    {
        player = GameObject.FindWithTag("Player").GetComponent<Spaceship>();
    }
    
    public void FixedUpdate()
    {
        var playerPosition = player.Rigidbody.position;
        var spaceshipPosition = spaceship.Rigidbody.position;
        var toPlayer = playerPosition - spaceshipPosition;
        var lookVector = spaceship.transform.rotation * Vector2.up;
        spaceship.OnRotate?.Invoke(GetRotationValue(lookVector, toPlayer));
        if (toPlayer.magnitude < 5)
        {
            spaceship.OnBoost?.Invoke(0.5f);
            spaceship.OnShoot?.Invoke(toPlayer);
        }
        else
        {
            spaceship.OnBoost?.Invoke(1);
        }
    }
    
    private float GetRotationValue(Vector2 lookVector, Vector2 toObject)
    {
        var mixedMultiplication = lookVector.x * toObject.y - lookVector.y * toObject.x;
        if (Math.Abs(mixedMultiplication) < 1e-9)
            return 0;
        return mixedMultiplication < 0 ? 0.5f : -0.5f;
    }
}