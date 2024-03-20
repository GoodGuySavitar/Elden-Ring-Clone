using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]

// SINCE WE WANT T0 REFERENCE THIS DATA FOR EVERY SAVE FILE, THIS SCRIPT IS NOT A MONOBEHAVIOUR AND IS INSTEAD A SERIALIZABLE
public class CharacterSaveData
{
    [Header("Character name")]
    public string characterName;

    [Header("Time player")] 
    public float secondsPlayed;

    //WE CAN ONLY USE BASIC VARIABLE TYPES WITH JSON(FLOAT, INT, STRING, BOOL ETC.)
    [Header("World Coordinates ")] 
    public float xPosition;
    public float yPosition;
    public float zPosition;
}
