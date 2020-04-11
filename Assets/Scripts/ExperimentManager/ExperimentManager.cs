using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class ExperimentManager : MonoBehaviour
{
    public static ExperimentManager Instance { get; private set; }
    [SerializeField] private GameObject participantsCar;
    [SerializeField] private Camera _camera;
    [SerializeField] private Camera _firstPersonCamera;

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
        BrokenCar
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
    }


    void Start()
    {
        Debug.Log(_activationTriggers.Count + " start of start");
        
        RunMainMenu();
        InformTriggers();
       
        Debug.Log(_activationTriggers.Count + " end of start");
    }

    
    private void Update()
    {  
        Debug.Log(_activationTriggers.Count);
        if (_scene == Scene.EndOfExperiment)
        {
            participantsCar.SetActive(false);
        }
    }


    // main menu
    private void RunMainMenu()
    { 
        _scene = Scene.MainMenu;
        participantsCar.SetActive(false);
        _firstPersonCamera.enabled = false;
        _camera.transform.position = Vector3.zero;
        _camera.transform.rotation = Quaternion.Euler(0,-90,0);
    }

    // inform all triggers to disable their gameobjects at the beginning of the experiment

    private void InformTriggers()
    {
        foreach (var trigger in _activationTriggers)
        {
            trigger.DeactivateTheGameObjects();
        }
    }


    public void OnGUI()
    {

        if (_scene == Scene.MainMenu)
        {
            if (GUI.Button(new Rect(500, 210, 200, 30), "Validation and Calibration"))
            {
                //todo call eye tracker validation and find a good name for this button
                Debug.Log("Clicked validation and calibration");
            }

            if (GUI.Button(new Rect(500, 250, 200, 30), "Start the experiment"))
            {
                StartExperiment();
            }
        }
        else
        {
            GUI.enabled = false;
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
        _firstPersonCamera.enabled = true;
        participantsCar.SetActive(true);
    }
    
    //todo inform the controllers
    //todo call data saving (maybe)
    
}
