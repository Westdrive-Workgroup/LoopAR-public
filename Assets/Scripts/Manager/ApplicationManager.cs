using System;
using System.Collections;
using System.Collections.Generic;
using LOD_avatar.scripts.Manager;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ApplicationManager : MonoBehaviour
{
    #region Fields

    public static ApplicationManager Instance { get; private set; }

    [SerializeField] private GameObject mainExperimentGroup;
    
    private List<GameObject> _children;
    private ActivationHandler _activationHandler;
    private string _sceneName;
    private bool _componentsOff;
    
    private string _menuSection;
    private string _experimentalCondition;
    
    #endregion

    #region PrivateMethods

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

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            SavingManager.Instance.StopAndSaveData(SceneManager.GetActiveScene().name);
            Application.Quit();
        }
    }

    private void SetComponents()
    {
        switch (_sceneName)
        {
            case "MainMenu":
                CameraManager.Instance.FadeIn();
                MainMenu.Instance.GetCanvas().worldCamera = Camera.main;
                TurnOffSpecificExperimentComponents();
                break;
            case "SceneLoader":
            case "SeatCalibrationScene":
            case "TrainingScene":
                TurnOffSpecificExperimentComponents();
                break;
            case "MountainRoad":
            case "Westbrueck":
            case "CountryRoad":
            case "Autobahn":
                TurnOnAllComponents();
                ExperimentManager.Instance.SetExperimentalCondition(_experimentalCondition);
                break;
            case "EyetrackingValidation":
                TurnOffSpecificExperimentComponents();
                EyeValidationManager.Instance.GetRelativeFixedPoint().gameObject.transform.SetParent(Camera.main.transform);
                EyeValidationManager.Instance.GetRelativeFixedPoint().transform.localPosition = new Vector3(0, 0, 5);
                EyeValidationManager.Instance.GetRelativeFixedPoint().transform.localEulerAngles = new Vector3(0, 0, 0);
                break;
        }
    }

    private void TurnOffSpecificExperimentComponents()
    {
        if (_componentsOff) return;
        
        foreach (var item in _children)
        {
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

    #endregion

    #region PublicMethods

    public void StoreMainMenuLastState(string section)
    {
        _menuSection = section;
    }

    public void SetExperimentalCondition(string condition)
    {
        _experimentalCondition = condition;
    }
    
    public string GetLastMainMenuState()
    {
        return _menuSection;
    }
    
    #endregion
}