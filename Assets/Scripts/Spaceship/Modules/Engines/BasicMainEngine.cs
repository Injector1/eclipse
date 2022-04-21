using UnityEngine;


public class BasicMainEngine : MonoBehaviour, IEngine
{
    [SerializeField] private float MaxSpeedSqr = 200f;
    [SerializeField] private float Power = 0.1f;
    [SerializeField] private float Maneuverability = 4.5f;
    [SerializeField] private float OnRotateSlowDown = 20f;
    private Spaceship spaceship;
    private Transform spaceshipTransform;
    private Vector3 spaceshipDirection;
    
    public void Awake()
    {
        spaceship = GetComponent<Spaceship>();
        spaceshipDirection = new Vector3(0, 1);
    }
    
    public void Start()
    {
        spaceship.OnRotate += ChangeDirection;
        spaceship.OnBoost += Boost;
        spaceship.OnSlowDown += SlowDown;
        spaceshipTransform = spaceship.transform;
    }
    
    private void ChangeDirection(float axis)
    {
        spaceshipTransform.rotation *= Quaternion.AngleAxis(axis * Maneuverability, Vector3.back);
        var zSpaceshipRotation = (spaceshipTransform.eulerAngles.z + 90) / 180 * Mathf.PI;
        spaceshipDirection = new Vector3(Mathf.Cos(zSpaceshipRotation), Mathf.Sin(zSpaceshipRotation));
    }
    
    private void SlowDown(float axis)
    {
        spaceship.Rigidbody.AddForce(axis * spaceship.Rigidbody.velocity);
    }

    private void Boost(float axis)
    {
        var shVelocity = spaceship.Rigidbody.velocity;
        var dirMul = 1 - Vector3.Dot(spaceshipDirection, shVelocity.normalized);
        dirMul = dirMul > 0.1f ? 0.1f : dirMul;
        dirMul *= OnRotateSlowDown;
        SlowDown(-dirMul);
        
        var f = (1 + dirMul) * (1 - shVelocity.sqrMagnitude / MaxSpeedSqr) * Power * axis * spaceshipDirection;
        spaceship.Rigidbody.AddForce(f, ForceMode2D.Impulse);
    }
}