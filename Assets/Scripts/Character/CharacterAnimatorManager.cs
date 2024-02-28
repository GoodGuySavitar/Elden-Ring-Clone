using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class CharacterAnimatorManager : MonoBehaviour
{
    private CharacterManager character;

    private int vertical;
    private int horizontal;
    
    protected virtual void Awake()
    {
        character = GetComponent<CharacterManager>();

        // SETS STRING TO HASH SO THAT WE DONT HAVE TO WRITE STRINGS EVERYTIME
        vertical = Animator.StringToHash("Vertical");
        horizontal = Animator.StringToHash("Horizontal");
    }

    public void UpdateAnimatorMovementParameters(float horizontalMovement, float verticalMovement, bool isSprinting)
    {
        float horizontalAmount = horizontalMovement;
        float verticalAmount = verticalMovement;
        
        if (isSprinting)
        {
            verticalAmount = 2;
        }
        character.animator.SetFloat(horizontal, horizontalAmount, 0.1f, Time.deltaTime);
        character.animator.SetFloat(vertical , verticalAmount, 0.1f, Time.deltaTime);
        
    }

    public virtual void PlayTargetActionAnimation(
        string targetAnimation, 
        bool isPerformingAction, 
        bool applyRotation = true, 
        bool canRotate = false, 
        bool canMove = false )
    {
        character.applyRootMotion = applyRotation; 
        character.animator.CrossFade(targetAnimation, 0.2f);
        //CAN BE USED TO STOP CHARACTER FROM ATTEMPTING NEW ACTIONS 
        //THIS WILL TURN TRUE IF STUN LOCKED SO THE PLAYER CANNOT PERFORM AN ACTION TO GET AWAY 
        //BASICALLY THE PLAYER CANNOT PERFORM MULTIPLE ACTIONS AT ONCE AND HENCE WE CHECK THIS BOOL BEFORE PERFORMING AN ACTION 
        character.isPerformingAction = isPerformingAction;
        character.canRotate = canRotate;
        character.canMove = canMove;
        
        //TELL THE HOST/SERVER THAT WE PLAYED AN ANIMATION AND PLAY THAT ANIMATION FOR EVERYBODY ELSE PRESENT 
        character.characterNetworkManager.NotifyTheServerOfActionAnimationServerRpc(NetworkManager.Singleton.LocalClientId , targetAnimation, applyRotation);
    }
}
