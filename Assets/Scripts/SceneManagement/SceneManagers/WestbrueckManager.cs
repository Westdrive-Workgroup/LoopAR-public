using System;
using System.Collections;
using System.Collections.Generic;
using PathCreation;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WestbrueckManager : MonoBehaviour
{
    public static WestbrueckManager Instance { get; private set; }
    
    [Space] [Header("Car and Path options")]
    [SerializeField] private GameObject participantsCar;
    [SerializeField] private GameObject seatPosition;
    [SerializeField] private PathCreator mainCarPath;
    [SerializeField] private float curveDetectorStepAhead = 0.008f;
    [SerializeField] private float precision = 0.002f;
    [SerializeField] private float trackerSensitivity = 4f;
    
    [Space] [Header("General GameObjects")]
    [SerializeField] private GameObject terrain;
    [SerializeField] private GameObject roadNetwork;
    [SerializeField] private GameObject assetCluster1;
    [SerializeField] private GameObject assetCluster2;
    [SerializeField] private GameObject assetCluster3;

    private GameObject[] _sceneAssets;
    private bool _activateObjects;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }

        _sceneAssets = new[] {/*terrain, */roadNetwork, assetCluster1, assetCluster2, assetCluster3};
    }

    public void ActivateGameObjects(bool activationState)
    {
        _activateObjects = activationState;
        StartCoroutine(ActivateAssets());
    }
    
    IEnumerator ActivateAssets()
    {
        foreach (var asset in _sceneAssets)
        {
            yield return ActivateEachGameObject(asset);
        }
    }

    IEnumerator ActivateEachGameObject(GameObject obj)
    {
        yield return null;
        obj.SetActive(_activateObjects);
    }
    
    public GameObject GetParticipantsCar()
    {
        return participantsCar != null ? participantsCar : null;
    }
    
    public GameObject GetSeatPosition()
    {
        return seatPosition != null ? seatPosition : null;
    }
    
    public PathCreator GetCarPath()
    {
        return mainCarPath;
    }

    public float GetCurveDetectorStepAhead()
    {
        return curveDetectorStepAhead;
    }
    
    public float GetPrecision()
    {
        return precision;
    }
    
    public float GetTrackerSensitivity()
    {
        return trackerSensitivity;
    }
}
