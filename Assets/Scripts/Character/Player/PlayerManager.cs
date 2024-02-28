using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerManager : CharacterManager
{
    [HideInInspector] public PlayerAnimatorManager playerAnimatorManager;
    [HideInInspector] public PlayerLocomotionManager playerLocomotionManager;
    [HideInInspector] public PlayerNetworkManager playerNetworkManager;
    protected override void Awake()
    {
        base.Awake();
        
        //DO MORE STUFF ONLY FOR THE PLAYER 
        playerLocomotionManager = GetComponent<PlayerLocomotionManager>();
        playerAnimatorManager = GetComponent<PlayerAnimatorManager>();
        playerNetworkManager = GetComponent<PlayerNetworkManager>();
    }

    protected override void Update()
    {
        base.Update();

        //IF WE DO NOT OWN THE GAME, WE DO NOT CONTROL OR EDIT       
        if (!IsOwner)
            return;
        
        //HANDLE MOVEMENT
        playerLocomotionManager.HandleAllMovement();
    }

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();

        if (IsOwner)
        {
            PlayerCamera.Instance.player = this;
            PlayerInputManager.Instance.player = this;  //ONLY ONE LOCAL PLAYER IN THE GAME
        }
    }
}
