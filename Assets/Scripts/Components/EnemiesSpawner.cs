using System;
using System.Collections.Generic;
using UnityEngine;

public class EnemiesSpawner : MonoBehaviour
{
    private RandomExtensions _random;
    private TemplatesSpawner _spawner;
    private int _frameNumber;

    private void Awake()
    {
        _frameNumber = 0;
        _random = new RandomExtensions();
        _spawner = GameObject.FindWithTag("TemplatesSpawner").GetComponent<TemplatesSpawner>();
    }

    private void FixedUpdate()
    {
        _frameNumber++;
        if (_frameNumber >= 100)
            _frameNumber %= 100;
        
        if (_frameNumber != 0)
            return;
        
        _spawner.Spawn("Enemy", transform.position + _random.GetRandomVector(3), _random.GetFloat());
    }
}