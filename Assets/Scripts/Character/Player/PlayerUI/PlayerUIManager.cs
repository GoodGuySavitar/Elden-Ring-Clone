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
