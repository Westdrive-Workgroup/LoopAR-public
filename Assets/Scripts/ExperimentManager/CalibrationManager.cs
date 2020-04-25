using System;
using System.Collections;
using System.Collections.Generic;
using System.Security;
using UnityEngine;

[DisallowMultipleComponent]
public class CalibrationManager : MonoBehaviour
{
    public static CalibrationManager Instance { get; private set; }
    private int _state;


    private bool _eyeTrackerCalibrationSuccessful;
    private bool _eyeTrackerValidationSuccessful;
    //...


    private Vector3 _eyeValidationError;
    private Vector3 _seatCalibrationOffset;

    private void Awake()
    {
        //singleton pattern a la Unity
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);         //the Eyetracking Manager should be persitent by changing the scenes maybe change it on the the fly
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void EyeCalibration()
    {
        EyetrackingManager.Instance.StartCalibration();
    }

    public void EyeCalibrationSuccessful()
    {
        _state = 1;
    }
    
    public void EyeValidation()
    {
        SceneLoader.Instance.AsyncLoad(1);
    }

    public void EyeValidationSuccessful()
    {
        _state = 2;
    }

    public void SeatCalibration()
    {
        SceneLoader.Instance.AsyncLoad(2);
    }

    public void SeatCalibrationSuccessful()
    {
        _state = 3;
    }

    public void StartTestDrive()
    {
        SceneLoader.Instance.AsyncLoad(3);
    }
    
    public void StartTestDriveSuccessful()
    {
        _state = 4;
    }

    public void GoToTheExperiment()
    {
        SceneLoader.Instance.AsyncLoad(4);
    }

    public void AbortExperiment()
    {
        SceneLoader.Instance.AsyncLoad(0);
        MainMenu.Instance.ReStartMainMenu();
    }

    public int GetMenuState()
    {
        return _state;
    }

    public bool GetEyeTrackerCalibrationState()
    {
        return _eyeTrackerCalibrationSuccessful;
    }
}
