using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerManager : CharacterManager
{
    [HideInInspector] public PlayerAnimatorManager playerAnimatorManager;
    [HideInInspector] public PlayerLocomotionManager playerLocomotionManager;
    [HideInInspector] public PlayerNetworkManager playerNetworkManager;
    [HideInInspector] public PlayerStatsManager playerStatsManager;
    
    protected override void Awake()
    {
        base.Awake();
        
        //DO MORE STUFF ONLY FOR THE PLAYER 
        playerLocomotionManager = GetComponent<PlayerLocomotionManager>();
        playerAnimatorManager = GetComponent<PlayerAnimatorManager>();
        playerNetworkManager = GetComponent<PlayerNetworkManager>();
        playerStatsManager = GetComponent<PlayerStatsManager>();
    }

    protected override void Update()
    {
        base.Update();

        //IF WE DO NOT OWN THE GAME, WE DO NOT CONTROL OR EDIT       
        if (!IsOwner)
            return;
        
        //HANDLE MOVEMENT
        playerLocomotionManager.HandleAllMovement();
        
        //REGEN STAMINA
        playerStatsManager.RegenerateStamina();
    }

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();

        if (IsOwner)
        {
            PlayerCamera.Instance.player = this;
            PlayerInputManager.Instance.player = this;  //ONLY ONE LOCAL PLAYER IN THE GAME

            playerNetworkManager.currentStamina.OnValueChanged += PlayerUIManager.Instance.playerUIHudManager.SetNewStaminaValue;
            playerNetworkManager.currentStamina.OnValueChanged += playerStatsManager.ResetStaminaRegenerationTimer;
            
            // THIS WILL BE MOVED WHEN SAVING AND LOADING IS ADDED 
            playerNetworkManager.maxStamina.Value = 
                playerStatsManager.ChangeStatBasedOnEnduranceLevel(playerNetworkManager.endurance.Value);
            playerNetworkManager.currentStamina.Value =
                playerStatsManager.ChangeStatBasedOnEnduranceLevel(playerNetworkManager.endurance.Value); 
            PlayerUIManager.Instance.playerUIHudManager.SetMaxStatValue(playerNetworkManager.maxStamina.Value);
        } 
    }
}
