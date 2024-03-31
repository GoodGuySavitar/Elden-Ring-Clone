using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Application = UnityEngine.Device.Application;

public class UI_Character_Save_Slot : MonoBehaviour
{
    private SaveFileDataWriter saveFileDataWriter;

    [Header("Game Slot")] 
    public CharacterSlot characterSlot;

    [Header("Character Slot Info")] 
    public TextMeshProUGUI characterName;
    public TextMeshProUGUI timePlayed;

    private void OnEnable()
    {
        LoadSaveSlots();
    }

    private void LoadSaveSlots()
    {
        saveFileDataWriter = new SaveFileDataWriter();
        saveFileDataWriter.saveDataDirectoryPath = Application.persistentDataPath;
        
        //  SAVE SLOT 01
        if (characterSlot == CharacterSlot.CharacterSlot_01)
        {
            saveFileDataWriter.saveFileName =
                WorldSaveGameManager.Instance.DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(characterSlot);

            //IF THE FILE EXISTS, GET INFO FROM IT
            if (saveFileDataWriter.CheckToSeeIfFileExists())
            {
                characterName.text = WorldSaveGameManager.Instance.characterSlot1.characterName;
            }
            // IF IT DOESNT EXIST, DISABLE THIS GAME OBJECT 
             else
             {
                 gameObject.SetActive(false);
             }
        }
        
        // SAVE SLOT 02
        else if (characterSlot == CharacterSlot.CharacterSlot_02)
        {
            saveFileDataWriter.saveFileName =
                WorldSaveGameManager.Instance.DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(characterSlot);

            //IF THE FILE EXISTS, GET INFO FROM IT
            if (saveFileDataWriter.CheckToSeeIfFileExists())
            {
                characterName.text = WorldSaveGameManager.Instance.characterSlot2.characterName;
            }
            // IF IT DOESNT EXIST, DISABLE THIS GAME OBJECT 
            else
            {
                gameObject.SetActive(false);
            }
        }
        
        // SAVE SLOT 03
        else if (characterSlot == CharacterSlot.CharacterSlot_03)
        {
            saveFileDataWriter.saveFileName =
                WorldSaveGameManager.Instance.DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(characterSlot);

            //IF THE FILE EXISTS, GET INFO FROM IT
            if (saveFileDataWriter.CheckToSeeIfFileExists())
            {
                characterName.text = WorldSaveGameManager.Instance.characterSlot3.characterName;
            }
            // IF IT DOESNT EXIST, DISABLE THIS GAME OBJECT 
            else
            {
                gameObject.SetActive(false);
            }
        }
        
        // SAVE SLOT 04
        else if (characterSlot == CharacterSlot.CharacterSlot_04)
        {
            saveFileDataWriter.saveFileName =
                WorldSaveGameManager.Instance.DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(characterSlot);

            //IF THE FILE EXISTS, GET INFO FROM IT
            if (saveFileDataWriter.CheckToSeeIfFileExists())
            {
                characterName.text = WorldSaveGameManager.Instance.characterSlot4.characterName;
            }
            // IF IT DOESNT EXIST, DISABLE THIS GAME OBJECT 
            else
            {
                gameObject.SetActive(false);
            }
        }
        
        // SAVE SLOT 05 
        else if (characterSlot == CharacterSlot.CharacterSlot_05)
        {
            saveFileDataWriter.saveFileName =
                WorldSaveGameManager.Instance.DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(characterSlot);

            //IF THE FILE EXISTS, GET INFO FROM IT
            if (saveFileDataWriter.CheckToSeeIfFileExists())
            {
                characterName.text = WorldSaveGameManager.Instance.characterSlot5.characterName;
            }
            // IF IT DOESNT EXIST, DISABLE THIS GAME OBJECT 
            else
            {
                gameObject.SetActive(false);
            }
        }
        
        // SAVE SLOT 06
        else if (characterSlot == CharacterSlot.CharacterSlot_06)
        {
            saveFileDataWriter.saveFileName =
                WorldSaveGameManager.Instance.DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(characterSlot);

            //IF THE FILE EXISTS, GET INFO FROM IT
            if (saveFileDataWriter.CheckToSeeIfFileExists())
            {
                characterName.text = WorldSaveGameManager.Instance.characterSlot6.characterName;
            }
            // IF IT DOESNT EXIST, DISABLE THIS GAME OBJECT 
            else
            {
                gameObject.SetActive(false);
            }
        }
        
        // SAVE SLOT 07
        else if (characterSlot == CharacterSlot.CharacterSlot_07)
        {
            saveFileDataWriter.saveFileName =
                WorldSaveGameManager.Instance.DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(characterSlot);

            //IF THE FILE EXISTS, GET INFO FROM IT
            if (saveFileDataWriter.CheckToSeeIfFileExists())
            {
                characterName.text = WorldSaveGameManager.Instance.characterSlot7.characterName;
            }
            // IF IT DOESNT EXIST, DISABLE THIS GAME OBJECT 
            else
            {
                gameObject.SetActive(false);
            }
        }
        
        // SAVE SLOT 08
        else if (characterSlot == CharacterSlot.CharacterSlot_08)
        {
            saveFileDataWriter.saveFileName =
                WorldSaveGameManager.Instance.DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(characterSlot);

            //IF THE FILE EXISTS, GET INFO FROM IT
            if (saveFileDataWriter.CheckToSeeIfFileExists())
            {
                characterName.text = WorldSaveGameManager.Instance.characterSlot8.characterName;
            }
            // IF IT DOESNT EXIST, DISABLE THIS GAME OBJECT 
            else
            {
                gameObject.SetActive(false);
            }
        }
        
        // SAVE SLOT 09
        else if (characterSlot == CharacterSlot.CharacterSlot_09)
        {
            saveFileDataWriter.saveFileName =
                WorldSaveGameManager.Instance.DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(characterSlot);

            //IF THE FILE EXISTS, GET INFO FROM IT
            if (saveFileDataWriter.CheckToSeeIfFileExists())
            {
                characterName.text = WorldSaveGameManager.Instance.characterSlot9.characterName;
            }
            // IF IT DOESNT EXIST, DISABLE THIS GAME OBJECT 
            else
            {
                gameObject.SetActive(false);
            }
        }
        
        // SAVE SLOT 10
        else if (characterSlot == CharacterSlot.CharacterSlot_10)
        {
            saveFileDataWriter.saveFileName =
                WorldSaveGameManager.Instance.DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(characterSlot);

            //IF THE FILE EXISTS, GET INFO FROM IT
            if (saveFileDataWriter.CheckToSeeIfFileExists())
            {
                characterName.text = WorldSaveGameManager.Instance.characterSlot10.characterName;
            }
            // IF IT DOESNT EXIST, DISABLE THIS GAME OBJECT 
            else
            {
                gameObject.SetActive(false);
            }
        }
    }

    public void LoadGameFromCharacterSaveSlots()
    {
        WorldSaveGameManager.Instance.currentCharacterSlotBeingUsed = characterSlot;
        WorldSaveGameManager.Instance.LoadGame();
    }
}
