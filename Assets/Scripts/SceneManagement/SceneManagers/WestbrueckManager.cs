using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WestbrueckManager : MonoBehaviour
{
    public static WestbrueckManager Instance { get; private set; }
    
    [SerializeField] private GameObject participantsCar;
    [SerializeField] private GameObject terrain;
    [SerializeField] private GameObject roadNetwork;
    [SerializeField] private GameObject remainingAssets;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }
   
    public GameObject GetParticipantsCar()
    {
        return participantsCar != null ? participantsCar : null;
    }
    
    public GameObject GetTerrain()
    {
        return terrain;
    }
    
    public GameObject GetRoadNetwork()
    {
        return roadNetwork;
    }
    
    public GameObject GetRemainingAssets()
    {
        return remainingAssets;
    }
}
