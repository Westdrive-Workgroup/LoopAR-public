using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathCreation;
using UnityEngine.SceneManagement;

public class CountryRoadManager : MonoBehaviour
{
    [SerializeField] private GameObject initialSpawnPoint;
    [SerializeField] private PathCreator mainCarPath;

    private void Awake()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    
    private void  OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        ExperimentManager.Instance.SetInitialSpawnPositionAndRotation(initialSpawnPoint.transform.position, initialSpawnPoint.transform.rotation);
        ExperimentManager.Instance.SetCarPath(mainCarPath);
    }
}
