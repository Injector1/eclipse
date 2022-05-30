using System;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerBody : MonoBehaviour
{
    [SerializeField] private GameObject EntityToSpawn;
    [SerializeField] private float MinSpawnRadius;
    [SerializeField] private float MaxSpawnRadius;
    [SerializeField] private float SpawnDelay;
    [SerializeField] private float DelayDeviation;
    private RandomExtensions _random;
    private PrefabsSpawner _spawner;
    private bool _stopFlag;

    private void Awake()
    {
        _random = new RandomExtensions();
        _spawner = GameObject.FindWithTag("Utilities").GetComponent<PrefabsSpawner>();
        var health = GetComponent<Health>();
        if (health != null)
            health.OnDeath += () => { _stopFlag = true; };
    }

    private void Start()
    {
        Spawn();
    }

    private void Spawn()
    {
        if (_stopFlag)
            return;
        _spawner.Spawn("Enemy", transform.position + _random.GetRandomVector(MinSpawnRadius, MaxSpawnRadius), _random.GetFloat());
        ActionPlanner.PostponeAnAction(Spawn, (int) (SpawnDelay + 2 * (_random.GetFloat() - 0.5f) * DelayDeviation * SpawnDelay));
    }
}