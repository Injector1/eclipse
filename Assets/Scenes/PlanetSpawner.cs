using System.Collections.Generic;
using UnityEngine;

public class PlanetSpawner : MonoBehaviour
{
    public GameObject[] planets;

    void Start()
    {
        // not implemented
        Instantiate(planets[0], transform.position, Quaternion.identity);
    }
}
