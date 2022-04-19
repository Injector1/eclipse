using UnityEngine;
using System;

/*
 Это движок - тоже модуль корабля, определяет логику передвижения
 Найти ГО движка можно в ГО Main Scene -> Spaceship -> Engine
 */
public class BasicMainEngine : MonoBehaviour, IEngine, ISpaceshipModule
{
    public Spaceship Spaceship;
    //SerializeField - поле, которое мы можем изменять в инспеторе юнити (чекните эти поля в ГО Engine, в компоненте BasicMainEngine)
    [SerializeField] public float MaxSpeedSqr = 200f;
    [SerializeField] public float Power = 0.1f;
    [SerializeField] public float Maneuverability = 4.5f;
    [SerializeField] public float OnRotateSlowDown = 20f;

    private Vector3 spaceshipDirection;
    // !!! важное правило - всё, кроме SerializeField и констант задается в Awake
    
    //налаживаем связи
    public void Awake()
    {
        Spaceship = GetComponentInParent<Spaceship>();
        spaceshipDirection = new Vector3(0, 1);
    }
    
    public void Start()
    {
        //добавляем к набору действий корабля при повороте - поворот, при бусте - буст, при замедлении - замедление
        Spaceship.OnRotate += ChangeDirection;
        Spaceship.OnBoost += Boost;
        Spaceship.OnSlowDown += SlowDown;
        //добавляем модуль движка
        Spaceship.AddModule(gameObject, typeof(IEngine));
    }
    
    private void ChangeDirection(float axis)
    {
        //страшное слово Quaternion, никто не знает что это, но все это используют
        //с помощью черной магии вычисляет углы
        var rotation = Quaternion.AngleAxis(axis * Maneuverability, Vector3.back);
        //transform - компонент каждого GameObject, из его названия понятно что из себя представляет
        Spaceship.transform.rotation *= rotation;
        var zSpaceshipRotation = (Spaceship.transform.eulerAngles.z + 90) / 180 * Mathf.PI;
        spaceshipDirection = new Vector3(Mathf.Cos(zSpaceshipRotation), Mathf.Sin(zSpaceshipRotation));
    }
    
    private void SlowDown(float axis)
    {
        Spaceship.Rigidbody.AddForce(axis * Spaceship.Rigidbody.velocity);
    }

    private void Boost(float axis)
    {
        var shVelocity = Spaceship.Rigidbody.velocity;
        var dirMul = 1 - Vector3.Dot(spaceshipDirection, shVelocity.normalized);
        dirMul = dirMul > 0.1f ? 0.1f : dirMul;
        dirMul *= OnRotateSlowDown;
        SlowDown(-dirMul);
        
        var f = (1 + dirMul) * (1 - shVelocity.sqrMagnitude / MaxSpeedSqr) * Power * axis * spaceshipDirection;
        Spaceship.Rigidbody.AddForce(f, ForceMode2D.Impulse);
    }
    
    /*
     Продолжение в Spacesip/Modules/Weapons/PlamsaGun/PlasmaGun.cs 
     */
}