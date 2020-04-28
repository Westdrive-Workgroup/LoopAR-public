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
        MainMenu,
        IDGeneration,
        EyeCalibration,
        EyeValidation,
        SeatCalibration,
        TrainingBlock,
        MainExperiment
    }

    private Section _section;

    private CalibrationManager _calibrationManager;

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
        _calibrationManager = CalibrationManager.Instance;
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

        // Quit
        GUI.backgroundColor = Color.red;
        GUI.color = Color.white;
        
        if (GUI.Button(new Rect(xForButtons, yForButtons + (heightDifference*9.5f), buttonWidth, buttonHeight), "Quit"))
        {
            Application.Quit();
        }
        
        // Lable
        GUI.color = Color.white;
        GUI.skin.label.fontSize = labelFontSize;
        GUI.skin.label.fontStyle = FontStyle.Bold;

        GUI.Label(new Rect(xForLable, yForLable, 500, 100),  "Welcome to Westdrive LoopAR");
        
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
            
            if (!_calibrationManager.GetParticipantUUIDState())
            {
                if (GUI.Button(new Rect(xForButtons, yForButtons, buttonWidth, buttonHeight), "Generate Participant ID"))
                {
                    _section = Section.IDGeneration;
                    CalibrationManager.Instance.GenerateID();
                }
            }
            else if (_calibrationManager.GetParticipantUUIDState() && !_calibrationManager.GetEyeTrackerCalibrationState())
            {
                if (GUI.Button(new Rect(xForButtons, yForButtons, buttonWidth, buttonHeight), "Eye Calibration"))
                {
                    _section = Section.EyeCalibration;
                    CalibrationManager.Instance.EyeCalibration();
                }
            }
            else if (_calibrationManager.GetEyeTrackerCalibrationState() && !_calibrationManager.GetEyeTrackerValidationState())
            {
                if (GUI.Button(new Rect(xForButtons, yForButtons, buttonWidth, buttonHeight), "Eye Validation"))
                {
                    _section = Section.EyeValidation;
                    CalibrationManager.Instance.EyeValidation();
                }
            }
            else if (_calibrationManager.GetEyeTrackerValidationState() && !_calibrationManager.GetSeatCalibrationState())
            {
                if (GUI.Button(new Rect(xForButtons, yForButtons, buttonWidth, buttonHeight),
                    "Seat Calibration"))
                {
                    _section = Section.SeatCalibration;
                    CalibrationManager.Instance.SeatCalibration();
                }
            }
            else if (_calibrationManager.GetSeatCalibrationState() && !_calibrationManager.GetTestDriveState())
            {
                if (GUI.Button(new Rect(xForButtons, yForButtons, buttonWidth, buttonHeight),
                    "Test Drive Scene"))
                {
                    _section = Section.TrainingBlock;
                    CalibrationManager.Instance.StartTestDrive();
                }
            }
            else if (_calibrationManager.GetTestDriveState())
            {
                if (GUI.Button(new Rect(xForButtons, yForButtons, buttonWidth, buttonHeight), "Main Experiment"))
                {
                    _section = Section.MainExperiment; 
                    // TODO check with calibration manager if it is allowed to go to the experiment (not mvp)
                    SceneLoader.Instance.AsyncLoad(4);
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
                SceneLoader.Instance.AsyncLoad(4);
            }
        }
    }

    public void ReStartMainMenu()
    {
        _section = Section.MainMenu;
    }
}
