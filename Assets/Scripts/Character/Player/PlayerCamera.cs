using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    public static PlayerCamera Instance;
    public PlayerManager player;
    public Camera cameraObject;
    [SerializeField] private Transform cameraPivotTransform;

    [Header("Camera Settings")]
    private float cameraSmoothTime = 1; //THE BIGGER THIS NUMBER, THE LONGER IT TAKES FOR THE CAMERA TO COME TO ITS POSITION DURING MOVEMENT

    [SerializeField] private float leftAndRightRotationSpeed = 220;
    [SerializeField] private float upAndDownRotationSpeed = 220;
    [SerializeField] private float minimumPivot = -30; //THE LOWEST POINT YOU'RE ABLE TO LOOK DOWN
    [SerializeField] private float maximumPivot = 60; // THE HIGHEST YOU'RE ABLE TO LOOK UP
    [SerializeField] private float cameraCollisionRadius = 0.2f;
    [SerializeField] private LayerMask collideWithLayers;
    
    [Header("Camera Values")]
    private Vector3 cameraVelocity;
    private Vector3 cameraObjectPosition; //USED FOR CAMERA COLLISION (MOVES THE CAMERA OBJECT TO THIS POSITION IF COLLISION OCCURS)
    [SerializeField] private float leftAndRightLookAngle;
    [SerializeField] private float upAndDownLookAngle;
    [SerializeField] private float cameraZPosition; //VALUES FOR CAMERA COLLISION
    [SerializeField] private float targetCameraZPosition;  //VALUES FOR CAMERA COLLISION
    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
        cameraZPosition = cameraObject.transform.localPosition.z;
    }

    public void HandleAllCameraActions()
    {
        if (player != null)
        {
            HandleFollowTarget();
            HandleCameraRotation();
            HandleCollisions();
        }
    }

    private void HandleFollowTarget()
    {
        Vector3 targetCameraPosition = Vector3.SmoothDamp(transform.position, 
            player.transform.position,
            ref cameraVelocity, 
            cameraSmoothTime * Time.deltaTime);
        transform.position = targetCameraPosition;
    }

    private void HandleCameraRotation()
    {
        //IF LOCKED ON THEN, FORCE ROTATION TOWARDS TARGET 
        //ELSE ALLOW TO ROTATE FREELY
        
        //NORMAL ROTATIONS
        // ROTATE LEFT AND RIGHT BASED ON MOUSE AND JOYSTICK INPUT
        leftAndRightLookAngle += (PlayerInputManager.Instance.cameraoHorizontalInput * leftAndRightRotationSpeed) * Time.deltaTime;
        // ROTATE UP AND DOWN BASED ON MOUSE AND JOYSTICK INPUT
        upAndDownLookAngle += (PlayerInputManager.Instance.cameraVerticalInput * upAndDownRotationSpeed) * Time.deltaTime;
        // CLAMP THE CAMERA ROTATION BETWEEN MIN AND MAX VALUES
        upAndDownLookAngle = Math.Clamp(upAndDownLookAngle, minimumPivot, maximumPivot);
        
        Vector3 cameraRotation = Vector3.zero;
        Quaternion targetRotation;
        
        // ROTATE THIS GAME OBJECT LEFT AND RIGHT 
        cameraRotation.y = leftAndRightLookAngle;
        targetRotation = Quaternion.Euler(cameraRotation);
        transform.rotation = targetRotation;
        
        //ROTATE CAMERA PIVOT UP AND DOWN
        cameraRotation.x = upAndDownLookAngle;
        targetRotation = Quaternion.Euler(cameraRotation);
        cameraPivotTransform.rotation = targetRotation;

    }

    private void HandleCollisions()
    {
        targetCameraZPosition = cameraZPosition;
        RaycastHit hit;
        //DIRECTION OF COLLISION CHECK
        
        Vector3 direction = cameraObject.transform.position - cameraPivotTransform.position;
        direction.Normalize();
        
        //WE CHECK IF THERE IS AN OBJECT IN FRONT OF OUR DESIRED DIRECTION 
        if (Physics.SphereCast(cameraPivotTransform.position, cameraCollisionRadius, direction, out hit,
                Math.Abs(targetCameraZPosition), collideWithLayers))
        {
            //IF THERE IS, GET THE DISTANCE FROM IT 
            float distanceFromHitObject = Vector3.Distance(cameraPivotTransform.position, hit.point);
            //WE EQUATE OUT TARGET Z POSITION TO THE FOLLOWING 
            targetCameraZPosition = -(distanceFromHitObject - cameraCollisionRadius);
        }

        //IF OUR TARGET POSITION IS LESS THAN COLLISION RADIUS, WE SUBTRACT THE COLLISION RADIUS (MAKING IT SNAP BACK)
        if (Math.Abs(targetCameraZPosition) < cameraCollisionRadius)
        {
            targetCameraZPosition = -cameraCollisionRadius;
        }

        //WE THEN APPLY OUR FINAL POSITION USING A LERP OVER A TIME OF 0.2F
        cameraObjectPosition.z = Mathf.Lerp(cameraObject.transform.localPosition.z, targetCameraZPosition, 0.2f);
        cameraObject.transform.localPosition = cameraObjectPosition;
    }
}

