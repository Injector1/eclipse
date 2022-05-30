using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameObserver : MonoBehaviour
{
    public DateTime GameStartTime;
    public int EnemyKillsCount;
    public int StationKillsCount;

    public HashSet<GameObject> Enemies;
    public HashSet<GameObject> Stations;
    public GameObject Player;

    public event Action<GameObject> OnEntityDeath;

    private void Start()
    {
        EnemyKillsCount = 0;
        StationKillsCount = 0;
        Stations = new HashSet<GameObject>(GameObject.FindGameObjectsWithTag("Station"));
        Enemies = new HashSet<GameObject>(GameObject.FindGameObjectsWithTag("Enemy"));
        Player = GameObject.FindWithTag("Player");
        GameStartTime = DateTime.Now;
        
        var spawner = GameObject.FindWithTag("Utilities").GetComponent<PrefabsSpawner>();
        spawner.OnEntitySpawn += e =>
        {
            AddEntityToObserver(e);
            if (e.TryGetComponent<Health>(out var health))
                health.OnDeath += () => RemoveEntityFromObserver(e);
        };

        foreach (var entity in Enemies.Concat(Stations))
            if (entity.TryGetComponent<Health>(out var health))
                health.OnDeath += () => HandleEntityDeath(entity);
        
    }

    private void HandleEntityDeath(GameObject entity)
    {
        if (entity.TryGetComponent<Health>(out _))
            RemoveEntityFromObserver(entity);
        OnEntityDeath?.Invoke(entity);
    }
    
    private void AddEntityToObserver(GameObject entity)
    {
        switch (entity.tag)
        {
            case "Enemy":
                Enemies.Add(entity);
                break;
            case "Station":
                Stations.Add(entity);
                break;
            case "Player":
                Player = entity;
                break;
        }
    }
    
    private void RemoveEntityFromObserver(GameObject entity)
    {
        switch (entity.tag)
        {
            case "Enemy":
                Enemies.Remove(entity);
                EnemyKillsCount++;
                break;
            case "Station":
                Stations.Remove(entity);
                StationKillsCount++;
                break;
            case "Player":
                Player = null;
                break;
        }
    }
}
