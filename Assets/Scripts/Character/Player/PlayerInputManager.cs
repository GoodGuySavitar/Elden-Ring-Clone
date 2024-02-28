using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
public class PlayerInputManager : MonoBehaviour
{
    // ADDING MOVEMENT
    //2. MOVE THE PLAYER BASED ON THOSE VALUES 

    public static PlayerInputManager Instance;
    public PlayerManager player;
    private PlayerControls playerControls;
    
    [Header("CAMERA ROTATION INPUT")]
    [SerializeField] private Vector2 cameraMovementInput;
    public float cameraVerticalInput;
    public float cameraoHorizontalInput;
    
    [Header("PLAYER MOVEMENT INPUT")]
    [SerializeField] private Vector2 movementInput;
    public float verticalInput;
    public float horizontalInput;
    [SerializeField] public float moveAmount;

    [Header("PLAYER ACTIONS INPUT")] 
    [SerializeField] private bool dodgeInput = false;
    [SerializeField] private bool sprintInput = false;
    
    
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
        // WHEN SCENE CHANGES, RUN THIS LOGIC( THIS IS BASICALLY AN EVENT) 
        SceneManager.activeSceneChanged += OnSceneChange;
        Instance.enabled = false;
    }

    private void OnSceneChange(Scene oldScene , Scene newScene)
    {
        //IF WE ARE LOADING INTO THE WORLD SCENE, WE ENABLE THE PLAYER CONTROLS BY ENABLING THIS SCRIPT
        if (newScene.buildIndex == WorldSaveGameManager.Instance.GetWorldSceneIndex()) 
        {
            Instance.enabled = true;
        }
        //DISABLE THE SCRIPT AND PLAYER CONTROLS IF WE ARE AT THE MAIN MENU
        //THIS IS DONE SO THE PLAYER CANNOT MOVE WHEN IN THE MAIN MENU SCREEN
        else
        {
            Instance.enabled = false;
        }
    }

    private void OnEnable()
    {
        if (playerControls == null)
        {
            playerControls = new PlayerControls();

            playerControls.PlayerMovement.Movement.performed += i => movementInput = i.ReadValue<Vector2>();
            playerControls.CameraMovement.Movement.performed += i => cameraMovementInput = i.ReadValue<Vector2>();
            playerControls.PlayerActions.Dodge.performed += i => dodgeInput = true;
            
            //HOLDING THE BUTTON SETS THE BOOL TO TRUE
            playerControls.PlayerActions.Sprint.performed += i => sprintInput = true;
            //RELEASING THE BUTTON SETS THE BUTTON TO FALSE 
            playerControls.PlayerActions.Sprint.canceled += i => sprintInput = false;
        }
        playerControls.Enable();
    }

    private void OnDestroy()
    {
        //IF WE DESTROY THIS OBJECT, UNSUBSCRIBE FROM THE EVENT  
        SceneManager.activeSceneChanged -= OnSceneChange;
    }

    //IF WE MINIMIZE OR LOWER THE WINDOW, WE STOP TAKING THE INPUTS 
    private void OnApplicationFocus(bool hasFocus)
    {
        if (enabled)
        {
            playerControls.Enable();
        }
        else
        {
            playerControls.Disable();
        }
    }

    private void Update()
    {
        HandleAllInputs();
    }

    private void HandleAllInputs()
    {
        HandlePlayerMovementInput();
        HandleCameraMovementInput();
        HandleDodgeMovement();
        HandleSprinting();
    }
    
    // MOVEMENT
    private void HandlePlayerMovementInput()
    {
        verticalInput = movementInput.y;
        horizontalInput = movementInput.x;
        
        //RETURNS THE NUMBER WITHOUT ITS SIGN.
        moveAmount = Mathf.Clamp01(Math.Abs(verticalInput) + Math.Abs(horizontalInput));

        //MAKES THE SPEED EITHER 0, 0.5 OR 1. SHOWING IDLE, WALKING AND RUNNING STATE.
        if (moveAmount > 0 && moveAmount <= 0.5)
        {
            moveAmount = 0.5f;
        }
        else if(moveAmount > 0.5f && moveAmount <= 1)
        {
            moveAmount = 1;
        }
        
        //WE DONT USE THE HORIZONTAL VALUES BECAUSE WHEN WE ARENT LOCKED ON, WE USE ONLY IDLE,WALK AND RUN ANIMATIONS
        //AND THE CAMERA ROTATES USING ONLY THESE ANIMATIONS
        //WHEN WE ARE LOCKED ON TO THE ENEMY, WE CAN ENABLE THE HORIZONTAL VALUES TO ENABLE STRAFING

        if (player == null)
            return;
        //IF WE ARE NOT LOCKED ON, USE ONLY MOVEMENT AMOUNT 
        player.playerAnimatorManager.UpdateAnimatorMovementParameters(0, moveAmount , player.playerNetworkManager.isSprinting.Value);
        
        //IF WE ARE LOCKED ON, PASS THE HORIZONTAL VALUES 
    }

    private void HandleCameraMovementInput()
    {
        cameraVerticalInput = cameraMovementInput.y;
        cameraoHorizontalInput = cameraMovementInput.x;
    }

    // ACTION
    private void HandleDodgeMovement()
    {
        if (dodgeInput)
        {
            dodgeInput = false;
            
            //  FUTURE NOTE: RETURN(DO NOTHING)  IF THE UI WINDOW OR MENU IS OPEN
            
            player.playerLocomotionManager.AttemptToPerformDodge();
        }
    }

    private void HandleSprinting()
    {
        if (sprintInput)
        {
            player.playerLocomotionManager.HandleSprinting();
        }
        else
        {
            player.playerNetworkManager.isSprinting.Value = false;
        }
    }
}
