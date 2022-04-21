using System;
using System.Collections.Generic;
using UnityEngine;

public class InitialCharacteristics : MonoBehaviour
{
    [SerializeField] private Vector3 Velocity = Vector3.zero;
    [SerializeField] private float AngularVelocity = 0;
    private new Rigidbody2D rigidbody;

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        rigidbody.velocity = Velocity;
        rigidbody.angularVelocity = AngularVelocity;
    }
}