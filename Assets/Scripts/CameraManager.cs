using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    private InputManager inputManager;
    private PlayerLocomotion playerLocomotion;
    
    public Transform targetTransform;       // The object that the camera will follow
    public Transform cameraPivot;           // The object that the camera will pivot (up and down)
    public Transform cameraTransform;       // The transform of the camera itself
    public LayerMask collisionLayers;       // The layers that the camera will collide with
    private float defaultPosition;
    private Vector3 cameraFollowVelocity = Vector3.zero;
    private Vector3 cameraVectorPosition;
    
    [SerializeField] private float cameraCollisionOffset = 0.2f; // The distance the camera will be from the player when colliding
    [SerializeField] private float minimumCollisionOffset = 0.2f; // The minimum distance the camera will be from the player when colliding
    [SerializeField] private float cameraCollisionRadius = 0.2f;
    private float cameraFollowSpeed = 0.04f;
    
    private float cameraLookSpeed = 1.0f;
    private float cameraPivotSpeed = 0.75f;
    public float minimumPivotAngle = -35.0f;
    public float maximumPivotAngle = 45.0f;

    public float lookAngle;     // The angle that the camera is looking at
    public float pivotAngle;    // The angle that the camera is pivoting around the player

    private void Awake()
    {
        inputManager = FindObjectOfType<InputManager>();
        targetTransform = FindObjectOfType<PlayerManager>().transform;
        playerLocomotion = FindObjectOfType<PlayerLocomotion>();
        cameraTransform = Camera.main.transform;
        defaultPosition = cameraTransform.localPosition.z;
    }
    
    public void HandleAllCameraMovement()
    {
        FollowTarget();
        RotateCamera();
        HandleCameraCollision();
    }

    private void FollowTarget()
    {
        if (playerLocomotion.isSprinting)
        {
            cameraFollowSpeed = 0.02f;
        }
        else
        {
            cameraFollowSpeed = 0.04f;
        }
        
        
        Vector3 targetPosition = Vector3.SmoothDamp(transform.position, targetTransform.position, 
                                                        ref cameraFollowVelocity, cameraFollowSpeed);
        transform.position = targetPosition;
    }
    
    private void RotateCamera()
    {
        Vector3 rotation;
        Quaternion targetRotation;
        
        lookAngle = lookAngle + (inputManager.cameraInputX * cameraLookSpeed);
        pivotAngle = pivotAngle - (inputManager.cameraInputY * cameraPivotSpeed);
        pivotAngle = Mathf.Clamp(pivotAngle, minimumPivotAngle, maximumPivotAngle);

        rotation = Vector3.zero;
        rotation.y = lookAngle;
        
        targetRotation = Quaternion.Euler(rotation);
        transform.rotation = targetRotation;
        
        rotation = Vector3.zero;
        rotation.x = pivotAngle;
        
        targetRotation = Quaternion.Euler(rotation);
        cameraPivot.localRotation = targetRotation;
    }

    private void HandleCameraCollision()
    {
        float targetPosition = defaultPosition;
        RaycastHit hit;
        Vector3 direction = cameraTransform.position - cameraPivot.position;
        
        direction.Normalize();
        
        if (Physics.SphereCast
                (cameraPivot.transform.position, cameraCollisionRadius, direction, out hit,
                    Mathf.Abs(targetPosition), collisionLayers))
        {
            float distance = Vector3.Distance(cameraPivot.position, hit.point);
            targetPosition =- (distance - cameraCollisionOffset);
        }

        if (Mathf.Abs(targetPosition) < minimumCollisionOffset)
        {
            targetPosition = targetPosition - minimumCollisionOffset;
        }
        
        cameraVectorPosition.z = Mathf.Lerp(cameraTransform.localPosition.z, targetPosition, 0.2f);
        cameraTransform.localPosition = cameraVectorPosition;
    }
}
