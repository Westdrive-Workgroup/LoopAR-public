using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathCreation;
using UnityEngine.SceneManagement;

public class MountainRoadManager : MonoBehaviour
{
    [SerializeField] private GameObject initialSpawnPoint;
    [SerializeField] private PathCreator mainCarPath;

    private void Awake()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    
    private void  OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        ExperimentManager.Instance.SetInitialTransform(initialSpawnPoint.transform.position, initialSpawnPoint.transform.rotation);
        ExperimentManager.Instance.SetCarPath(mainCarPath);
    }
}
