using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Dependencies.NCalc;
using UnityEngine;
    
public class PlayerLocomotionManager : CharacterManager
{   
    private PlayerManager player;
    
    //SAME AS PLAYER VERTICAL AND HORIZONTAL INPUT VALUES IN THE INPUT MANAGER
    
    public float verticalMovement;
    public float horizontalMovement;
    public float moveAmount;
    private Vector3 turnRotationDirection;

    [SerializeField] private float walkingSpeed = 2;
    [SerializeField] private float runningSpeed = 5;
    [SerializeField] private float rotationSpeed = 15;
    
    private Vector3 moveDirection;
    protected override void Awake()
    {
        base.Awake();
    
        player = GetComponent<PlayerManager>();
    }
    
    public void HandleAllMovement()
    {
        HandleGroundedMovement();
        //AERIAL MOVEMENT
    }

    private void GetVerticalAndHorizontalInput()
    {
        verticalMovement = PlayerInputManager.Instance.verticalInput;
        horizontalMovement = PlayerInputManager.Instance.horizontalInput;
        
        //CLAMP THE MOVEMENTS
    }

    public void HandleGroundedMovement()
    {
        GetVerticalAndHorizontalInput();
        HandleRotation();
        //MOVEMENT BASED ON THE DIRECTION OF THE CAMERA PERSPECTIVE AND INPUT   
        moveDirection = PlayerCamera.Instance.transform.forward * verticalMovement;
        moveDirection = moveDirection + PlayerCamera.Instance.transform.right * horizontalMovement;
        moveDirection.Normalize();
        moveDirection.y = 0;

        if (PlayerInputManager.Instance.movementAmount > 0.5f)
        {
                player.characterController.Move( Time.deltaTime * runningSpeed * moveDirection );
        }
        else if (PlayerInputManager.Instance.movementAmount <= 0.5f)
        {
            player.characterController.Move(Time.deltaTime * walkingSpeed * moveDirection );
        }

    }

    public void HandleRotation()
    {
        turnRotationDirection = Vector3.zero;
        turnRotationDirection = PlayerCamera.Instance.cameraObject.transform.forward * verticalMovement;
        turnRotationDirection = turnRotationDirection + PlayerCamera.Instance.cameraObject.transform.right * horizontalMovement;
        turnRotationDirection.Normalize();
        turnRotationDirection.y = 0;
        

        if (turnRotationDirection == Vector3.zero)
        {
            turnRotationDirection = transform.forward;
        }
        
        Quaternion newRotation = Quaternion.LookRotation(turnRotationDirection);
        Quaternion turnRotation = Quaternion.Slerp(transform.rotation, newRotation, rotationSpeed * Time.deltaTime);
        transform.rotation = turnRotation;
    }
    
}   
