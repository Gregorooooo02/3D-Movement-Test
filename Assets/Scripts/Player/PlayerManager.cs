using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    private InputManager inputManager;
    private CameraManager cameraManager;
    private PlayerLocomotion playerLocomotion;

    private void Awake()
    {
        inputManager = GetComponent<InputManager>();
        cameraManager = FindObjectOfType<CameraManager>();
        playerLocomotion = GetComponent<PlayerLocomotion>();
    }
    
    private void Update()
    {
        inputManager.HandleAllInputs();
    }
    
    private void FixedUpdate()
    {
        playerLocomotion.HandleAllMovement();
    }
    
    private void LateUpdate()
    {
        cameraManager.HandleAllCameraMovement();
    }
}
