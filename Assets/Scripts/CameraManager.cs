using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    // The object that the camera will follow
    public Transform targetTransform;
    public float followSpeed = 0.2f;
    
    private Vector3 cameraFollowVelocity = Vector3.zero;

    private void Awake()
    {
        targetTransform = FindObjectOfType<PlayerManager>().transform;
    }

    public void FollowTarget()
    {
        Vector3 targetPosition = Vector3.SmoothDamp(transform.position, targetTransform.position, 
                                                        ref cameraFollowVelocity, followSpeed);
        transform.position = targetPosition;
    }
}
