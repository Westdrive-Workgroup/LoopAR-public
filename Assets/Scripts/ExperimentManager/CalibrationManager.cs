using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Security;
using UnityEngine;

[DisallowMultipleComponent]
public class CalibrationManager : MonoBehaviour
{
    public static CalibrationManager Instance { get; private set; }
    private int _state;

    private String calibrationFilePath;
    
    private bool _eyeTrackerCalibrationSuccessful;
    private bool _eyeTrackerValidationSuccessful;
    private bool _seatCalibrationSuccessful;
    private bool _testDriveSuccessful;


    private CalibrationData _calibrationData;
    private Vector3 _eyeValidationError;
    private Vector3 _seatCalibrationOffset;
    
    private void Awake()
    {
        
        calibrationFilePath = GetPathForSaveFile("CalibrationData");

        if (!File.Exists(calibrationFilePath))
        {
            _calibrationData = LoadCalibrationFile(calibrationFilePath);
            _seatCalibrationOffset = _calibrationData.SeatCalibrationOffset;
        }
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

    public void EyeCalibration()
    {
        EyetrackingManager.Instance.StartCalibration();
    }

    public void EyeCalibrationSuccessful()
    {
        _eyeTrackerCalibrationSuccessful = true;
    }
    
    public void EyeValidation()
    {
        SceneLoader.Instance.AsyncLoad(1);
    }

    public void EyeValidationSuccessful()
    {
        _eyeTrackerValidationSuccessful = true;
        SceneLoader.Instance.AsyncLoad(0);
    }

    public void SeatCalibration()
    {
        SceneLoader.Instance.AsyncLoad(2);
    }

    public void SeatCalibrationSuccessful()
    {
        _seatCalibrationSuccessful = true;
        SceneLoader.Instance.AsyncLoad(0);
    }

    public void StartTestDrive()
    {
        SceneLoader.Instance.AsyncLoad(3);
    }
    
    public void TestDriveSuccessful()
    {
        _testDriveSuccessful = true;
        SceneLoader.Instance.AsyncLoad(0);
    }

    public void GoToTheExperiment()
    {
        //TODO check if all tests wer successful
        SceneLoader.Instance.AsyncLoad(4);
    }

    public void AbortExperiment()
    {
        SceneLoader.Instance.AsyncLoad(0);
        MainMenu.Instance.ReStartMainMenu();
    }

    public bool GetEyeTrackerCalibrationState()
    {
        return _eyeTrackerCalibrationSuccessful;
    }

    public bool GetEyeTrackerValidationState()
    {
        return _eyeTrackerValidationSuccessful;
    }

    public bool GetSeatCalibrationState()
    {
        return _seatCalibrationSuccessful;
    }

    public bool GetTestDriveState()
    {
        return _testDriveSuccessful;
    }

    public Vector3 GetSeatCalibrationOffset()
    {
        return _seatCalibrationOffset;
    }
    void SaveCalibrationFile(CalibrationData calibrationData)
    {
        string jsonString = JsonUtility.ToJson(calibrationData);
        File.WriteAllText(calibrationFilePath, jsonString);
    }
    
    private string GetPathForSaveFile(string saveFileName)
    {
        return Path.Combine(Application.persistentDataPath, saveFileName + ".txt");
    }
    
    private CalibrationData LoadCalibrationFile(string dataPath)
    {
        string jsonString;
        if(!File.Exists(dataPath))
        {
            Debug.Log("file not found");
            
        }
        
        else
        {
            Debug.Log("found data");
            jsonString = File.ReadAllText(dataPath);
            //Debug.Log(jsonString);
            return JsonUtility.FromJson<CalibrationData>(jsonString);
        }
    }

    public void StoreSeatCalibrationData(Vector3 seatOffset)
    {
        _seatCalibrationOffset = seatOffset;
        _calibrationData.SeatCalibrationOffset = seatOffset;
    }
    
    
}
