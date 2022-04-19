using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private new Rigidbody2D rigidbody;
    [SerializeField] private int LifeTime = 5;
    [SerializeField] private float Speed = 0.001f;
    
    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        rigidbody.velocity =  Speed * (transform.rotation * Vector2.up);
        Destroy(gameObject, LifeTime);
    }
}
