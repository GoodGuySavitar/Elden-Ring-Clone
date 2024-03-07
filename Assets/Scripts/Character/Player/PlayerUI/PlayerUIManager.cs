using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
public class PlayerUIManager : MonoBehaviour
{
    public static PlayerUIManager Instance;

    [Header("NETWORK JOIN")] 
    [SerializeField] bool startGameAsClient;

    [HideInInspector] public PlayerUIHUDManager playerUIHudManager;
    public void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        playerUIHudManager = GetComponentInChildren<PlayerUIHUDManager>();
    }

    public void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    public void Update()
    {
        if (startGameAsClient)
        {
            startGameAsClient = false;
            //WE FIRST SHUT DOWN BECAUSE WE STARTED AS A HOST. 
            NetworkManager.Singleton.Shutdown();
            //WE RESTART AS A CLIENT
            NetworkManager.Singleton.StartClient();
        }
    }
}
