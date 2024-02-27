using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldSoundFXManager : MonoBehaviour
{
   public static WorldSoundFXManager Instance;

   [Header("Action sound fx")]
   public AudioClip rollSFX;
   
   private void Awake()
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

   private void Start()
   {
      DontDestroyOnLoad(gameObject);
   }
}
