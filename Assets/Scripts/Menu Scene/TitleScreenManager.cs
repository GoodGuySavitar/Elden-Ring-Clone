using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class TitleScreenManager : MonoBehaviour
{
    public static TitleScreenManager Instance;
    
    [Header("Menus")]
    [SerializeField] private GameObject titleScreenMainMenu;
    [SerializeField] private GameObject titleScreenLoadMenu;

    [Header("Buttons")] 
    public Button loadMenuReturnButton;
    public Button mainMenuLoadGameButton;
    public Button mainMenuNewGameButton;

    [Header("PopUps")] 
    [SerializeField] private GameObject noCharacterSlotsPopUp; 
    [SerializeField] private Button noCharacterSlotsOkButton; 
    
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

    public void StartNetworkHost()
    {
        NetworkManager.Singleton.StartHost();
    }
        
    public void StartNewGame()
    {
        WorldSaveGameManager.Instance.AttemptToCreateNewGame();
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

    public void DisplayNoFreeSlotsPopUp()
    {
        noCharacterSlotsPopUp.SetActive(true);
        noCharacterSlotsOkButton.Select();
    }

    public void CloseDisplayNoFreeSlotsPopUp()
    {
        noCharacterSlotsPopUp.SetActive(false);
        mainMenuNewGameButton.Select();
    }
}
