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

    private void Awake()
    {
        _random = new RandomExtensions();
        _spawner = GameObject.FindWithTag("Utilities").GetComponent<PrefabsSpawner>();
    }

    private void Start()
    {
        Spawn();
    }

    private void Spawn()
    {
        //TODO: сделать, чтобы при уничтожении спавнера он переставал спавнить
        _spawner.Spawn("Enemy", transform.position + _random.GetRandomVector(MinSpawnRadius, MaxSpawnRadius), _random.GetFloat());
        ActionPlanner.PostponeAnAction(Spawn, (int) (SpawnDelay + 2 * (_random.GetFloat() - 0.5f) * DelayDeviation * SpawnDelay));
    }
}