using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class MainMenu : MonoBehaviour
{
    [Space] [Header("Scene Type")]
    [SerializeField] private bool vRScene;
    private enum Section
    {
        MainMenu,
        EyeCalibration,
        EyeValidation,
        SeatCalibration,
        TrainingBlock,
        MainExperiment
    }

    private Section _section;

    private int _test;
    
    private void Start()
    {
        _test = 0;
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
        
                
        // Buttons
        GUI.backgroundColor = Color.cyan;
        GUI.color = Color.white;

        if (_section == Section.MainMenu)
        {
            GUI.Label(new Rect(xForLable*10, yForLable, 500, 100),  "Welcome to Westdrive LoopAR");
            
            if (GUI.Button(new Rect(xForButtons, yForButtons - heightDifference, buttonWidth, buttonHeight), "Start again"))
            {
                _section = Section.MainMenu;
            }
            
            if (vRScene)
            {
                if (GUI.Button(new Rect(xForButtons, yForButtons - heightDifference, buttonWidth, buttonHeight), "Eye Calibration"))
                {
                    // EyetrackingManager.Instance.StartCalibration();
                    _section = Section.EyeCalibration;
                } 
                else if (_section == Section.EyeCalibration /* && EyetrackingManager.Instance.StartCalibration()*/)
                {
                    if (GUI.Button(new Rect(xForButtons, yForButtons, buttonWidth, buttonHeight), "Eye Validation"))
                    {
                        // EyetrackingManager.Instance.StartValidation();
                        _section = Section.EyeValidation;
                    }
                }
                else if (_section == Section.EyeValidation /*&& dummy eyevalidation bool*/)
                {
                    if (GUI.Button(new Rect(xForButtons, yForButtons + heightDifference, buttonWidth, buttonHeight),
                        "Seat Calibration"))
                    {
                        SceneLoader.Instance.AsyncLoad(2);
                        _section = Section.SeatCalibration;
                    }
                }
                else if (_section == Section.SeatCalibration /*&& dummy seatcalibration bool*/)
                {
                    if (GUI.Button(new Rect(xForButtons, yForButtons + heightDifference, buttonWidth, buttonHeight),
                        "Test Drive Scene"))
                    {
                        SceneLoader.Instance.AsyncLoad(3);
                        _section = Section.TrainingBlock;
                    }
                }
                else if (_section == Section.TrainingBlock /*&& drive training true*/)
                {
                    if (GUI.Button(new Rect(xForButtons, yForButtons, buttonWidth, buttonHeight), "Main Experiment"))
                    {
                        SceneLoader.Instance.AsyncLoad(4);
                        _section = Section.MainExperiment;           
                    }
                }
            }
            else
            {
                if (GUI.Button(new Rect(xForButtons, yForButtons, buttonWidth, buttonHeight), "Main Experiment"))
                {
                    SceneLoader.Instance.AsyncLoad(4);
                    _section = Section.MainExperiment;           
                }
            }
        }
    }
}
