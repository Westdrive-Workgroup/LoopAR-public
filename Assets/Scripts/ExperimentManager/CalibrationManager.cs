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
    private bool _seatCalibrationSuccessful;
    private bool _testDriveSuccessful;


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
        _eyeTrackerCalibrationSuccessful = true;
    }
    
    public void EyeValidation()
    {
        SceneLoader.Instance.AsyncLoad(1);
    }

    public void EyeValidationSuccessful()
    {
        _eyeTrackerValidationSuccessful = true;
    }

    public void SeatCalibration()
    {
        SceneLoader.Instance.AsyncLoad(2);
    }

    public void SeatCalibrationSuccessful()
    {
        _seatCalibrationSuccessful = true;
    }

    public void StartTestDrive()
    {
        SceneLoader.Instance.AsyncLoad(3);
    }
    
    public void TestDriveSuccessful()
    {
        _testDriveSuccessful = true;
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

    public bool GetEyeTrackerCalibrationState()
    {
        return _eyeTrackerCalibrationSuccessful;
    }

    public bool GetEyeTrackerValidationState()
    {
        return _eyeTrackerValidationSuccessful;
    }

    public bool GetSeatCalibrationState()
    {
        return _seatCalibrationSuccessful;
    }

    public bool GetTestDriveState()
    {
        return _testDriveSuccessful;
    }
}
