using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;
public class WorldSaveGameManager : MonoBehaviour
{
    public static WorldSaveGameManager Instance;
    public PlayerManager player;

    [Header("SAVE/LOAD")] 
    [SerializeField] private bool saveGame;
    [SerializeField] private bool loadGame;
    
    [Header("World Scene Index")]
    [SerializeField] private int worldSceneIndex = 1;

    [Header("Save Data Writer")] 
    public SaveFileDataWriter saveFileDataWriter;

    [Header("Current Character Data")] 
    public CharacterSlot currentCharacterSlotBeingUsed;
    public CharacterSaveData currentCharacterData;
    private string saveFileName;
    
    [Header("Character Slots")] 
    public CharacterSaveData characterSlot1;
    public CharacterSaveData characterSlot2;
    public CharacterSaveData characterSlot3;
    public CharacterSaveData characterSlot4;
    public CharacterSaveData characterSlot5;
    public CharacterSaveData characterSlot6;
    public CharacterSaveData characterSlot7;
    public CharacterSaveData characterSlot8;
    public CharacterSaveData characterSlot9;
    public CharacterSaveData characterSlot10;
    
    //There can only be one instance of this script. If another exists then destroy it.
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
        LoadAllCharacterProfiles();
    }

    private void Update()
    {
        if (saveGame)
        {
            saveGame = false;
            SaveGame();
        }

        if (loadGame)
        {
            loadGame = false;
            LoadGame();
        }
    }

    public string DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(CharacterSlot characterSlot)
    {
        string fileName = "";
        switch (characterSlot)
        { 
            case CharacterSlot.CharacterSlot_01:
                fileName = "CharacterSlot_01";
                break;
            case CharacterSlot.CharacterSlot_02:
                fileName = "CharacterSlot_02";
                break;
            case CharacterSlot.CharacterSlot_03:
                fileName = "CharacterSlot_03";
                break;
            case CharacterSlot.CharacterSlot_04:
                fileName = "CharacterSlot_04";
                break;
            case CharacterSlot.CharacterSlot_05:
                fileName = "CharacterSlot_05";
                break;
            case CharacterSlot.CharacterSlot_06:
                fileName = "CharacterSlot_06";
                break;
            case CharacterSlot.CharacterSlot_07:
                fileName = "CharacterSlot_07";
                break;
            case CharacterSlot.CharacterSlot_08:
                fileName = "CharacterSlot_08";
                break;
            case CharacterSlot.CharacterSlot_09:
                fileName = "CharacterSlot_09";
                break;
            case CharacterSlot.CharacterSlot_10:
                fileName = "CharacterSlot_10";
                break;
            default:
                break;
        }

        return fileName;
    }

    public void AttemptToCreateNewGame()
    {
        saveFileDataWriter = new SaveFileDataWriter();
        saveFileDataWriter.saveFileName = Application.persistentDataPath;
        //CHECK IF WE CAN CREATE A NEW SAVE FILE(CHECK FOR EXISTING SAVE FILES FIRST)
        saveFileName = DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(CharacterSlot.CharacterSlot_01);
        if (!saveFileDataWriter.CheckToSeeIfFileExists())
        {
            currentCharacterSlotBeingUsed = CharacterSlot.CharacterSlot_01;
            currentCharacterData = new CharacterSaveData();
            StartCoroutine(LoadWorldScene());
            return;
        }
        
        //CHECK IF WE CAN CREATE A NEW SAVE FILE(CHECK FOR EXISTING SAVE FILES FIRST)
        saveFileName = DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(CharacterSlot.CharacterSlot_02);
        if (!saveFileDataWriter.CheckToSeeIfFileExists())
        {
            currentCharacterSlotBeingUsed = CharacterSlot.CharacterSlot_02;
            currentCharacterData = new CharacterSaveData();
            StartCoroutine(LoadWorldScene());
            return;
        }
       
        //IF THERE ARE NO FREE SLOTS, NOTIFY THE PLAYER
        
    }

    public void LoadGame()
    {
        //LOAD A SAVED FILE, WITH THE NAME DEPENDING ON THE SLOT WE ARE USING
        saveFileName = DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(currentCharacterSlotBeingUsed);

        saveFileDataWriter = new SaveFileDataWriter();
        //  GENERALLY WORKS ON MULTIPLE MACHINE TYPES (Application.persistentDataPath)
        saveFileDataWriter.saveDataDirectoryPath = Application.persistentDataPath;
        saveFileDataWriter.saveFileName = saveFileName;
        currentCharacterData = saveFileDataWriter.LoadSaveFile();

        StartCoroutine(LoadWorldScene());
    }

    public void SaveGame()
    { 
        //SAVE THE CURRENT FILE UNDER A FILE NAME DEPENDING ON WHICH SLOT WE ARE USING
        saveFileName = DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(currentCharacterSlotBeingUsed);
        
        saveFileDataWriter = new SaveFileDataWriter();
        //  GENERALLY WORKS ON MULTIPLE MACHINE TYPES (Application.persistentDataPath)
        saveFileDataWriter.saveDataDirectoryPath = Application.persistentDataPath;
        Debug.Log(saveFileDataWriter.saveDataDirectoryPath);
        saveFileDataWriter.saveFileName = saveFileName;
        
        //PASS THE PLAYER INFO, FROM GAME, TO THEIR SAVE FILE 
        player.SaveGameDataToCurrentCharacterData(ref currentCharacterData);
        
        //WRITE THAT INFO ONTO A JSON FILE, SAVED TO THIS MACHINE 
        saveFileDataWriter.CreateNewSaveFile(currentCharacterData);
    }

    // LAOD ALL CHARACTER PROFILES ON  DEVICE WHEN GAME STARTS

    public void LoadAllCharacterProfiles()
    {
        saveFileDataWriter = new SaveFileDataWriter();
        saveFileDataWriter.saveFileName = Application.persistentDataPath;

        saveFileDataWriter.saveFileName = DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(CharacterSlot.CharacterSlot_01);
        characterSlot1 = saveFileDataWriter.LoadSaveFile();
        
        saveFileDataWriter.saveFileName = DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(CharacterSlot.CharacterSlot_02);
        characterSlot2 = saveFileDataWriter.LoadSaveFile();
        
        saveFileDataWriter.saveFileName = DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(CharacterSlot.CharacterSlot_03);
        characterSlot3 = saveFileDataWriter.LoadSaveFile();
        
        saveFileDataWriter.saveFileName = DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(CharacterSlot.CharacterSlot_04);
        characterSlot4 = saveFileDataWriter.LoadSaveFile();
        
        saveFileDataWriter.saveFileName = DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(CharacterSlot.CharacterSlot_05);
        characterSlot5 = saveFileDataWriter.LoadSaveFile();
        
        saveFileDataWriter.saveFileName = DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(CharacterSlot.CharacterSlot_06);
        characterSlot6 = saveFileDataWriter.LoadSaveFile();
        
        saveFileDataWriter.saveFileName = DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(CharacterSlot.CharacterSlot_07);
        characterSlot7 = saveFileDataWriter.LoadSaveFile();
        
        saveFileDataWriter.saveFileName = DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(CharacterSlot.CharacterSlot_08);
        characterSlot8 = saveFileDataWriter.LoadSaveFile();
        
        saveFileDataWriter.saveFileName = DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(CharacterSlot.CharacterSlot_09);
        characterSlot9 = saveFileDataWriter.LoadSaveFile();
        
        saveFileDataWriter.saveFileName = DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(CharacterSlot.CharacterSlot_10);
        characterSlot10 = saveFileDataWriter.LoadSaveFile();
    }
    
    public IEnumerator LoadWorldScene()
    {
        AsyncOperation loadOperation = SceneManager.LoadSceneAsync(worldSceneIndex);
        player.LoadGameDataToCurrentCharacterData(ref currentCharacterData);
        
        yield return null;
    }

    public int GetWorldSceneIndex()
    {
        return worldSceneIndex;
    }

    
}
