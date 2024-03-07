using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStatsManger : MonoBehaviour
{
    private CharacterManager character;
    
    [Header("Stamina Regeneration")]
    float staminaTickTimer = 0;
    private float staminaRegenerationTimer = 0;
    [SerializeField] float staminaRegenerationAmount = 2;
    [SerializeField] private float staminaRegenerationDelay = 2;
    public int ChangeStatBasedOnEnduranceLevel(int endurance)
    {
        float stamina = 0; 
        
        //CREATE AN EQUATION FOR STAMINA AND ENDURANCE CALCULATION 

        stamina = endurance * 10;

        return Mathf.RoundToInt(stamina);
    }

    public virtual void Awake()
    {
        character = GetComponent<CharacterManager>();
    }

    public virtual void RegenerateStamina()
    {
        // ONLY OWNERS CAN EDIT THEIR OWN NETWORK VARIABLES
        if (!character.IsOwner)
            return;
        
        // WE DON'T WANT TO REGENERATE STAMINA DURING SPRINTING
        if (character.characterNetworkManager.isSprinting.Value)
            return;

        if (character.isPerformingAction)
            return;
        
        staminaRegenerationTimer += Time.deltaTime;

        if (staminaRegenerationTimer >= staminaRegenerationDelay)
        {
            if (character.characterNetworkManager.currentStamina.Value < character.characterNetworkManager.maxStamina.Value)
            {
                staminaTickTimer += Time.deltaTime;

                if (staminaTickTimer >= 0.1)   //1/10TH SECOND 
                {
                    staminaTickTimer = 0;
                    character.characterNetworkManager.currentStamina.Value += staminaRegenerationAmount;
                }
            }
        }
    }
    
    public virtual void ResetStaminaRegenerationTimer(float previousStaminaAmount, float currentStaminaAmount)
    {   
        // WE ONLY WANT TO RESET THE TIMER IF THE ACTION USED STAMINA 
        // WE DONT WANT TO RESET THE REGENERATION IF WE ARE ALREADY REGENERATING STAMINA
        if (currentStaminaAmount < previousStaminaAmount)
        {
            staminaRegenerationTimer = 0;
        }
    }
}
