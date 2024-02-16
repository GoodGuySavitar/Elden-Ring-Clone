using System.Collections;
using System.Collections.Generic;
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
        
        //HANDLE MOVEMENT
        playerLocomotionManager.HandleAllMovement();
    }
}
