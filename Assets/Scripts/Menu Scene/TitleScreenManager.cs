using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class TitleScreenManager : MonoBehaviour
{
    public void StartNetworkHost()
    {
        NetworkManager.Singleton.StartHost();
    }
        
    public void StartNewGame()
    {
        StartCoroutine(WorldSaveGameManager.Instance.LoadNewGame());
    }
}
