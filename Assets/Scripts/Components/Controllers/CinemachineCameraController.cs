using System;
using Cinemachine;
using UnityEngine;
using UnityEngine.SceneManagement;


public class CinemachineCameraController : MonoBehaviour
{
    private CinemachineVirtualCamera _camera;
    
    public void Awake()
    {
        _camera = GetComponent<CinemachineVirtualCamera>();
    }


    public void Update()
    {
        if (Input.mouseScrollDelta != Vector2.zero)
            _camera.m_Lens.OrthographicSize -= Input.mouseScrollDelta.y * _camera.m_Lens.OrthographicSize / 20;
    }
}