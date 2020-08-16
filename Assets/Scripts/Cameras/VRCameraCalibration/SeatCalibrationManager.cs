using System;
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
    
    private void  OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        vRCam = CameraManager.Instance.GetVRCamera();
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

    private void Update()
    {
        float positionOffset = 0.01f;

        if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
        {
            positionOffset = 0.5f;
        }
        
        if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
        {
            _cameraOffsetObject.transform.Translate(-positionOffset, 0, 0);
        }
        if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
        {
            _cameraOffsetObject.transform.Translate(positionOffset, 0, 0);
        }
        if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W))
        {
            _cameraOffsetObject.transform.Translate(0, positionOffset, 0);
        }
        if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S))
        {
            _cameraOffsetObject.transform.Translate(0, -positionOffset, 0);
        }
        if (Input.GetKeyDown(KeyCode.Q) || Input.GetKeyDown(KeyCode.KeypadPlus)  || Input.GetKeyDown(KeyCode.Plus))
        {
            _cameraOffsetObject.transform.Translate(0, 0, positionOffset);
        }
        if (Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.KeypadMinus) || Input.GetKeyDown(KeyCode.Minus))
        {
            _cameraOffsetObject.transform.Translate(0, 0, -positionOffset);
        }

        if (Input.GetKeyDown(KeyCode.Return))
        {
            ApplyCalibration();
        }
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
            _successful = false;
            DeleteSeatCalibration();
            ApplyCalibration();
        }
        
                
        // Buttons
        GUI.backgroundColor = Color.cyan;
        GUI.color = Color.white;

        if (GUI.Button(new Rect(xForButtons, yForButtons - heightDifference, buttonWidth, buttonHeight), "Test Positioning"))
        {
            TestPositioning();
        }
        
        /*if (GUI.Button(new Rect(xForButtons, yForButtons + heightDifference, buttonWidth, buttonHeight),
            "Calibrate and Store"))
        {
            CalibrateAndStore();
        }*/
        
        if (GUI.Button(new Rect(xForButtons, yForButtons, buttonWidth, buttonHeight),
            "Apply Calibration"))
        {
            ApplyCalibration();
        }
        
        GUI.backgroundColor = Color.green;
            
        if (GUI.Button(new Rect(xForButtons, yForButtons + (heightDifference*2), buttonWidth, buttonHeight), "Confirm Calibration"))
        {
            CalibrationManager.Instance.SeatCalibrationSuccessful();
        }
        
        
        GUI.backgroundColor = Color.red;
        GUI.color = Color.white;
        
        if (GUI.Button(new Rect(xForButtons, yForButtons + (heightDifference * 4), buttonWidth, buttonHeight), "Delete Calibration"))
        {
            DeleteSeatCalibration();
        }
        
        if (GUI.Button(new Rect(xForButtons*9, yForLable, buttonWidth, buttonHeight), "Abort Experiment"))
        {
            CalibrationManager.Instance.AbortExperiment();
        }
    }

    #endregion
    
    private void TestPositioning()
    {
        vRCam.Seat();
    }
    
    private void DeleteSeatCalibration()
    {
        CalibrationManager.Instance.StoreSeatCalibrationData(Vector3.zero);
        vRCam.UnSeat();
    }

    private void ApplyCalibration()
    {
        CalibrationManager.Instance.StoreSeatCalibrationData(_cameraOffsetObject.transform.localPosition);
    }

    public GameObject GetParticipantsCar()
    {
        return car;
    }
    
    public GameObject GetSeatPosition()
    {
        return seatPosition != null ? seatPosition : null;
    }
}
