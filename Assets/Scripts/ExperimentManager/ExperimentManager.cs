﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using Valve.VR;

[DisallowMultipleComponent]
public class ExperimentManager : MonoBehaviour
{
    public static ExperimentManager Instance { get; private set; }
    
    [Space] [Header("Necessary Objects")]
    [SerializeField] private GameObject participantsCar;
    [SerializeField] private Camera _camera;
    [SerializeField] private Camera firstPersonCamera;
    [SerializeField] private VRCam _vrCamera;

    private SavingManager _savingManager;
    // registers in which scene or state the experiment is
    private enum Scene
    {
        MainMenu,
        CountryRoad,
        MountainRoad,
        Autobahn,
        City,
        EndOfExperiment
    }
    
    // todo use them
    private enum Event
    {
        Deer,
        BrokenCar,
        ChildAtTheMarket
    }
    
    private Scene _scene;
    
    private List<ActivationTrigger> _activationTriggers;

    private void Awake()
    {
        _activationTriggers = new List<ActivationTrigger>();
        
        //singleton pattern a la Unity
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);         //the Traffic Manager should be persitant by changing the scenes maybe change it on the the fly
        }
        else
        {
            Destroy(gameObject);
        }
        
        _savingManager = SavingManager.Instance;
        _savingManager.SetParticipantCar(participantsCar);
    }


    void Start()
    {
        
        InformTriggers();
        
        if (_activationTriggers.Count == 0)
        {
            Debug.Log("<color=red>Error: </color>Please ensure that ActivationTrigger is being executed before ExperimentManager if there are triggers present in the scene.");
        }

        if (EyetrackingManager.Instance == null)
        {
            Debug.Log("<color=red>Error: </color>EyetrackingManager should be present in the scene.");
        }
        
        RunMainMenu();
    }
    

    // main menu
    private void RunMainMenu()
    { 
        _scene = Scene.MainMenu;
        if (_vrCamera == null)
        {
            firstPersonCamera.enabled = false;
            
        }
        else
        {
            _vrCamera.SetPosition(_camera.transform.position);
        }
       // participantsCar.SetActive(false);
        
        //_camera.transform.position = Vector3.zero;
        //_camera.transform.rotation = Quaternion.Euler(0,-90,0);
    }

    // inform all triggers to disable their gameobjects at the beginning of the experiment

    private void InformTriggers()
    {
        foreach (var trigger in _activationTriggers)
        {
            trigger.DeactivateTheGameObjects();
        }
    }


    // Reception desk for ActivationTriggers to register themselves

    public void RegisterToExperimentManager(ActivationTrigger listener)
    {
        _activationTriggers.Add(listener);
    }


    // starting the experiment

    private void StartExperiment()
    {
        _scene = Scene.CountryRoad;
        _camera.enabled = false;
        
        if (_vrCamera == null)
        {
            firstPersonCamera.enabled = true;
        }
        else
        {
            Debug.Log("vr ");
            _vrCamera.Seat();
        }
        
        participantsCar.SetActive(true);
        SavingManager.Instance.StartRecordingData();
    }

    // ending the experiment
    public void EndTheExperiment()
    {
        SavingManager.Instance.StopRecordingData();
        SavingManager.Instance.SaveData();
        _scene = Scene.EndOfExperiment;
        //todo activate data saving
        // EyetrackingManager.Instance.DataSaving();
        FadeOut();
        if (_vrCamera == null)
        {
            firstPersonCamera.enabled = false;
        }
        else
        {
            _vrCamera.UnSeat();
        }
        _camera.enabled=true;
        participantsCar.SetActive(false);
    }


    // used for respawning the participant car in case of screwing up
    private void FadeIn()
    {
        SteamVR_Fade.Start(Color.clear, 2f);
    }


    // usage: in case the participant screws up and end of the experiment
    private void FadeOut()
    {
        SteamVR_Fade.Start(Color.black, 2f);
    }


    public GameObject GetParticipantCar()
    {
        return participantsCar;
    }

    public void OnGUI()
    {
        float height = Screen.height;
        float width = Screen.width;
        
        float xForButtons = (width / 2f) - 100f;
        float yForButtons = height / 2f;
        
        float xForLable = (Screen.width / 2f) - 250f;
        float yForLable = yForButtons - (height / 2.5f);

        float buttonWidth = 200f;
        float buttonHeight = 30f;
        float heightDifference = 40f;
        
        int labelFontSize = 33;

        if (_scene == Scene.MainMenu)
        {
            // Lable
            GUI.color = Color.black;
            GUI.skin.label.fontSize = labelFontSize;
            GUI.skin.label.fontStyle = FontStyle.Bold;
            GUI.Label(new Rect(xForLable, yForLable, 500, 100),  "Welcome to Westdrive LoopAR");
            
            // Buttons
            GUI.backgroundColor = Color.blue;
            GUI.color = Color.white;
            
            if (GUI.Button(new Rect(xForButtons, yForButtons - heightDifference, buttonWidth, buttonHeight), "Calibration"))
            {
                EyetrackingManager.Instance.StartCalibration();
            }

            if (GUI.Button(new Rect(xForButtons, yForButtons, buttonWidth, buttonHeight), "Validation"))
            {
                EyetrackingManager.Instance.StartValidation();
            }

            if (GUI.Button(new Rect(xForButtons, yForButtons + heightDifference, buttonWidth, buttonHeight), "Start the experiment"))
            {
                StartExperiment();
            }
        }
        else
        {
            GUI.enabled = false;
        }
    }
}
