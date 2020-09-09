﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using Random = System.Random;

[DisallowMultipleComponent]
public class MainMenu : MonoBehaviour
{
    #region Fields

    public static MainMenu Instance { get; private set; }

    [SerializeField] private GameObject welcome;
    [SerializeField] private GameObject loading;
    [SerializeField] private GameObject thankYou;
    [SerializeField] private Canvas canvas;

    private bool _eyeCalibrationSelected;
    private bool _eyeValidationSelected;

    private enum Section
    {
        ChooseVRState,
        ChooseSteeringInput,
        IDGeneration,
        EyeCalibration,
        EyeValidation,
        SeatCalibration,
        TrainingBlock
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
        _section = Section.ChooseVRState;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (CalibrationManager.Instance.GetWasMainMenuLoaded())
        {
            _section = (Section) Enum.Parse(typeof(Section), ApplicationManager.Instance.GetLastMainMenuState(), true);

            _eyeCalibrationSelected = _eyeValidationSelected = true;
            
            if (welcome != null)
            {
                Destroy(welcome);
            }
        }
    }

    #endregion

    #region PublicMethods

    public void ReStartMainMenu()
    {
        _section = Section.ChooseVRState;
    }

    public Canvas GetCanvas()
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
        
        float xB = width / 12f;
        float yB = height / 7f;

        float w = 200f;
        float h = 30f;
        
        int labelFontSize = 33;

        #endregion

        // Quit
        GUI.backgroundColor = Color.red;
        GUI.color = Color.white;
        
        if (GUI.Button(new Rect(xB*9, yB, w, h), "Quit"))
        {
            Application.Quit();
        }
        
        if (_eyeCalibrationSelected && !CalibrationManager.Instance.GetEndOfExperimentState())
        {
            Destroy(welcome);

            if (loading != null) loading.gameObject.SetActive(true);
            if (thankYou != null) thankYou.gameObject.SetActive(false);
        }
        else if (CalibrationManager.Instance.GetEndOfExperimentState())
        {
            if (welcome != null) Destroy(welcome);
            if (loading != null) Destroy(loading);
            
            thankYou.gameObject.SetActive(true);
        }
        
        #region Table

        if (CalibrationManager.Instance.GetCameraModeSelectionState())
        {
            GUI.color = Color.green;
            GUI.skin.box.fontStyle = FontStyle.Bold;
            GUI.Box(new Rect(xB*9, yB*4.7f, w, h-8), new GUIContent("Camera Mode"));
        }
        else
        {
            GUI.color = Color.yellow;
            GUI.skin.box.fontStyle = FontStyle.Italic;
            GUI.Box(new Rect(xB*9, yB*4.7f, w, h-8), new GUIContent("Camera Mode"));
        }

        if (CalibrationManager.Instance.GetSteeringInputSelectedState())
        {
            GUI.color = Color.green;
            GUI.skin.box.fontStyle = FontStyle.Bold;
            GUI.Box(new Rect(xB*9, yB*5f, w, h-8), new GUIContent("Control Input"));
        }
        else
        {
            GUI.color = Color.yellow;
            GUI.skin.box.fontStyle = FontStyle.Italic;
            GUI.Box(new Rect(xB*9, yB*5f, w, h-8), new GUIContent("Control Input"));
        }

        if (CalibrationManager.Instance.GetParticipantUUIDState())
        {
            GUI.color = Color.green;
            GUI.skin.box.fontStyle = FontStyle.Bold;
            GUI.Box(new Rect(xB*9, yB*5.3f, w, h-8), new GUIContent("Participant ID"));
        }
        else
        {
            GUI.color = Color.yellow;
            GUI.skin.box.fontStyle = FontStyle.Italic;
            GUI.Box(new Rect(xB*9, yB*5.3f, w, h-8), new GUIContent("Participant ID"));
        }

        if (CalibrationManager.Instance.GetVRActivationState())
        {
            if (!_eyeCalibrationSelected)
            {
                GUI.color = Color.yellow;
                GUI.skin.box.fontStyle = FontStyle.Italic;
                GUI.Box(new Rect(xB * 9, yB * 5.6f, w, h - 8), new GUIContent("Eye-tracker Calibration"));
            }
            else if (CalibrationManager.Instance.GetEyeTrackerCalibrationState())
            {
                GUI.color = Color.green;
                GUI.skin.box.fontStyle = FontStyle.Bold;
                GUI.Box(new Rect(xB * 9, yB * 5.6f, w, h - 8), new GUIContent("Eye-tracker Calibration"));
            }
            else
            {
                GUI.color = Color.red;
                GUI.skin.box.fontStyle = FontStyle.BoldAndItalic;
                GUI.Box(new Rect(xB * 9, yB * 5.6f, w, h - 8), new GUIContent("Eye-tracker Calibration"));
            }

            if (!_eyeValidationSelected)
            {
                GUI.color = Color.yellow;
                GUI.skin.box.fontStyle = FontStyle.Italic;
                GUI.Box(new Rect(xB * 9, yB * 5.9f, w, h - 8), new GUIContent("Eye-tracker Validation"));
            }
            else if (CalibrationManager.Instance.GetEyeTrackerValidationState())
            {
                GUI.color = Color.green;
                GUI.skin.box.fontStyle = FontStyle.Bold;
                GUI.Box(new Rect(xB * 9, yB * 5.9f, w, h - 8), new GUIContent("Eye-tracker Validation"));
            }
            else
            {
                GUI.color = Color.red;
                GUI.skin.box.fontStyle = FontStyle.BoldAndItalic;
                GUI.Box(new Rect(xB * 9, yB * 5.9f, w, h - 8), new GUIContent("Eye-tracker Validation"));
            }

            if (CalibrationManager.Instance.GetSeatCalibrationState())
            {
                GUI.color = Color.green;
                GUI.skin.box.fontStyle = FontStyle.Bold;
                GUI.Box(new Rect(xB * 9, yB * 6.2f, w, h - 8), new GUIContent("Seat Calibration"));
            }
            else
            {
                GUI.color = Color.yellow;
                GUI.skin.box.fontStyle = FontStyle.Italic;
                GUI.Box(new Rect(xB * 9, yB * 6.2f, w, h - 8), new GUIContent("Seat Calibration"));
            }
        }

        #endregion
        
        #region States

        GUI.color = Color.white;

        // Choosing VR or non-VR mode
        if (_section == Section.ChooseVRState)
        {
            GUI.backgroundColor = Color.green;

            if (GUI.Button(new Rect(xB, yB, w, h), "VR Mode"))
            {
                CalibrationManager.Instance.StoreVRState(true);
                CalibrationManager.Instance.SetCameraMode(true);
                _section = Section.ChooseSteeringInput;
            }
            
            GUI.backgroundColor = Color.cyan;
        
            if (GUI.Button(new Rect(xB, yB*2, w, h), "Non-VR Mode"))
            {
                CalibrationManager.Instance.StoreVRState(false);
                CalibrationManager.Instance.SetCameraMode(false);
                _section = Section.ChooseSteeringInput;
            }
        }
        
        // Choosing control input
        if (_section == Section.ChooseSteeringInput)
        {
            GUI.backgroundColor = Color.green;
            
            if (GUI.Button(new Rect(xB, yB, w, h), "Steering Wheel"))
            {
                CalibrationManager.Instance.StoreSteeringInputDevice("SteeringWheel");
                _section = Section.IDGeneration;
            }
        
            GUI.backgroundColor = Color.cyan;
            
            if (GUI.Button(new Rect(xB, yB*1.5f, w, h), "Xbox One Controller"))
            {
                CalibrationManager.Instance.StoreSteeringInputDevice("XboxOneController");
                _section = Section.IDGeneration;
            }
            
            GUI.backgroundColor = Color.blue;
            
            if (GUI.Button(new Rect(xB, yB*2f, w, h), "Keyboard"))
            {
                CalibrationManager.Instance.StoreSteeringInputDevice("Keyboard");
                _section = Section.IDGeneration;
            }
        }
        
        // Participant ID
        if (_section == Section.IDGeneration)
        {
            GUI.backgroundColor = Color.green;

            if (GUI.Button(new Rect(xB, yB, w, h), "Generate Participant ID"))
            {
                _section = Section.IDGeneration;
                CalibrationManager.Instance.GenerateID();
                _section = CalibrationManager.Instance.GetVRActivationState() ? Section.EyeCalibration : Section.TrainingBlock;
            }
        }
        
        // Eye calibration
        if (_section == Section.EyeCalibration)
        {
            GUI.backgroundColor = Color.green;
            if (GUI.Button(new Rect(xB, yB, w, h), "Eye Calibration"))
            {
                _eyeCalibrationSelected = true;
                CalibrationManager.Instance.EyeCalibration();

                if (CalibrationManager.Instance.GetEyeTrackerCalibrationState())
                {
                    _section = Section.EyeValidation;
                }
            }
                
            GUI.backgroundColor = Color.yellow;
            if (GUI.Button(new Rect(xB, yB*2, w, h), "Skip Eye Calibration"))
            {
                _eyeCalibrationSelected = true;
                _section = Section.EyeValidation;
            }
        }
        
        // Eye validation
        if (_section == Section.EyeValidation)
        {
            ApplicationManager.Instance.StoreMainMenuLastState("SeatCalibration");
            
            GUI.backgroundColor = Color.green;
            if (GUI.Button(new Rect(xB, yB, w, h), "Eye Validation"))
            {
                _eyeValidationSelected = true;
                CalibrationManager.Instance.EyeValidation();
            }
                
            GUI.backgroundColor = Color.yellow;
            if (GUI.Button(new Rect(xB, yB*2, w, h), "Skip Eye Validation"))
            {
                _eyeValidationSelected = true;
                _section = Section.SeatCalibration;
            }
        }
        
        // Seat calibration
        if (_section == Section.SeatCalibration)
        {
            ApplicationManager.Instance.StoreMainMenuLastState("TrainingBlock");

            GUI.backgroundColor = Color.green;
            if (GUI.Button(new Rect(xB, yB, w, h), "Seat Calibration"))
            {
                CalibrationManager.Instance.SeatCalibration();
            }
                
            GUI.backgroundColor = Color.yellow;
            if (GUI.Button(new Rect(xB, yB*2, w, h), "Skip Seat Calibration"))
            {
                _section = Section.TrainingBlock;
            }
        }
        
        // Training scene
        if (_section == Section.TrainingBlock)
        {
            GUI.backgroundColor = Color.green;
            if (GUI.Button(new Rect(xB, yB, w, h), "Training Block"))
            {
                CalibrationManager.Instance.StartTestDrive();
            }
                
            GUI.backgroundColor = Color.yellow;
            if (GUI.Button(new Rect(xB, yB*2, w, h), "Skip Training Block"))
            {
                if (welcome != null) 
                    Destroy(welcome);
                
                if (loading != null)
                    Destroy(loading);
                    
                SceneLoadingHandler.Instance.LoadExperimentScenes();
            }
        }
        
        #endregion
    }

    #endregion
}
