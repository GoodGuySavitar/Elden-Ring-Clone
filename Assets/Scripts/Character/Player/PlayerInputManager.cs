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
    private PlayerControls playerControls;
    public float verticalInput;
    public float horizontalInput;
    [SerializeField] public float movementAmount;

    [SerializeField] private Vector2 movementInput;

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
        HandleMovementInput();
    }

    private void HandleMovementInput()
    {
        verticalInput = movementInput.y;
        horizontalInput = movementInput.x;
        
        //RETURNS THE NUMBER WITHOUT ITS SIGN.
        movementAmount = Mathf.Clamp01(Math.Abs(verticalInput) + Math.Abs(horizontalInput));

        //MAKES THE SPEED EITHER 0, 0.5 OR 1. SHOWING IDLE, WALKING AND RUNNING STATE.
        if (movementAmount > 0 && movementAmount <= 0.5)
        {
            movementAmount = 0.5f;
        }
        else if(movementAmount > 0.5f && movementAmount <= 1)
        {
            movementAmount = 1;
        }
    }
}
