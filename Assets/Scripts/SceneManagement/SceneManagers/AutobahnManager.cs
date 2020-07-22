using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AutobahnManager : MonoBehaviour
{
    [SerializeField] private GameObject participantsCar;

    private void Awake()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    
    private void  OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        ExperimentManager.Instance.SetParticipantsCar(participantsCar);
    }
}
