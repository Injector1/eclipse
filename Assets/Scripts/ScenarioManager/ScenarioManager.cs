using UnityEngine;
using System;

public class ScenarioManager : MonoBehaviour
{
    private void Awake()
    {
        Console.WriteLine("ScenarioManager try to create spaceship");
        SpaceshipBuilder.CreateNew(new BasicMainEngine(), new PlasmaGun(), GameObject.FindWithTag("Physical scene"));
        Console.WriteLine("ScenarioManager create spaceship");
    }
}