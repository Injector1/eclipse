using UnityEngine;
using System;

public static class SpaceshipBuilder
{
    public static void CreateNew(IEngine engine, IWeapon weapon, GameObject parent)
    {
        //TODO реализовать
        var gameObject = new GameObject();
        gameObject.AddComponent<Spaceship>();
        gameObject.AddComponent<PlayerController>();
        gameObject.AddComponent<BasicMainEngine>();
        gameObject.AddComponent<PlasmaGun>();

        gameObject.name = "PlayerSpaceship";
        gameObject.transform.parent = parent.transform;
        
    }
}