using System;
using UnityEngine;


public class PlayerController : MonoBehaviour, IController
{
    private Engine _engine;
    private Health _health;
    private WeaponController _weaponController;
    private SavedInput<float> horizontalAxis;
    private SavedInput<float> verticalAxis;
    private SavedInput<Vector3> mousePosition;
    public bool IsDisabled { get; set; }

    public void Awake()
    {
        _engine = GetComponent<Engine>();
        _health = GetComponent<Health>();
        _weaponController = GetComponentInParent<WeaponController>();
        horizontalAxis = new SavedInput<float>();
        verticalAxis = new SavedInput<float>();
        mousePosition = new SavedInput<Vector3>();
    }

    private void Start()
    {
        _health.OnDeath += () => IsDisabled = true;
    }

    public void Update()
    {
        if (IsDisabled)
            return;
        
        if (Input.GetButton("Fire1"))
            mousePosition.Value = Input.mousePosition;

        if (Input.GetButton("Horizontal"))
            horizontalAxis.Value = Input.GetAxis("Horizontal");

        if (Input.GetButton("Vertical"))
            verticalAxis.Value = Input.GetAxis("Vertical");
        
    }

    public void FixedUpdate()
    {
        if (IsDisabled)
            return;
        
        if (mousePosition.IsUpdated)
        {
            var lookVector = mousePosition.Value - Camera.main.WorldToScreenPoint(transform.position);
            _weaponController?.OnShoot?.Invoke(lookVector);
        }

        if (horizontalAxis.IsUpdated) 
            _engine.OnRotate?.Invoke(horizontalAxis.Value);
        
        if (verticalAxis.IsUpdated)
        {
            if (verticalAxis.Value > 0) _engine.OnBoost?.Invoke(verticalAxis.Value);
            else _engine.OnSlowDown?.Invoke(verticalAxis.Value);
        }
    }
}