using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WestbrueckManager : MonoBehaviour
{
    public static WestbrueckManager Instance { get; private set; }
    
    [SerializeField] private GameObject participantsCar;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }
   
    public GameObject GetParticipantsCar()
    {
        return participantsCar;
    }
}
