using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class AnimatorAdapter : MonoBehaviour
{
    private Animator animator;
    private Spaceship spaceship;
    private readonly string[] AnimationStates = new[] {"isMoving", "isShooting", "isDead"};
    private readonly Dictionary<string, DateTime> LastUpdated = new Dictionary<string, DateTime>();

    private void Awake()
    {
        animator = GetComponent<Animator>();
        spaceship = GetComponent<Spaceship>();
    }

    private void Start()
    {
        spaceship.OnBoost += _ => StartAnimation("isMoving", 100);
        spaceship.OnShoot += _ => StartAnimation("isShooting", 500);
        spaceship.OnDeath += () => { StopAllAnimations(); StartAnimation("isDead"); };
    }

    public void StopAllAnimations()
    {
        foreach (var state in AnimationStates)
            StopAnimation(state);
    }
    
    async public void StartAnimation(string state, int durationInMs)
    {
        StartAnimation(state);
        await Task.Delay(durationInMs);
        try
        {
            if (LastUpdated[state].AddMilliseconds(durationInMs) <= DateTime.Now)
                StopAnimation(state);
        }
        catch { /* ignored */ }
    }
    
    public void StartAnimation(string state)
    {
        LastUpdated[state] = DateTime.Now;
        animator.SetBool(state, true);
    }
    
    public void StopAnimation(string state)
    {
        animator.SetBool(state, false);
    }
}
