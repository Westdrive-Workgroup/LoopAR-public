using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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

    private int _test;

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

        GUI.Label(new Rect(xForLable, yForLable, 500, 100),  "Welcome to Westdrive LoopAR");
        
        if (GUI.Button(new Rect(xForButtons*9, yForButtons - heightDifference, buttonWidth, buttonHeight), "Start again"))
        {
            _section = Section.MainMenu;
        }
        
        if (vRScene)
        {
            if (_section == Section.MainMenu)
            {
                if (GUI.Button(new Rect(xForButtons, yForButtons - heightDifference, buttonWidth, buttonHeight), "Eye Calibration"))
                {
                    // EyetrackingManager.Instance.StartCalibration();
                    _section = Section.EyeCalibration;
                }
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
                    LoadScene(2);
                    
                    // SceneLoader.Instance.AsyncLoad(2);
                    _section = Section.SeatCalibration;
                }
            }
            else if (_section == Section.SeatCalibration /*&& dummy seatcalibration bool*/)
            {
                if (GUI.Button(new Rect(xForButtons, yForButtons + heightDifference, buttonWidth, buttonHeight),
                    "Test Drive Scene"))
                {
                    LoadScene(3);
                    
                    // SceneLoader.Instance.AsyncLoad(3);
                    _section = Section.TrainingBlock;
                }
            }
            else if (_section == Section.TrainingBlock /*&& drive training true*/)
            {
                if (GUI.Button(new Rect(xForButtons, yForButtons, buttonWidth, buttonHeight), "Main Experiment"))
                {
                    LoadScene(4);
                    
                    // SceneLoader.Instance.AsyncLoad(4);
                    _section = Section.MainExperiment;           
                }
            }
        }
        else
        {
            if (GUI.Button(new Rect(xForButtons, yForButtons, buttonWidth, buttonHeight), "Main Experiment"))
            {
                LoadScene(4);
                
                // SceneLoader.Instance.AsyncLoad(4);
                _section = Section.MainExperiment;           
            }
        }
    }

    public void ReStartMainMenu()
    {
        _section = Section.MainMenu;
    }

    private void LoadScene(int index)
    {
        SceneManager.LoadScene(index);
    }
}
