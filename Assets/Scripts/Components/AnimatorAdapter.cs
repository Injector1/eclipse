using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class AnimatorAdapter : MonoBehaviour
{
    private Engine _engine;
    private Animator _animator;
    private Health _health;
    private WeaponController _weaponController;
    private readonly string[] AnimationStates = new[] {"isMoving", "isShooting", "isDead"};
    private readonly Dictionary<string, DateTime> LastUpdated = new Dictionary<string, DateTime>();

    private void Awake()
    {
        _engine = GetComponent<Engine>();
        _health = GetComponent<Health>();
        _animator = GetComponent<Animator>();
        _weaponController = GetComponentInChildren<WeaponController>();
    }

    private void Start()
    {
        _health.OnDeath += () => { StopAllAnimations(); StartAnimation("isDead"); };
        try
        {
            _engine.OnBoost += _ => StartAnimation("isMoving", 100);
            _weaponController.OnShoot += _ => StartAnimation("isShooting", 100);
        }
        catch (Exception e)
        {
            return;
        }
    }

    public void StopAllAnimations()
    {
        foreach (var state in AnimationStates)
            StopAnimation(state);
    }
    
    public void StartAnimation(string state, int durationInMs)
    {
        StartAnimation(state);
        ActionPlanner.PostponeAnAction(() =>
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
