using System.Collections;
using System.Collections.Generic;
using System;
using System.Data;
using System.IO;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class SaveFileDataWriter:MonoBehaviour
{
public string saveDataDirectoryPath = "";
    public string saveFileName = "";
    private bool shouldReadFile = false;
    
    // BEFORE WE CREATE A FILE, WE CHECK TO SEE IF THE FILE ALREADY EXISTS( MAX 10 SLOTS) 
    public bool CheckToSeeIfFileExists()
    {
        if (File.Exists(Path.Combine(saveDataDirectoryPath, saveFileName)))
        {
            return true;
        }
        else
        { 
            return false;
        }
    }

    // USED TO DELETE A SAVED FILE
    public void DeleteSaveFile()
    {
        File.Delete(Path.Combine(saveDataDirectoryPath,saveFileName));
    }

    // USED TO CREATE A SAVE FILE UPON STARTING A NEW GAME  
    public void CreateNewSaveFile(CharacterSaveData characterData)
    {
        // MAKE A PATH TO SAVE THE FILE( A LOCATION ON THE MACHINE)
        string savePath = Path.Combine(saveDataDirectoryPath, saveFileName);
        try
        {
            // CREATE THE DIRECTORY TO SAVE THE FILE, IF IT DOESNT EXIST ALREADY
            Directory.CreateDirectory(Path.Combine(saveDataDirectoryPath, saveFileName));
            Debug.Log("CREATING SAVE FILE AT SAVE PATH: " + savePath);
            Debug.Log(saveDataDirectoryPath);

            // SERIALIZE THE C# GAME DATA INTO JSON
            string dataToStore = JsonUtility.ToJson(characterData, true);
            Debug.Log("Json to h");
            
            // WRITING THE FILE INTO OUR SYSTEM
            using (FileStream stream = new FileStream(savePath, FileMode.Create))
            {
                Debug.LogError("ok");
                using (StreamWriter fileWriter = new StreamWriter(stream))
                {
                    fileWriter.Write(dataToStore);
                    Debug.LogError("yaha");
                }
            }
           
        }
        catch (Exception ex)
        {
            Debug.LogError("ERROR WHILE TRYING TO SAVE CHARACTER DATA, GAME NOT SAVED " + savePath + "\n" + ex);
        }

        
        
    }
    
    // USED TO LOAD A SAVE FILE UPON LOADING A PREVIOUS GAME

    public CharacterSaveData LoadSaveFile()
    {
        CharacterSaveData characterData = null;
        
        // MAKE A PATH TO LOAD THE FILE( A LOCATION ON THE MACHINE)
        string loadPath = Path.Combine(saveDataDirectoryPath, saveFileName);

        if (File.Exists(loadPath))
        {
            try
            {
                string dataToLoad = "";

                using (FileStream stream = new FileStream(loadPath, FileMode.Open))
                {
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        dataToLoad = reader.ReadToEnd();
                    }
                }
                
                //DESERIALIZE THE DATA FROM JSON BACK TO UNITY
                characterData = JsonUtility.FromJson<CharacterSaveData>(dataToLoad);
            }
            
            catch (Exception ex)
            {
                
            }
        }

        return characterData;
    }
}
