using System.Collections;
using System.Collections.Generic;
using PathCreation;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MountainRoadManager : MonoBehaviour
{
    public static MountainRoadManager Instance { get; private set; }
    
    [Space] [Header("Car and Path options")]
    [SerializeField] private GameObject participantsCar;
    [SerializeField] private PathCreator mainCarPath;
    [SerializeField] private float curveDetectorStepAhead = 0.01f;
    [SerializeField] private float precision = 0.005f;
    [SerializeField] private float trackerSensitivity = 5f;
    
    [Space] [Header("General GameObjects")]
    [SerializeField] private GameObject terrain;
    [SerializeField] private GameObject roadNetwork;
    [SerializeField] private GameObject remainingAssets;

    private GameObject[] _sceneAssets;
    private bool _activateObjects;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }

        _sceneAssets = new[] {terrain, roadNetwork, remainingAssets};
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
