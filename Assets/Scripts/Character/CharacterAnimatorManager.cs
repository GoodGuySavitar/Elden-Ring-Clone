using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAnimatorManager : MonoBehaviour
{
    private CharacterManager character;
    protected virtual void Awake()
    {
        character = GetComponent<CharacterManager>();
    }

    public void UpdateAnimatorMovementParameters(float horizontalValue, float verticalValue)
    {
        character.animator.SetFloat("Horizontal", horizontalValue, 0.1f, Time.deltaTime);
        character.animator.SetFloat("Vertical", verticalValue, 0.1f, Time.deltaTime);
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
    }
}
