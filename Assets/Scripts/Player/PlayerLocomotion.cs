using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLocomotion : MonoBehaviour
{
    private InputManager inputManager;
    private Vector3 moveDirection;
    
    private Transform cameraObject;
    private Rigidbody playerRigidbody;

    public float walkingSpeed = 1.5f;
    public float runningSpeed = 5.0f;
    public float sprintingSpeed = 7.0f;
    public float rotationSpeed = 15.0f;
    
    private void Awake()
    {
        inputManager = GetComponent<InputManager>();
        playerRigidbody = GetComponent<Rigidbody>();
        cameraObject = Camera.main.transform;
    }
    
    public void HandleAllMovement()
    {
        HandleMovement();
        HandleRotation();
    }
    
    private void HandleMovement()
    {
        moveDirection = cameraObject.forward * inputManager.verticalInput;
        moveDirection = moveDirection + cameraObject.right * inputManager.horizontalInput;
        moveDirection.Normalize();
        moveDirection.y = 0;

        if (inputManager.moveAmount >= 0.5f)
        {
            moveDirection = moveDirection.normalized * runningSpeed;    
        }
        else
        {
            moveDirection = moveDirection.normalized * walkingSpeed;    
        }

        Vector3 movementVelocity = moveDirection;
        playerRigidbody.velocity = movementVelocity;
    }

    private void HandleRotation()
    {
        Vector3 targetDirection = Vector3.zero;

        targetDirection = cameraObject.forward * inputManager.verticalInput;
        targetDirection = targetDirection + cameraObject.right * inputManager.horizontalInput;
        targetDirection.Normalize();
        targetDirection.y = 0;
        
        if (targetDirection == Vector3.zero)
        {
            targetDirection = transform.forward;
        }
        
        Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
        Quaternion playerRotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        
        transform.rotation = playerRotation;
    }
}
