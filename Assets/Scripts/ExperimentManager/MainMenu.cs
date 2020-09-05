using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

[DisallowMultipleComponent]
public class MainMenu : MonoBehaviour
{
    #region Fields

    public static MainMenu Instance { get; private set; }

    [SerializeField] private GameObject welcome;
    [SerializeField] private GameObject loading;
    [SerializeField] private GameObject thankYou;
    [SerializeField] private GameObject canvas;
    
    private enum Section
    {
        ChooseVRState,
        ChooseSteeringInput,
        MainMenu,
        NonVRMenu,
        IDGeneration,
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
        loading.gameObject.SetActive(false);
        thankYou.gameObject.SetActive(false);
    }

    private void  OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (CalibrationManager.Instance.GetWasMainMenuLoaded())
        {
            _section = Section.MainMenu;

            if (welcome != null) 
            {
                
            }
                Destroy(welcome);
        }

    }

    #endregion

    #region PublicMethods

    public void ReStartMainMenu()
    {
        _section = Section.MainExperiment;
    }

    public GameObject GetCanvas()
    {
        return canvas;
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
        
//        GUI.Label(new Rect(xForLable, yForLable, 1000, 100),  "Main Menu                    Westdrive LoopAR");
        
        
        // Choose mode
        GUI.backgroundColor = Color.green;
        GUI.color = Color.white;

        if (_section == Section.ChooseVRState && !CalibrationManager.Instance.GetWasMainMenuLoaded())
        {
            if (GUI.Button(new Rect(xForButtons, yForButtons, buttonWidth, buttonHeight), "VR Mode"))
            {
                CalibrationManager.Instance.StoreVRState(true);
                CalibrationManager.Instance.SetCameraMode(true);
                _section = Section.ChooseSteeringInput;
            }
            
            GUI.backgroundColor = Color.cyan;
        
            if (GUI.Button(new Rect(xForButtons, yForButtons*2, buttonWidth, buttonHeight), "Non-VR Mode"))
            {
                CalibrationManager.Instance.StoreVRState(false);
                CalibrationManager.Instance.SetCameraMode(false);
                _section = Section.ChooseSteeringInput;
            }
        }

        if (_section == Section.ChooseSteeringInput)
        {
            GUI.backgroundColor = Color.cyan;
            
            if (GUI.Button(new Rect(xForButtons, yForButtons, buttonWidth, buttonHeight), "Steering Wheel"))
            {
                CalibrationManager.Instance.StoreSteeringInputDevice("SteeringWheel");
                _section = CalibrationManager.Instance.GetVRActivationState() ? Section.MainMenu : Section.NonVRMenu;
            }
        
            GUI.backgroundColor = Color.green;
            
            if (GUI.Button(new Rect(xForButtons, yForButtons*1.5f, buttonWidth, buttonHeight), "Xbox One Controller"))
            {
                CalibrationManager.Instance.StoreSteeringInputDevice("XboxOneController");
                _section = CalibrationManager.Instance.GetVRActivationState() ? Section.MainMenu : Section.NonVRMenu;
            }
            
            GUI.backgroundColor = Color.blue;
            
            if (GUI.Button(new Rect(xForButtons, yForButtons*2f, buttonWidth, buttonHeight), "Keyboard"))
            {
                CalibrationManager.Instance.StoreSteeringInputDevice("Keyboard");
                _section = CalibrationManager.Instance.GetVRActivationState() ? Section.MainMenu : Section.NonVRMenu;
            }
        }
        
        if (CalibrationManager.Instance.GetEyeTrackerValidationState() && !CalibrationManager.Instance.GetEndOfExperimentState())
        {
            Destroy(welcome);

            if (loading != null)
                loading.gameObject.SetActive(true);
        }
        
        

        if (CalibrationManager.Instance.GetVRActivationState() && CalibrationManager.Instance.GetSteeringInputSelectedState() && CalibrationManager.Instance.GetWasMainMenuLoaded())
        {
            if (CalibrationManager.Instance.GetEndOfExperimentState())
            {
                thankYou.gameObject.SetActive(true);
                
                if (welcome != null) 
                    Destroy(welcome);
                
                if (loading != null)
                    Destroy(loading);
            }

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
            else if (CalibrationManager.Instance.GetParticipantUUIDState() && !CalibrationManager.Instance.GetEyeTrackerCalibrationState() && _section == Section.IDGeneration)
            {
                if (GUI.Button(new Rect(xForButtons, yForButtons, buttonWidth, buttonHeight), "Eye Calibration"))
                {
                    CalibrationManager.Instance.EyeCalibration();
                }
                
                GUI.backgroundColor = Color.yellow;
                if (GUI.Button(new Rect(xForButtons, yForButtons*2, buttonWidth, buttonHeight), "Skip Eye Calibration"))
                {
                    _section = Section.EyeValidation;
                }
            }
            else if ((CalibrationManager.Instance.GetEyeTrackerCalibrationState() && !CalibrationManager.Instance.GetEyeTrackerValidationState()) || _section == Section.EyeValidation)
            {
                if (GUI.Button(new Rect(xForButtons, yForButtons, buttonWidth, buttonHeight), "Eye Validation"))
                {
                    _section = Section.EyeValidation;
                    CalibrationManager.Instance.EyeValidation();
                }
                
                GUI.backgroundColor = Color.yellow;
                if (GUI.Button(new Rect(xForButtons, yForButtons*2, buttonWidth, buttonHeight), "Skip Eye Validation"))
                {
                    _section = Section.SeatCalibration;
                }
            }
            else if ((CalibrationManager.Instance.GetEyeTrackerValidationState() && !CalibrationManager.Instance.GetSeatCalibrationState() && _section != Section.TrainingBlock
                      && !CalibrationManager.Instance.GetEndOfExperimentState()) || _section == Section.SeatCalibration)
            {
                if (GUI.Button(new Rect(xForButtons, yForButtons, buttonWidth, buttonHeight), "Seat Calibration"))
                {
                    _section = Section.SeatCalibration;
                    CalibrationManager.Instance.SeatCalibration();
                }
                
                GUI.backgroundColor = Color.yellow;
                if (GUI.Button(new Rect(xForButtons, yForButtons*2, buttonWidth, buttonHeight), "Skip Seat Calibration"))
                {
                    _section = Section.TrainingBlock;
                }
            } 
            else if ((CalibrationManager.Instance.GetSeatCalibrationState() && !CalibrationManager.Instance.GetTestDriveState() 
                                                                            && !CalibrationManager.Instance.GetEndOfExperimentState()) || _section == Section.TrainingBlock)
            {
                if (GUI.Button(new Rect(xForButtons, yForButtons, buttonWidth, buttonHeight), "Training Block"))
                {
                    _section = Section.MainExperiment; 
                    CalibrationManager.Instance.StartTestDrive();
                }
                
                GUI.backgroundColor = Color.yellow;
                if (GUI.Button(new Rect(xForButtons, yForButtons*2, buttonWidth, buttonHeight), "Skip Training Block"))
                {
                    _section = Section.MainExperiment;
                                    
                    if (welcome != null) 
                        Destroy(welcome);
                
                    if (loading != null)
                        Destroy(loading);
                    
                    SceneLoadingHandler.Instance.LoadExperimentScenes();
                }
            }
            
            else if (_section == Section.MainExperiment)
            {
                if (loading != null)
                    Destroy(loading);
                
                thankYou.gameObject.SetActive(true);
            }
        }
        else if (!CalibrationManager.Instance.GetVRActivationState() && CalibrationManager.Instance.GetWasMainMenuLoaded())
        {
            GUI.backgroundColor = Color.cyan;
            GUI.color = Color.white;

            if (!CalibrationManager.Instance.GetParticipantUUIDState() && _section == Section.NonVRMenu)
            {
                if (GUI.Button(new Rect(xForButtons, yForButtons, buttonWidth, buttonHeight), "Generate Participant ID"))
                {
                    _section = Section.IDGeneration;
                    CalibrationManager.Instance.GenerateID();
                }
            } else if (CalibrationManager.Instance.GetParticipantUUIDState() && !CalibrationManager.Instance.GetTestDriveState() && !CalibrationManager.Instance.GetEndOfExperimentState())
            {
                if (GUI.Button(new Rect(xForButtons, yForButtons, buttonWidth, buttonHeight), "Training Block"))
                {
                    _section = Section.MainExperiment;
                    CalibrationManager.Instance.StartTestDrive();
                }
                
                GUI.backgroundColor = Color.yellow;
                if (GUI.Button(new Rect(xForButtons, yForButtons*2, buttonWidth, buttonHeight), "Skip Training Block"))
                {
                    _section = Section.MainExperiment; 
                    SceneLoadingHandler.Instance.LoadExperimentScenes();
                }
            } else if (CalibrationManager.Instance.GetEndOfExperimentState())
            {
                if (loading != null)
                    Destroy(loading);

                thankYou.gameObject.SetActive(true);
            }
        }
    }

    #endregion
}
