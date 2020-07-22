using System;
using System.Collections;
using System.Collections.Generic;
using LOD_avatar.scripts.Manager;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ApplicationManager : MonoBehaviour
{
    public static ApplicationManager Instance { get; private set; }

    [SerializeField] private GameObject mainExperimentGroup;
    
    private List<GameObject> _children;
    private ActivationHandler _activationHandler;
    private string _sceneName;
    private bool _componentsOff;
    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        _children = new List<GameObject>();

        foreach (Transform child in mainExperimentGroup.transform)
        {
            _children.Add(child.gameObject);
        }
        
        _sceneName = SceneManager.GetActiveScene().name;
        SetComponents();
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (_sceneName == SceneManager.GetActiveScene().name) return;
        _sceneName = SceneManager.GetActiveScene().name;
        SetComponents();
    }

    private void SetComponents()
    {
        switch (_sceneName)
        {
            case "MainMenu":
            case "EyetrackingValidation":
                TurnOffSpecificExperimentComponents(false);
                break;
            case "SceneLoader":
            case "SeatCalibrationScene":
            case "TestDrive2.0":
                TurnOffSpecificExperimentComponents(true);
                break;
            case "safe-mountainroad01":
            case "Westbrueck":
            case "countryroad01":
            case "Autobahn":
                TurnOnAllComponents();
                break;
        }
    }

    private void TurnOffSpecificExperimentComponents(bool carState)
    {
        if (_componentsOff) return;
        
        foreach (var item in _children)
        {
            if (item.GetComponent<ParticipantsCar>())
            {
                item.gameObject.SetActive(carState);
            }
                
            item.gameObject.SetActive(false);
        }
        _componentsOff = true;
    }
    
    private void TurnOnAllComponents()
    {
        if (!_componentsOff) return;
        
        foreach (var item in _children)
        {
            item.gameObject.SetActive(true);
        }
        _componentsOff = false;
    }
}