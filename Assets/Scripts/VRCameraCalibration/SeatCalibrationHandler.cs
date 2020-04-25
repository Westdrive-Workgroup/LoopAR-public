using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[DisallowMultipleComponent]
public class SeatCalibrationHandler : MonoBehaviour
{
    private bool _successful;
    // Start is called before the first frame update
    void Start()
    {
        
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
        GUI.color = Color.black;
        GUI.skin.label.fontSize = labelFontSize;
        GUI.skin.label.fontStyle = FontStyle.Bold;
        
        GUI.Label(new Rect(xForLable, yForLable, 500, 100),  "Seat Calibration");
        
        
        GUI.backgroundColor = Color.red;
        GUI.color = Color.white;
        
        if (GUI.Button(new Rect(xForButtons*9, yForButtons - heightDifference, buttonWidth, buttonHeight), "Start again"))
        {
            // todo
            _successful = false;
        }
        
                
        // Buttons
        GUI.backgroundColor = Color.cyan;
        GUI.color = Color.white;

        if (GUI.Button(new Rect(xForButtons, yForButtons - heightDifference, buttonWidth, buttonHeight), "Delete Calibration"))
        {
            // todo
        }
        
        if (GUI.Button(new Rect(xForButtons, yForButtons, buttonWidth, buttonHeight), "Test Positioning"))
        {
            // todo
        }
        
        if (GUI.Button(new Rect(xForButtons, yForButtons + heightDifference, buttonWidth, buttonHeight),
            "Calibrate and Store"))
        {
            // todo
        }
        
        if (GUI.Button(new Rect(xForButtons, yForButtons + (heightDifference*2), buttonWidth, buttonHeight),
            "Apply Calibration"))
        {
            // todo
            _successful = true;
        }

        if (_successful)
        {
            GUI.backgroundColor = Color.green;
            
            if (GUI.Button(new Rect(xForButtons, yForButtons + (heightDifference*3), buttonWidth, buttonHeight), "Confirm Calibration"))
            {
                CalibrationManager.Instance.SeatCalibrationSuccessful();
            }
        }
    }
}
