using System;
using System.Collections;
using System.Collections.Generic;
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
    private bool _validated;
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
    }

    private void Start()
    {
        _section = Section.MainMenu;
    }

    public void OnGUI()
    {
        float height = Screen.height;
        float width = Screen.width;
        
        float xForButtons = width / 12f;
        float yForButtons = height / 7f;
        
        float xForLable = (Screen.width / 2f);
        float yForLable = (height/2f) + (height / 3f);

        float buttonWidth = 200f;
        float buttonHeight = 30f;
        float heightDifference = 40f;
        
        int labelFontSize = 33;

        
        // Lable
        GUI.color = Color.white;
        GUI.skin.label.fontSize = labelFontSize;
        GUI.skin.label.fontStyle = FontStyle.Bold;
        
        GUI.Label(new Rect(xForLable, yForLable, 500, 100),  "Welcome to Westdrive LoopAR");
        
        // Reset Button
        GUI.backgroundColor = Color.red;
        GUI.color = Color.white;
        
        if (GUI.Button(new Rect(xForButtons, yForButtons + (heightDifference*8), buttonWidth, buttonHeight), "Reset"))
        {
            _section = Section.MainMenu;
        }
            
        
        // Buttons
        GUI.backgroundColor = Color.cyan;
        GUI.color = Color.white;

        
        if (vRScene)
        {
            if (_section == Section.MainMenu)
            {
                if (GUI.Button(new Rect(xForButtons, yForButtons, buttonWidth, buttonHeight), "Eye Calibration"))
                {
                    _section = Section.EyeCalibration;
                    CalibrationManager.Instance.EyeCalibration();
                }
            }
            else if (_eyeCalibrated && !_validated)
            {
                if (GUI.Button(new Rect(xForButtons, yForButtons, buttonWidth, buttonHeight), "Eye Validation"))
                {
                    _section = Section.EyeValidation;
                    CalibrationManager.Instance.EyeValidation();
                }
            }
            else if (_validated && !_seatCalibrated)
            {
                if (GUI.Button(new Rect(xForButtons, yForButtons + heightDifference, buttonWidth, buttonHeight),
                    "Seat Calibration"))
                {
                    _section = Section.SeatCalibration;
                    CalibrationManager.Instance.SeatCalibration();
                }
            }
            else if (_seatCalibrated && !_trained)
            {
                if (GUI.Button(new Rect(xForButtons, yForButtons + heightDifference, buttonWidth, buttonHeight),
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
            if (GUI.Button(new Rect(xForButtons, yForButtons, buttonWidth, buttonHeight), "Main Experiment"))
            {
                _section = Section.MainExperiment;    
                CalibrationManager.Instance.GoToTheExperiment();          
            }
        }
    }

    public void ReStartMainMenu()
    {
        _section = Section.MainMenu;
    }

    public void EyeCalibrated()
    {
        _eyeCalibrated = true;
    }

    public void EyeValidated()
    {
        _validated = true;
    }

    public void SeatCalibrated()
    {
        _seatCalibrated = true;
    }

    public void TrainingDone()
    {
        _trained = true;
    }
}
