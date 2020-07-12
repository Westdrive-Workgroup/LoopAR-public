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
    
    [SerializeField]private VRCam vRCam;
    [SerializeField]private GameObject car;
    [SerializeField]private GameObject seatPosition;
    
    
    
    private void Awake()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    
    void Start()
    {
        distanceVector = new Vector3();
        vRCam = CameraManager.Instance.GetVRCamera();
        vrCameraObject = CameraManager.Instance.GetMainCamera().gameObject;
        cameraOffsetObject = CameraManager.Instance.GetCalibrationOffset();
        
        if (vRCam == null)
        {
            Debug.LogError("Please add the VRCam Prefab in the Inspector");
        }
    }
    
    private void  OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        vRCam = CameraManager.Instance.GetVRCamera();
        CameraManager.Instance.SetObjectToFollow(car);
        CameraManager.Instance.SetSeatPosition(seatPosition);
    }

    #region GUI

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
            DeleteSeatCalibration();
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
            ApplyCalibration();
        }
        
        GUI.backgroundColor = Color.green;
            
        if (GUI.Button(new Rect(xForButtons, yForButtons + (heightDifference*3), buttonWidth, buttonHeight), "Confirm Calibration"))
        {
            CalibrationManager.Instance.SeatCalibrationSuccessful();
        }
        
        
        // Reset Button
        GUI.backgroundColor = Color.red;
        GUI.color = Color.white;
        
        if (GUI.Button(new Rect(xForButtons*9, yForLable, buttonWidth, buttonHeight), "Abort Experiment"))
        {
            CalibrationManager.Instance.AbortExperiment();
        }
    }

    #endregion
    
    
    
    private void DeleteSeatCalibration()
    {
        CalibrationManager.Instance.StoreSeatCalibrationData(Vector3.zero);
    }

    private void TestPositioning()
    {
        vRCam.Seat();
    }

    private void CalibrateAndStore()
    {
       Debug.Log(cameraOffsetObject.transform.position);
       Debug.Log(vrCameraObject.transform.position);
       
        distanceVector.x = cameraOffsetObject.transform.position.x - vrCameraObject.transform.position.x;
        distanceVector.y = cameraOffsetObject.transform.position.y - vrCameraObject.transform.position.y;
        distanceVector.z = cameraOffsetObject.transform.position.z - vrCameraObject.transform.position.z;
        
        //distanceVector.x = vrCameraObject.transform.position.x - SeatPosition.transform.position.x;
        //distanceVector.y = vrCameraObject.transform.position.y - SeatPosition.transform.position.y;
        //distanceVector.z = vrCameraObject.transform.position.z - SeatPosition.transform.position.z;
        
        CalibrationManager.Instance.StoreSeatCalibrationData(distanceVector);
        Debug.Log(distanceVector);
    }

    private void ApplyCalibration()
    {
        // SceneLoader.Instance.AsyncLoad(2);
        Debug.Log("loading the scene again");
        SceneManager.LoadSceneAsync("SeatCalibrationScene");
    }
}
