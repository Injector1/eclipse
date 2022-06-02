﻿using System;
using Cinemachine;
using UnityEngine;
using UnityEngine.SceneManagement;


public class CinemachineCameraController : MonoBehaviour, IController
{
    private CinemachineVirtualCamera _camera;
    public bool IsDisabled { get; set; }
    public float MaxDistance;
    public float MinDistance;
    
    public void Awake()
    {
        _camera = GetComponent<CinemachineVirtualCamera>();
        _camera.Follow = GameObject.FindWithTag("Player").transform;
    }


    public void Update()
    {
        if (IsDisabled)
            return;

        if (Input.mouseScrollDelta == Vector2.zero) return;
        
        var newDistance = _camera.m_Lens.OrthographicSize -
                          Input.mouseScrollDelta.y * _camera.m_Lens.OrthographicSize / 20;
        if (newDistance > MinDistance && newDistance < MaxDistance)
            _camera.m_Lens.OrthographicSize = newDistance;

    }
}