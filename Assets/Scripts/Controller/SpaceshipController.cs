using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceshipController : MonoBehaviour
{
    [SerializeField] private Spaceship spaceship;
    private SavedInput<float> verticalAxis = new SavedInput<float>();
    private SavedInput<float> horizontalAxis = new SavedInput<float>();
    private SavedInput<Vector3> mousePosition = new SavedInput<Vector3>();
    
    
    
    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
            mousePosition.UpdateValue(Input.mousePosition);
        

        if (Input.GetButton("Horizontal"))
            horizontalAxis.UpdateValue(Input.GetAxis("Horizontal"));
        
        
        if (Input.GetButton("Vertical"))
            verticalAxis.UpdateValue(Input.GetAxis("Vertical"));
    }
    
    public void OnTick()
    {
        var isNoActionsCommitted = true;
        
        if (mousePosition.IsUpdated)
        {
            isNoActionsCommitted = false;
            spaceship.OnShoot?.Invoke(mousePosition.TakeValue());
        }


        if (horizontalAxis.IsUpdated)
        {
            isNoActionsCommitted = false;
            spaceship.OnRotate?.Invoke(horizontalAxis.TakeValue());
        }


        if (verticalAxis.IsUpdated)
        {
            if (verticalAxis.Value < 0)
                spaceship.OnDecelerating?.Invoke(-verticalAxis.TakeValue());
            else
                spaceship.OnForwardBoost?.Invoke(verticalAxis.TakeValue());
            isNoActionsCommitted = false;
        }
        
        if (isNoActionsCommitted)
            spaceship.OnIdle?.Invoke();
    }
}
