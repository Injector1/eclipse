using UnityEngine;
using System;
using System.Collections.Generic;

public class Spaceship : MonoBehaviour
{
    public Rigidbody2D Rigidbody;
    public Action<Vector3> OnShoot;
    public Action<float> OnRotate;
    public Action<float> OnBoost;
    public Action<float> OnSlowDown;

    private void Awake()
    {
        Rigidbody = GetComponent<Rigidbody2D>();
        Modules = new Dictionary<Type, HashSet<ISpaceshipModule>>();
    }

    public Dictionary<Type, HashSet<ISpaceshipModule>> Modules;
    public void AddModule(ISpaceshipModule module, Type moduleType)
    {
        if (!Modules.TryGetValue(moduleType, out var modulesSet))
            modulesSet = new HashSet<ISpaceshipModule>();
        modulesSet.Add(module);
    }
}
