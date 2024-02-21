using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerManager : CharacterManager
{
    private PlayerLocomotionManager playerLocomotionManager;
    protected override void Awake()
    {
        base.Awake();
        
        //DO MORE STUFF ONLY FOR THE PLAYER 
        playerLocomotionManager = GetComponent<PlayerLocomotionManager>();
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
        }
    }
}
