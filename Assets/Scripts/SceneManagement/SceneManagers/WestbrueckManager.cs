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

    private GameObject[] _sceneAssets;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }

        _sceneAssets = new[] {terrain, roadNetwork, remainingAssets};
    }

    public IEnumerator ActivateAssets()
    {
        foreach (var asset in _sceneAssets)
        {
            yield return ActivateGameObjects(asset);
        }    
    }

    IEnumerator ActivateGameObjects(GameObject obj)
    {
        yield return null;
        obj.SetActive(true);
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
