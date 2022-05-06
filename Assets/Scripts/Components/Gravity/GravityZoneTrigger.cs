using System;
using UnityEngine;

public class GravityZoneTrigger : MonoBehaviour
{
    private Gravity objectWithGravity;
    
    private void Awake()
    {
        objectWithGravity = GetComponentInParent<Gravity>();
    }

    public void OnTriggerEnter2D(Collider2D other) => objectWithGravity.OnEnterGravityZoneTrigger(other);
    
    public void OnTriggerExit2D(Collider2D other) => objectWithGravity.OnExitGravityZoneTrigger(other);
}