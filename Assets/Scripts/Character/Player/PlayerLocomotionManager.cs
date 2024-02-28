using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.VisualScripting.Dependencies.NCalc;
using UnityEditor;
using UnityEngine;
    
public class PlayerLocomotionManager : CharacterLocomotionManager
{   
    private PlayerManager player;
    
    //SAME AS PLAYER VERTICAL AND HORIZONTAL INPUT VALUES IN THE INPUT MANAGER
    [HideInInspector]public float verticalMovement;
    [HideInInspector]public float horizontalMovement;
    [HideInInspector]public float moveAmount;

    [Header("MOVEMENT SETTINGS")]
    private Vector3 turnRotationDirection;
    private Vector3 moveDirection;
    [SerializeField] private float walkingSpeed = 2;
    [SerializeField] private float runningSpeed = 5;
    [SerializeField] private float sprintingSpeed = 6.5f;
    [SerializeField] private float rotationSpeed = 15;

    [Header("DODGE")] 
    private Vector3 rollDirection;
    
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
            player.playerAnimatorManager.UpdateAnimatorMovementParameters(0, moveAmount , player.playerNetworkManager.isSprinting.Value);
            
            //IF LOCKED ON, PASS HORZ AND VERT
        }
    }

    public void HandleAllMovement()
    {
        HandleGroundedMovement();
        HandleRotation();
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
        if (!player.canMove)
            return;
        
        GetMovementValues();
        //MOVEMENT BASED ON THE DIRECTION OF THE CAMERA PERSPECTIVE AND INPUT   
        moveDirection = PlayerCamera.Instance.transform.forward * verticalMovement;
        moveDirection = moveDirection + PlayerCamera.Instance.transform.right * horizontalMovement;
        moveDirection.Normalize();
        moveDirection.y = 0;

        if (player.playerNetworkManager.isSprinting.Value)
        {
            player.characterController.Move( Time.deltaTime * sprintingSpeed * moveDirection );
        }
        else
        {
            if (PlayerInputManager.Instance.moveAmount > 0.5f)
            {
                player.characterController.Move( Time.deltaTime * runningSpeed * moveDirection );
            }
            else if (PlayerInputManager.Instance.moveAmount <= 0.5f)
            {
                player.characterController.Move(Time.deltaTime * walkingSpeed * moveDirection );
            }    
        }
    }

    public void HandleRotation()
    {
        if(!player.canRotate)
            return;
        
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

    public void HandleSprinting()
    {
        if (player.isPerformingAction)
        {
            player.playerNetworkManager.isSprinting.Value = false;
        }
        
        // IF MOVING, SPRINTING IS SET TO TRUE 
        if (moveAmount >= 0.5)
        {
            player.playerNetworkManager.isSprinting.Value = true;
        }
        //IF NOT MOVING/MOVING SLOWLY, SPRINTING IS SET TO FALSE
        else
        {
            player.playerNetworkManager.isSprinting.Value = false;
        }
        
        // IF NO STAMINA THEN SPRINTING IS SET TO FALSE 
        // IF THE PLAYER IS STANDING, SPRINTING IS SET TO FALSE
    }

    public void AttemptToPerformDodge()
    {
        if (player.isPerformingAction)
            return;
        
        //IF MOVING, PERFORM A ROLL
        if (moveAmount > 0)
        {
            rollDirection = PlayerCamera.Instance.transform.forward * verticalMovement;  //can also use PlayerInputManager.Instance.verticalInput;
            rollDirection += PlayerCamera.Instance.transform.right * horizontalMovement;
            rollDirection.y = 0;
            rollDirection.Normalize();

            Quaternion playerRotation = Quaternion.LookRotation(rollDirection);
            player.transform.rotation = playerRotation;

            player.playerAnimatorManager.PlayTargetActionAnimation("Roll_Forward_01",true,true);
        }
        //IF STATIONARY, PERFORM A BACKSTEP
        else
        {
            player.playerAnimatorManager.PlayTargetActionAnimation("Jump_Backward_01",true,true);
        }
    }
}   
