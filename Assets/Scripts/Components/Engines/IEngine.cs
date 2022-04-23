using System;
using UnityEngine;

public partial class Actions : MonoBehaviour
{
    public Action<float> OnRotate;
    public Action<float> OnBoost;
    public Action<float> OnSlowDown;
}

public interface IEngine
{
}