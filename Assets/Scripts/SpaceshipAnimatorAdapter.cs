using System;
using UnityEngine;

public class SpaceshipAnimatorAdapter : MonoBehaviour
{
    [SerializeField] private Spaceship spaceship;
    private static readonly int IsMoving = Animator.StringToHash("isMoving");
    private static readonly int IsShooting = Animator.StringToHash("isShooting");
    private static readonly int IsIdle = Animator.StringToHash("isIdle");

    private void Awake()
    {
        spaceship.OnForwardBoost += f => spaceship.State = SpaceshipStates.ForwardBoost;
        spaceship.OnRotate += f => spaceship.State = SpaceshipStates.Rotating;
        spaceship.OnDecelerating += f => spaceship.State = SpaceshipStates.Decelerating;
        spaceship.OnShoot += v => spaceship.State = SpaceshipStates.Shooting;
        spaceship.OnIdle += () => spaceship.State = SpaceshipStates.Idle;
    }

    private void Update()
    {
        switch (spaceship.State)
        {
            //case states
        }
    }
}
