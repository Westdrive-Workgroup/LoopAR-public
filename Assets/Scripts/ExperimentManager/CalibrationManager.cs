using System;
using System.Collections;
using System.Collections.Generic;
using System.Security;
using UnityEngine;

[DisallowMultipleComponent]
public class CalibrationManager : MonoBehaviour
{
    public static CalibrationManager Instance { get; private set; }

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
        MainMenu.Instance.EyeCalibrated();
    }
    
    public void EyeValidation()
    {
        SceneLoader.Instance.AsyncLoad(1);
        // todo write the GUI in eye validation manager
    }

    public void EyeValidationSuccessful()
    {
        MainMenu.Instance.EyeValidated();
        // todo get the confirmation from eye validation
    }

    public void SeatCalibration()
    {
        SceneLoader.Instance.AsyncLoad(2);
    }

    public void SeatCalibrationSuccessful()
    {
        MainMenu.Instance.SeatCalibrated();
    }

    public void StartTestDrive()
    {
        SceneLoader.Instance.AsyncLoad(3);
    }
    
    public void StartTestDriveSuccessful()
    {
        MainMenu.Instance.TrainingDone();
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
}
