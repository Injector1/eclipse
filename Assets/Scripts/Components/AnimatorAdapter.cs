using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class AnimatorAdapter : MonoBehaviour
{
    private Actions _actions;
    private Animator _animator;
    private readonly string[] AnimationStates = new[] {"isMoving", "isShooting", "isDead"};
    private readonly Dictionary<string, DateTime> LastUpdated = new Dictionary<string, DateTime>();

    private void Awake()
    {
        _actions = GetComponent<Actions>();
        _animator = GetComponent<Animator>();
    }

    private void Start()
    {
        _actions.OnBoost += _ => StartAnimation("isMoving", 100);
        _actions.OnShoot += _ => StartAnimation("isShooting", 100);
        _actions.OnDeath += () => { StopAllAnimations(); StartAnimation("isDead"); };
    }

    public void StopAllAnimations()
    {
        foreach (var state in AnimationStates)
            StopAnimation(state);
    }
    
    public void StartAnimation(string state, int durationInMs)
    {
        StartAnimation(state);
        EventPlanner.PostponeAnEvent(() =>
        {
            if (LastUpdated[state].AddMilliseconds(durationInMs) <= DateTime.Now)
                StopAnimation(state);
        }, durationInMs);
    }
    
    public void StartAnimation(string state)
    {
        LastUpdated[state] = DateTime.Now;
        _animator.SetBool(state, true);
    }
    
    public void StopAnimation(string state)
    {
        _animator.SetBool(state, false);
    }
}
