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
    [SerializeField] public double MinDistanceToSpawn;
    [SerializeField] public int EnemyCountToSpawn;
    
    private GameObserver _observer;
    private RandomExtensions _random;
    private PrefabsSpawner _spawner;
    private bool _stopFlag;

    private void Awake()
    {
        _observer = GameObject.Find("Utilities").GetComponent<GameObserver>();
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
        if (_stopFlag) return;

        if ((MinDistanceToSpawn < 1e-9 || GetDistanceToPlayer() < MinDistanceToSpawn) && (EnemyCountToSpawn--) > 0)
            _spawner.Spawn("Enemy", transform.position + _random.GetRandomVector(MinSpawnRadius, MaxSpawnRadius), _random.GetFloat());
        
        this.StartCoroutine(Spawn, (int) (SpawnDelay + 2 * (_random.GetFloat() - 0.5f) * DelayDeviation * SpawnDelay));
    }
    
    private double GetDistanceToPlayer()
    {
        var p1 = _observer.Player.transform.position;
        var p2 = gameObject.transform.position;
        return Math.Sqrt((p1.x - p2.x) * (p1.x - p2.x) + (p1.y - p2.y) * (p1.y - p2.y));
    }
}