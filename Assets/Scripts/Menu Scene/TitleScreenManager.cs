using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class TitleScreenManager : MonoBehaviour
{
    [Header("Menus")]
    [SerializeField] private GameObject titleScreenMainMenu;
    [SerializeField] private GameObject titleScreenLoadMenu;

    [Header("Buttons")] 
    public Button loadMenuReturnButton;
    public Button mainMenuLoadGameButton;
    
    public void StartNetworkHost()
    {
        NetworkManager.Singleton.StartHost();
    }
        
    public void StartNewGame()
    {
        WorldSaveGameManager.Instance.CreateNewGame();
        StartCoroutine(WorldSaveGameManager.Instance.LoadWorldScene());
    }

    public void OpenLoadGameMenu()
    {
        //CLOSE MAIN MENU
        titleScreenMainMenu.SetActive(false);
        
        //OPEN LOAD MENU
        titleScreenLoadMenu.SetActive(true);
        
        // SELECT RETURN BUTTON
        loadMenuReturnButton.Select();
    }

    public void CloseLoadGameMenu()
    {
        //CLOSE LOAD MENU
        titleScreenLoadMenu.SetActive(false);
        
        //OPEN MAIN MENU
        titleScreenMainMenu.SetActive(true);
        
        // SELECT RETURN BUTTON
        mainMenuLoadGameButton.Select();
    }
}
