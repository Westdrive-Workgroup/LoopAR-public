using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using UnityEngine;

[DisallowMultipleComponent]
public class MainMenu : MonoBehaviour
{
    public static MainMenu Instance { get; private set; }

    [Space] [Header("Scene Type")]
    [SerializeField] private bool vRScene;
    private enum Section
    {
        MainMenu = 0,
        EyeCalibration = 1,
        EyeValidation = 2,
        SeatCalibration = 3,
        TrainingBlock = 4,
        MainExperiment = 5
    }

    private Section _section;

    private bool _eyeCalibrated;
    private bool _eyeValidated;
    private bool _seatCalibrated;
    private bool _trained;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        
        _section = Section.MainMenu;
    }

    private void Start()
    {
        _section = Section.MainMenu;
        _eyeCalibrated = CalibrationManager.Instance.GetEyeTrackerCalibrationState();
        _eyeValidated = CalibrationManager.Instance.GetEyeTrackerValidationState();
        _seatCalibrated = CalibrationManager.Instance.GetSeatCalibrationState();
        _trained = CalibrationManager.Instance.GetTestDriveState();
    }

    public void OnGUI()
    {
        float height = Screen.height;
        float width = Screen.width;
        
        float xForButtons = width / 12f;
        float yForButtons = height / 7f;
        
        float xForLable = (width / 2f);
        float yForLable = height/1.35f;

        float buttonWidth = 200f;
        float buttonHeight = 30f;
        float heightDifference = 40f;
        
        int labelFontSize = 33;

        
        // Lable
        GUI.color = Color.white;
        GUI.skin.label.fontSize = labelFontSize;
        GUI.skin.label.fontStyle = FontStyle.Bold;
        
        GUI.Label(new Rect(xForLable, yForLable, 500, 100),  "Welcome to Westdrive LoopAR");
        
        GUI.backgroundColor = Color.red;
        GUI.color = Color.white;
        
        if (GUI.Button(new Rect(xForButtons, yForButtons + (heightDifference*9.5f), buttonWidth, buttonHeight), "Quit"))
        {
            Application.Quit();
        }
        
        
        if (vRScene)
        {
            // Reset Button
            GUI.backgroundColor = Color.yellow;
            GUI.color = Color.white;
        
            if (GUI.Button(new Rect(xForButtons*9, yForButtons, buttonWidth, buttonHeight), "Reset"))
            {
                _section = Section.MainMenu;
            }
            
            
            // Buttons
            GUI.backgroundColor = Color.cyan;
            GUI.color = Color.white;
            
            if (_section == Section.MainMenu)
            {
                if (GUI.Button(new Rect(xForButtons, yForButtons, buttonWidth, buttonHeight), "Eye Calibration"))
                {
                    _section = Section.EyeCalibration;
                    CalibrationManager.Instance.EyeCalibration();
                }
            }
            else if (_eyeCalibrated && !_eyeValidated)
            {
                if (GUI.Button(new Rect(xForButtons, yForButtons, buttonWidth, buttonHeight), "Eye Validation"))
                {
                    _section = Section.EyeValidation;
                    CalibrationManager.Instance.EyeValidation();
                }
            }
            else if (_eyeValidated && !_seatCalibrated)
            {
                if (GUI.Button(new Rect(xForButtons, yForButtons, buttonWidth, buttonHeight),
                    "Seat Calibration"))
                {
                    _section = Section.SeatCalibration;
                    CalibrationManager.Instance.SeatCalibration();
                }
            }
            else if (_seatCalibrated && !_trained)
            {
                if (GUI.Button(new Rect(xForButtons, yForButtons, buttonWidth, buttonHeight),
                    "Test Drive Scene"))
                {
                    _section = Section.TrainingBlock;
                    CalibrationManager.Instance.StartTestDrive();
                }
            }
            else if (_trained)
            {
                if (GUI.Button(new Rect(xForButtons, yForButtons, buttonWidth, buttonHeight), "Main Experiment"))
                {
                    _section = Section.MainExperiment;    
                    CalibrationManager.Instance.GoToTheExperiment();
                }
            }
        }
        else
        {
            GUI.backgroundColor = Color.cyan;
            GUI.color = Color.white;
            
            if (GUI.Button(new Rect(xForButtons, yForButtons, buttonWidth, buttonHeight), "Main Experiment"))
            {
                _section = Section.MainExperiment;    
                CalibrationManager.Instance.GoToTheExperiment();
                Debug.Log("Main experiment clicked");
            }
        }
    }

    public void ReStartMainMenu()
    {
        _section = Section.MainMenu;
    }
}
