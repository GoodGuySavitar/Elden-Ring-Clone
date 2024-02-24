using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class CharacterManager : NetworkBehaviour
{
    [HideInInspector]public CharacterController characterController;
    [HideInInspector] public Animator animator;
    
    [HideInInspector]public CharacterNetworkManager characterNetworkManager;
    protected virtual void Awake()
    {
        DontDestroyOnLoad(this);
        characterController = GetComponent<CharacterController>();
        characterNetworkManager = GetComponent<CharacterNetworkManager>();
        animator = GetComponent<Animator>();
    }

    protected virtual void Update()
    {
        //IF THE CHARACTER IS BEING CONTROLLED FROM OUR SIDE, THEN ITS NETWORK POSITION IS SET TO TRANSFORM POSITION
        if (IsOwner)
        {
            characterNetworkManager.networkPosition.Value = transform.position;
            characterNetworkManager.networkRotation.Value = transform.rotation;
        }
        //IF THE CHARACTER IS BEING CONTROLLED FROM ELSE WHERE,
        //THEN ASSIGN ITS POSITION HERE LOCALLY BY THE POSITION OF ITS NETWORK TRANSFORM
        else
        {
            transform.position = Vector3.SmoothDamp
                (transform.position, 
                      characterNetworkManager.networkPosition.Value, 
                     ref characterNetworkManager.networkPositionVelocity, 
                    characterNetworkManager.networkPositionSmoothTime);

            transform.rotation = Quaternion.Slerp
            (transform.rotation,
                characterNetworkManager.networkRotation.Value,
                characterNetworkManager.networkRotationSmoothTime);
        }
    }

    protected void LateUpdate()
    {
        if (!IsOwner)
            return;
        PlayerCamera.Instance.HandleAllCameraActions();
    }
}
