﻿using System;
using Cinemachine;
using UnityEngine;
using UnityEngine.SceneManagement;


public class CinemachineCameraController : MonoBehaviour, IController
{
    private CinemachineVirtualCamera _camera;
    public bool IsDisabled { get; set; }
    
    public void Awake()
    {
        _camera = GetComponent<CinemachineVirtualCamera>();
        _camera.Follow = GameObject.FindWithTag("Player").transform;
    }


    public void Update()
    {
        if (IsDisabled)
            return;
        
        if (Input.mouseScrollDelta != Vector2.zero)
            _camera.m_Lens.OrthographicSize -= Input.mouseScrollDelta.y * _camera.m_Lens.OrthographicSize / 20;
    }
}