using System;
using System.Collections;
using System.Collections.Generic;
using Tobii.XR;
using UnityEngine;
using UnityEngine.SceneManagement;
using ViveSR.anipal.Eye;

public class EyetrackingManager : MonoBehaviour
{
    public static EyetrackingManager Instance { get; private set; }

    public int SetSampleRate = 90;
    private Transform _hmdTransform;
    private List<EyeTrackingDataFrame> _eyeTrackingDataFrames;
    private EyeValidationData _eyeValidationData;
    private EyetrackingValidation _eyetrackingValidation;
    private bool _eyeValidationSucessful;
    private EyetrackingDataRecorder _eyeTrackingRecorder;
    private float _sampleRate;

    private bool _calibrationSuccess;

    private float eyeValidationDelay;

    private Vector3 _eyeValidationErrorAngles;
    
    public delegate void OnCompletedEyeValidation(bool wasSuccessful);
    public event OnCompletedEyeValidation NotifyEyeValidationCompletnessObservers;
    
    private void Awake()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        _sampleRate = 1f / SetSampleRate; 
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
        
        
        //  I do not like this: we still needs tags to find that out.
    }

    private void  OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        _hmdTransform = Camera.main.transform;
        //Debug.Log("hello new World");
    }

    private void OnEnable()
    {
        
    }

    private void OnLevelWasLoaded()
    {
        
    }

    private void Update()
    {
        
    }

    // Start is called before the first frame update
    void Start()
    {
        
        _eyeTrackingRecorder = GetComponent<EyetrackingDataRecorder>();

        _eyetrackingValidation = GetComponentInChildren<EyetrackingValidation>();

        _eyetrackingValidation.NotifyEyeValidationObservers += SetEyeValidationStatus;
    }

    // Update is called once per frame

    public void StartValidation()
    {
        Debug.Log("validating...");
        _eyetrackingValidation.StartValidation(eyeValidationDelay);
    }

    public void AbortValidation()
    {
        _eyetrackingValidation.AbortValidation();
        NotifyEyeValidationCompletnessObservers?.Invoke(false);
    }

    public void StartValidation(float delay)
    {
        _eyetrackingValidation.StartValidation(delay);
    }
    
    
    public void StartCalibration()
    {
        if (SRanipal_Eye_v2.LaunchEyeCalibration())
        {
            Debug.Log("<color=green>calibration successful :)</color>");
            CalibrationManager.Instance.EyeCalibrationSuccessful();
        }
        else
        {
            Debug.Log("<color=red>calibration failed :(</color>");
        }
    }

    public void StartRecording()
    {
        Debug.Log("<color=green>Recording eye-tracking Data!</color>");
        _eyeTrackingRecorder.StartRecording();
    }
    
    public void StopRecording()
    {
        _eyeTrackingRecorder.StopRecording();
        StoreEyeTrackingData();
        

    }
    
    
    public Transform GetHmdTransform()
    {
        return _hmdTransform;
    }

    public float GetSampleRate()
    {
        return _sampleRate;
    }

    public void StoreEyeValidationData(EyeValidationData data)
    {
        _eyeValidationData = data;
    }

    public Vector3 GetEyeValidationErrorAngles()
    {
        return _eyeValidationErrorAngles;
    }
    

    private void StoreEyeTrackingData()
    {
        _eyeTrackingDataFrames = _eyeTrackingRecorder.GetDataFrames();
    }

    public List<EyeTrackingDataFrame> GetEyeTrackingData()
    {
        if (_eyeTrackingDataFrames != null)
        {
            return _eyeTrackingDataFrames;
        }
        else
        {
            throw new Exception("There are no Eyetrackingdata");
        }
    }

    private void SetEyeValidationStatus(bool eyeValidationWasSucessfull, Vector3 errorAngles)
    {
        Debug.Log("eyeValidation Status was called in EyeTrackingManager with " + eyeValidationWasSucessfull);
        _eyeValidationSucessful = eyeValidationWasSucessfull;
        
        if (!eyeValidationWasSucessfull)
        {
            _eyeValidationErrorAngles = errorAngles;
            NotifyEyeValidationCompletnessObservers?.Invoke(false);
            
        }
        else
        {
            _eyeValidationErrorAngles = errorAngles;
            NotifyEyeValidationCompletnessObservers?.Invoke(true);
        }
        
        
    }
    

    public bool GetEyeValidationStatus()
    {
        return _eyeValidationSucessful;
    }
    
    public double getCurrentTimestamp()
    {
        System.DateTime epochStart = new System.DateTime(1970, 1, 1, 0, 0, 0, System.DateTimeKind.Utc);
        return (System.DateTime.UtcNow - epochStart).TotalSeconds;
    }

    
}
