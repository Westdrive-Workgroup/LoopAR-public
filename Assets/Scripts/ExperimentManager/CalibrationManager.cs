using System;
using System.Collections;
using System.Collections.Generic;
using System.Security;
using UnityEngine;
using UnityEngine.SceneManagement;

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
        // EyetrackingManager.Instance.StartCalibration();
        Debug.Log("EyeCalibration activated");
    }

    public void EyeCalibrationSuccessful()
    {
        MainMenu.Instance.EyeCalibrated();
    }
    
    public void EyeValidation()
    {
        SceneManager.LoadScene(1);
        // todo write the GUI in eye validation manager
    }

    public void EyeValidationSuccessful()
    {
        MainMenu.Instance.EyeValidated();
    }

    public void SeatCalibration()
    {
        SceneManager.LoadScene(2);
        // todo write the GUI in seat calibration manager
    }

    public void SeatCalibrationSuccessful()
    {
        MainMenu.Instance.SeatCalibrated();
    }

    public void StartTestDrive()
    {
        SceneManager.LoadScene(3);
    }
    
    public void StartTestDriveSuccessful()
    {
        MainMenu.Instance.TrainingDone();
    }

    public void GoToTheExperiment()
    {
        SceneManager.LoadScene(4);
    }

    public void RestartProject()
    {
        MainMenu.Instance.ReStartMainMenu();
    }
}
