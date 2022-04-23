using System;
using UnityEngine;


public class PlayerController : MonoBehaviour
{
    private Actions _actions;
    private SavedInput<float> horizontalAxis;
    private SavedInput<float> verticalAxis;
    private SavedInput<Vector3> mousePosition;
    public bool IsDisabled;

    public void Awake()
    {
        _actions = GetComponent<Actions>();
        horizontalAxis = new SavedInput<float>();
        verticalAxis = new SavedInput<float>();
        mousePosition = new SavedInput<Vector3>();
    }

    private void Start()
    {
        _actions.OnDeath += () => IsDisabled = true;
    }

    public void Update()
    {
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
            _actions.OnShoot?.Invoke(lookVector);
        }

        if (horizontalAxis.IsUpdated) 
            _actions.OnRotate?.Invoke(horizontalAxis.Value);
        
        if (verticalAxis.IsUpdated)
        {
            if (verticalAxis.Value > 0) _actions.OnBoost?.Invoke(verticalAxis.Value);
            else _actions.OnSlowDown?.Invoke(verticalAxis.Value);
        }
    }
}