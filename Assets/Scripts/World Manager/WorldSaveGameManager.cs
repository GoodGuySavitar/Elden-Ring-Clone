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

    [SerializeField] private PlayerManager player;

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
    // public CharacterSaveData characterSlot2;
    // public CharacterSaveData characterSlot3;
    // public CharacterSaveData characterSlot4;
    // public CharacterSaveData characterSlot5;
    // public CharacterSaveData characterSlot6;
    // public CharacterSaveData characterSlot7;
    // public CharacterSaveData characterSlot8;
    // public CharacterSaveData characterSlot9;
    // public CharacterSaveData characterSlot10;
    
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

    private void DecideCharacterFileNameBasedOnCharacterSlotBeingUsed()
    {
        switch (currentCharacterSlotBeingUsed)
        {
            case CharacterSlot.CharacterSlot_01:
                saveFileName = "CharacterSlot_01";
                break;
            case CharacterSlot.CharacterSlot_02:
                saveFileName = "CharacterSlot_02";
                break;
            case CharacterSlot.CharacterSlot_03:
                saveFileName = "CharacterSlot_03";
                break;
            case CharacterSlot.CharacterSlot_04:
                saveFileName = "CharacterSlot_04";
                break;
            case CharacterSlot.CharacterSlot_05:
                saveFileName = "CharacterSlot_05";
                break;
            case CharacterSlot.CharacterSlot_06:
                saveFileName = "CharacterSlot_06";
                break;
            case CharacterSlot.CharacterSlot_07:
                saveFileName = "CharacterSlot_07";
                break;
            case CharacterSlot.CharacterSlot_08:
                saveFileName = "CharacterSlot_08";
                break;
            case CharacterSlot.CharacterSlot_09:
                saveFileName = "CharacterSlot_09";
                break;
            case CharacterSlot.CharacterSlot_10:
                saveFileName = "CharacterSlot_10";
                break;
            default:
                break;
        }
    }

    public void CreateNewGame()
    {
        //CREATE A NEW FILE, WITH THE FILE NAME DEPENDING ON THE SLOT WE ARE USING
        DecideCharacterFileNameBasedOnCharacterSlotBeingUsed();

        currentCharacterData = new CharacterSaveData();
    }

    public void LoadGame()
    {
        //LOAD A SAVED FILE, WITH THE NAME DEPENDING ON THE SLOT WE ARE USING
        DecideCharacterFileNameBasedOnCharacterSlotBeingUsed();

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
        DecideCharacterFileNameBasedOnCharacterSlotBeingUsed();
        
        saveFileDataWriter = new SaveFileDataWriter();
        //  GENERALLY WORKS ON MULTIPLE MACHINE TYPES (Application.persistentDataPath)
        saveFileDataWriter.saveDataDirectoryPath = Application.persistentDataPath;
        saveFileDataWriter.saveFileName = saveFileName;
        
        //PASS THE PLAYER INFO, FROM GAME, TO THEIR SAVE FILE 
        player.SaveGameDataToCurrentCharacterData(ref currentCharacterData);
        
        //WRITE THAT INFO ONTO A JSON FILE, SAVED TO THIS MACHINE 
        saveFileDataWriter.CreateNewSaveFile(currentCharacterData);
    }

    public IEnumerator LoadWorldScene()
    {
        AsyncOperation loadOperation = SceneManager.LoadSceneAsync(worldSceneIndex);
        
        yield return null;
    }

    public int GetWorldSceneIndex()
    {
        return worldSceneIndex;
    }
}
