using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Dependencies.NCalc;
using UnityEditor;
using UnityEngine;
    
public class PlayerLocomotionManager : CharacterLocomotionManager
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

    protected override void Update()
    {
        base.Update();
        if (player.IsOwner)
        {
            player.characterNetworkManager.verticalMovement.Value = verticalMovement;
            player.characterNetworkManager.horizontalMovement.Value = horizontalMovement;
            player.characterNetworkManager.moveAmount.Value = moveAmount;
        }
        else
        {
            verticalMovement = player.characterNetworkManager.verticalMovement.Value;
            horizontalMovement = player.characterNetworkManager.horizontalMovement.Value;
            moveAmount = player.characterNetworkManager.moveAmount.Value;

            //IF NOT LOCKED ON, PASS MOVE AMOUNT
            player.playerAnimatorManager.UpdateAnimatorMovementParameters(0, moveAmount);
            
            //IF LOCKED ON, PASS HORZ AND VERT
        }
    }

    public void HandleAllMovement()
    {
        HandleGroundedMovement();
        //AERIAL MOVEMENT
    }

    private void GetMovementValues()
    {
        verticalMovement = PlayerInputManager.Instance.verticalInput;
        horizontalMovement = PlayerInputManager.Instance.horizontalInput;
        moveAmount = PlayerInputManager.Instance.moveAmount;

        //CLAMP THE MOVEMENTS
    }

    public void HandleGroundedMovement()
    {
        GetMovementValues();
        HandleRotation();
        //MOVEMENT BASED ON THE DIRECTION OF THE CAMERA PERSPECTIVE AND INPUT   
        moveDirection = PlayerCamera.Instance.transform.forward * verticalMovement;
        moveDirection = moveDirection + PlayerCamera.Instance.transform.right * horizontalMovement;
        moveDirection.Normalize();
        moveDirection.y = 0;

        if (PlayerInputManager.Instance.moveAmount > 0.5f)
        {
                player.characterController.Move( Time.deltaTime * runningSpeed * moveDirection );
        }
        else if (PlayerInputManager.Instance.moveAmount <= 0.5f)
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
