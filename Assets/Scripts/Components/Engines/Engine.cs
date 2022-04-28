using System;
using UnityEngine;

public class Engine : MonoBehaviour
{
    public Action<float> OnRotate;
    public Action<float> OnBoost;
    public Action<float> OnSlowDown;
}