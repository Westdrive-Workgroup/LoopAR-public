using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[DisallowMultipleComponent]
public class SeatCalibrationManager : MonoBehaviour
{
    private bool _successful;

    private Vector3 distanceVector;
    private GameObject vrCameraObject;
    private GameObject cameraOffsetObject;

    [SerializeField]private VRCam _vrCam;
    private 
    // Start is called before the first frame update
    void Start()
    {
        distanceVector = CalibrationManager.Instance.GetSeatCalibrationOffset();
        vrCameraObject = _vrCam.GetCamera();
        cameraOffsetObject = _vrCam.GetCameraOffset();
        
        if (_vrCam == null)
        {
            Debug.LogError("Please add the VRCam Prefab in the Inspector");
        }
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
            TestPositioning();
        }
        
        if (GUI.Button(new Rect(xForButtons, yForButtons + heightDifference, buttonWidth, buttonHeight),
            "Calibrate and Store"))
        {
            CalibrateAndStore();
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
        _vrCam.Seat();
    }

    private void CalibrateAndStore()
    {
       
            distanceVector.x = cameraOffsetObject.transform.position.x - vrCameraObject.transform.position.x;
            distanceVector.y = cameraOffsetObject.transform.position.y - vrCameraObject.transform.position.y;
            distanceVector.z = cameraOffsetObject.transform.position.z - vrCameraObject.transform.position.z;
            
            CalibrationManager.Instance.StoreSeatCalibrationData(distanceVector);
            
    }
    
    
}
