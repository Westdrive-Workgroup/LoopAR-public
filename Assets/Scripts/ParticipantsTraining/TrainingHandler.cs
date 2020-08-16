using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TrainingHandler : MonoBehaviour
{
    public static TrainingHandler Instance { get; private set; }

    public TestEventManager testEventManager;

    [SerializeField] private GameObject seatPosition;
    private enum State
    {
        TrainingMenu,
        Training
    }

    private State _state;
    private void Start()
    {
        _state = State.TrainingMenu;
    }
    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }
    
    public GameObject GetSeatPosition()
    {
        return seatPosition;
    }

    public void OnGUI()
    {
        float height = Screen.height;
        float width = Screen.width;
        
        float xForButtons = width / 12f;
        float yForButtons = height / 7f;
        
        float xForLable = (width / 12f);
        float yForLable = height/1.35f;

        float buttonWidth = 200f;
        float buttonHeight = 30f;
        
        int labelFontSize = 33;

        
        // Lable
        GUI.color = Color.white;
        GUI.skin.label.fontSize = labelFontSize;
        GUI.skin.label.fontStyle = FontStyle.Bold;
        
        // Buttons
        GUI.backgroundColor = Color.cyan;
        GUI.color = Color.white;
        
        if (_state == State.TrainingMenu)
        {
            GUI.Label(new Rect(xForLable, yForLable, 500, 100),  "Training Block");

            if (GUI.Button(new Rect(xForButtons, yForButtons, buttonWidth, buttonHeight), "Start"))
            {
                _state = State.Training;
                testEventManager.StartTestDrive();
            }
            
            // Reset Button
            GUI.backgroundColor = Color.red;
            GUI.color = Color.white;
        
            if (GUI.Button(new Rect(xForButtons*9, yForButtons, buttonWidth, buttonHeight), "Abort"))
            {
                CalibrationManager.Instance.AbortExperiment();
            }
        } 
        else if (_state == State.Training)
        {
            GUI.backgroundColor = Color.red;
            GUI.color = Color.white;
            
            if (GUI.Button(new Rect(xForButtons*9, yForButtons, buttonWidth, buttonHeight), "End"))
            {
                SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().name);
                _state = State.TrainingMenu;
            }
        }
    }
}
