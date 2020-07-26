using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using UnityEngine;
using UnityEngine.SceneManagement;

[DisallowMultipleComponent]
public class MainMenu : MonoBehaviour
{
    #region Fields

    public static MainMenu Instance { get; private set; }
    
    private enum Section
    {
        ChoosingState,
        MainMenu,
        IDGeneration,
        EyeCalibration,
        EyeValidation,
        SeatCalibration,
        TrainingBlock,
        MainExperiment
    }

    private Section _section;

    #endregion

    #region PrivateMethods

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
        
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void  OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (CalibrationManager.Instance.GetWasMainMenuLoaded())
        {
            _section = Section.MainMenu;
        }
    }

    #endregion

    #region PublicMethods

    public void ReStartMainMenu()
    {
        _section = Section.MainMenu;
    }

    #endregion

    #region GUI

    public void OnGUI()
    {
        #region LocalVariables

        float height = Screen.height;
        float width = Screen.width;
        
        float xForButtons = width / 12f;
        float yForButtons = height / 7f;
        
        float xForLable = (width / 12f);
        float yForLable = height/1.35f;

        float buttonWidth = 200f;
        float buttonHeight = 30f;
        
        int labelFontSize = 33;

        #endregion
        
        // Quit
        GUI.backgroundColor = Color.red;
        GUI.color = Color.white;
        
        if (GUI.Button(new Rect(xForButtons*9, yForButtons, buttonWidth, buttonHeight), "Quit"))
        {
            Application.Quit();
        }
        
        // Label
        GUI.color = Color.white;
        GUI.skin.label.fontSize = labelFontSize;
        GUI.skin.label.fontStyle = FontStyle.Bold;
        
        GUI.Label(new Rect(xForLable, yForLable, 1000, 100),  "Main Menu                    Westdrive LoopAR");
        
        
        // Choose mode
        GUI.backgroundColor = Color.magenta;
        GUI.color = Color.white;

        if (_section == Section.ChoosingState)
        {
            if (GUI.Button(new Rect(xForButtons*9, yForButtons*4, buttonWidth, buttonHeight), "VR Mode"))
            {
                CalibrationManager.Instance.StoreVRState(true);
                CalibrationManager.Instance.SetCameraMode(true);
                _section = Section.MainMenu;
            }
        
            if (GUI.Button(new Rect(xForButtons*5, yForButtons*4, buttonWidth, buttonHeight), "Non-VR Mode"))
            {
                CalibrationManager.Instance.StoreVRState(false);
                CalibrationManager.Instance.SetCameraMode(false);
                _section = Section.MainMenu;
            }
        }

        if (_section != Section.ChoosingState)
        {
            // Reset Button
            GUI.backgroundColor = Color.yellow;
            GUI.color = Color.white;
        
            if (GUI.Button(new Rect(xForButtons*5, yForButtons, buttonWidth, buttonHeight), "Reset"))
            {
                _section = Section.MainMenu;
            }
        }


        if (CalibrationManager.Instance.GetVRActivationState() && CalibrationManager.Instance.GetWasMainMenuLoaded())
        {
            // Buttons
            GUI.backgroundColor = Color.cyan;
            GUI.color = Color.white;
            
            if (!CalibrationManager.Instance.GetParticipantUUIDState())
            {
                if (GUI.Button(new Rect(xForButtons, yForButtons, buttonWidth, buttonHeight), "Generate Participant ID"))
                {
                    _section = Section.IDGeneration;
                    CalibrationManager.Instance.GenerateID();
                }
            }
            /*else if (CalibrationManager.Instance.GetParticipantUUIDState() && !CalibrationManager.Instance.GetEyeTrackerCalibrationState())
            {
                if (GUI.Button(new Rect(xForButtons, yForButtons, buttonWidth, buttonHeight), "Eye Calibration"))
                {
                    _section = Section.EyeCalibration;
                    CalibrationManager.Instance.EyeCalibration();
                }
            }
            else if (CalibrationManager.Instance.GetEyeTrackerCalibrationState() && !CalibrationManager.Instance.GetEyeTrackerValidationState())
            {
                if (GUI.Button(new Rect(xForButtons, yForButtons, buttonWidth, buttonHeight), "Eye Validation"))
                {
                    _section = Section.EyeValidation;
                    CalibrationManager.Instance.EyeValidation();
                }
            }*/
            else if (/*CalibrationManager.Instance.GetEyeTrackerValidationState()*/ CalibrationManager.Instance.GetParticipantUUIDState() 
                                                                                    && !CalibrationManager.Instance.GetSeatCalibrationState())
            {
                if (GUI.Button(new Rect(xForButtons, yForButtons, buttonWidth, buttonHeight),
                    "Seat Calibration"))
                {
                    _section = Section.SeatCalibration;
                    CalibrationManager.Instance.SeatCalibration();
                }
            } 
            /*else if (CalibrationManager.Instance.GetSeatCalibrationState() && !CalibrationManager.Instance.GetTestDriveState())
            {
                if (GUI.Button(new Rect(xForButtons, yForButtons, buttonWidth, buttonHeight), "Training Block"))
                {
                    _section = Section.TrainingBlock; 
                    SceneManager.LoadSceneAsync("TestDrive2.0");
                }
            }*/
            else if (CalibrationManager.Instance.GetSeatCalibrationState() /*CalibrationManager.Instance.GetTestDriveState()*/)
            {
                if (GUI.Button(new Rect(xForButtons, yForButtons, buttonWidth, buttonHeight), "Start Experiment"))
                {
                    _section = Section.MainExperiment; 
                    // TODO check with calibration manager if it is allowed to go to the experiment (not mvp)
                    SceneLoadingHandler.Instance.SceneChange("safe-mountainroad01");
                }
            }
        }
        else if (!CalibrationManager.Instance.GetVRActivationState() && CalibrationManager.Instance.GetWasMainMenuLoaded())
        {
            GUI.backgroundColor = Color.cyan;
            GUI.color = Color.white;
            
            if (GUI.Button(new Rect(xForButtons, yForButtons, buttonWidth, buttonHeight), "Start Experiment"))
            {
                _section = Section.MainExperiment;
                SceneLoadingHandler.Instance.SceneChange("safe-mountainroad01");
            }
            
            /*if (!CalibrationManager.Instance.GetTestDriveState())
            {
                if (GUI.Button(new Rect(xForButtons, yForButtons, buttonWidth, buttonHeight), "Training Block"))
                {
                    _section = Section.TrainingBlock; 
                    SceneManager.LoadSceneAsync("TestDrive2.0");
                }
            }
            else if (CalibrationManager.Instance.GetTestDriveState())
            {
                if (GUI.Button(new Rect(xForButtons, yForButtons, buttonWidth, buttonHeight), "Start Experiment"))
                {
                    _section = Section.MainExperiment;
                    SceneManager.LoadSceneAsync("safe-mountainroad01");
                }
            }*/
        }
    }

    #endregion
}
