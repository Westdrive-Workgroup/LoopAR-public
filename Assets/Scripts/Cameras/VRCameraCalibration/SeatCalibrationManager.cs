using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[DisallowMultipleComponent]
public class SeatCalibrationManager : MonoBehaviour
{
    public static SeatCalibrationManager Instance { get; private set; }
    
    private bool _successful;

    private Vector3 _distanceVector;
    private GameObject _vrCameraObject;
    private GameObject _cameraOffsetObject;
    
    [SerializeField] private VRCam vRCam;
    [SerializeField] private GameObject car;
    [SerializeField] private GameObject seatPosition;
    
    
    
    private void Awake()
    {
        //singleton pattern a la Unity
        if (Instance == null)
        {
            Instance = this;
        }

        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    
    void Start()
    {
        _distanceVector = new Vector3();
        vRCam = CameraManager.Instance.GetVRCamera();
        _vrCameraObject = CameraManager.Instance.GetMainCamera().gameObject;
        _cameraOffsetObject = CameraManager.Instance.GetCalibrationOffset();
        
        if (vRCam == null)
        {
            Debug.LogError("Please add the VRCam Prefab in the Inspector");
        }
    }
    
    private void  OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        vRCam = CameraManager.Instance.GetVRCamera();
        // CameraManager.Instance.SetObjectToFollow(car);
        // CameraManager.Instance.SetSeatPosition(seatPosition);
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
       Debug.Log(_cameraOffsetObject.transform.position);
       Debug.Log(_vrCameraObject.transform.position);
       
        _distanceVector.x = _cameraOffsetObject.transform.position.x - _vrCameraObject.transform.position.x;
        _distanceVector.y = _cameraOffsetObject.transform.position.y - _vrCameraObject.transform.position.y;
        _distanceVector.z = _cameraOffsetObject.transform.position.z - _vrCameraObject.transform.position.z;
        
        //distanceVector.x = vrCameraObject.transform.position.x - SeatPosition.transform.position.x;
        //distanceVector.y = vrCameraObject.transform.position.y - SeatPosition.transform.position.y;
        //distanceVector.z = vrCameraObject.transform.position.z - SeatPosition.transform.position.z;
        
        CalibrationManager.Instance.StoreSeatCalibrationData(_distanceVector);
        Debug.Log(_distanceVector);
    }

    private void ApplyCalibration()
    {
        // SceneLoader.Instance.AsyncLoad(2);
        Debug.Log("loading the scene again");
        SceneManager.LoadSceneAsync("SeatCalibrationScene");
    }

    public GameObject GetParticipantsCar()
    {
        return car;
    }
}
