using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class BackgroundGenerator : MonoBehaviour
{
    [SerializeField] private GameObject player;
    private PrefabsSpawner _spawner;
    private List<Vector3> _backgroundPositions;
    private const int BackgroundSize = 30;

    private void Start()
    {
        _backgroundPositions = new List<Vector3>();
        _spawner = GameObject.FindWithTag("Utilities").GetComponent<PrefabsSpawner>();
    }

    void Update()
    {
        var position = player.transform.position;
        
        foreach (var newPosition in CreatePositions((int) Math.Round(position.x), (int) Math.Round(position.y)))
        {
            if (!_backgroundPositions.Contains(newPosition))
            {
                _spawner.Spawn("BackgroundImage", newPosition, 0);
                _backgroundPositions.Add(newPosition);
            };
        }
    }

    private static IEnumerable<Vector3> CreatePositions(int x0, int y0)
    {
        for (int k1 = -1; k1 < 2; k1++)
            for (int k2 = -1; k2 < 2; k2++)
            {
                var (x, y) = CreateCoords(x0, y0, k1, k2);
                yield return new Vector3(x, y, 500);
            }
    }

    private static Tuple<int, int> CreateCoords(int x, int y, int k1, int k2)
    {
        return Tuple.Create(
            Math.Sign(x)*(Math.Abs(x) / BackgroundSize + k1) * BackgroundSize,
            Math.Sign(y)*(Math.Abs(y) / BackgroundSize + k2) * BackgroundSize
            );
    }
}