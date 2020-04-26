using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[DisallowMultipleComponent]
public class SeatCalibrationManager : MonoBehaviour
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
        
        float xForLable = (width / 2f);
        float yForLable = height/1.35f;

        float buttonWidth = 200f;
        float buttonHeight = 30f;
        float heightDifference = 40f;
        
        int labelFontSize = 33;

        
        // Lable
        GUI.color = Color.black;
        GUI.skin.label.fontSize = labelFontSize;
        GUI.skin.label.fontStyle = FontStyle.Bold;
        
        GUI.Label(new Rect(xForLable, yForLable, 500, 100),  "Seat Calibration");


        GUI.backgroundColor = Color.yellow;
        GUI.color = Color.white;
        
        if (GUI.Button(new Rect(xForButtons*9, yForButtons - heightDifference, buttonWidth, buttonHeight), "Reset"))
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
        
        // Reset Button
        GUI.backgroundColor = Color.red;
        GUI.color = Color.white;
        
        if (GUI.Button(new Rect(xForButtons, yForButtons + (heightDifference*8), buttonWidth, buttonHeight), "Abort Experiment"))
        {
            CalibrationManager.Instance.AbortExperiment();
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
    
    
    private void DeleteSeatCalibration()
    {
            
    }

    private void TestPositioning()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            GetComponent<VRCam>().Seat();
            
        }
    }

    private void CalibrateAndStore()
    {
        if (Input.GetKeyDown(KeyCode.D))
        {
            distanceVector.x = _cameraArm.transform.position.x - _camera.transform.position.x;
            distanceVector.y = _cameraArm.transform.position.y - _camera.transform.position.y;
            distanceVector.z = _cameraArm.transform.position.z - _camera.transform.position.z;
            
            Debug.Log(distanceVector);
            

        }
    }
    
    
}
