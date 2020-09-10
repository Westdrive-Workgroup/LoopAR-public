using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Security;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = System.Random;

[DisallowMultipleComponent]
public class CalibrationManager : MonoBehaviour
{
    #region Fields

    public static CalibrationManager Instance { get; private set; }
    
    private bool _wasMainMenuLoaded;
    private bool _uUIDGenerated;
    private bool _eyeTrackerCalibrationSuccessful;
    private bool _eyeTrackerValidationSuccessful;
    private bool _seatCalibrationSuccessful;
    private bool _testDriveSuccessful;
    private bool _endOfExperiment;

    private bool _cameraModeSelected;
    private bool _steeringInputGiven;
    
    
    private CalibrationData _calibrationData;
    private String _calibrationFilePath;
    private Random _random;

    private int _numberOfTrainingTrials;
    private string _experimentalCondition;
    
    #endregion

    #region PrivateMethods

    private void Awake()
    {
        _calibrationFilePath = GetPathForSaveFile("CalibrationData");

        if (File.Exists(_calibrationFilePath))
        {
            _calibrationData = LoadCalibrationFile(_calibrationFilePath);
        }
        else
        {
            _calibrationData = new CalibrationData();
        }

        if (!File.Exists(GetPathForSaveFolder("Input")))
        {
            Directory.CreateDirectory(Path.GetFullPath(Path.Combine(Application.persistentDataPath, "Input")));
        }
        
        if (!File.Exists(GetPathForSaveFolder("EyeTracking")))
        {
            Directory.CreateDirectory(Path.GetFullPath(Path.Combine(Application.persistentDataPath, "EyeTracking")));
        }
        
        if (!File.Exists(GetPathForSaveFolder("ParticipantCalibrationData")))
        {
            Directory.CreateDirectory(Path.GetFullPath(Path.Combine(Application.persistentDataPath, "ParticipantCalibrationData")));
        }
        
        if (!File.Exists(GetPathForSaveFolder("SceneData")))
        {
            Directory.CreateDirectory(Path.GetFullPath(Path.Combine(Application.persistentDataPath, "SceneData")));
        }

        _random = new Random();
        
        //singleton pattern a la Unity
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    private void StoreParticipantUuid(string iD)
    {
        _calibrationData.ParticipantUuid = iD.Replace("-", "");
        SaveCalibrationData();
    }

    private void DeleteCalibrationFile(string dataPath)
    {
        if(!File.Exists(dataPath))
        {
            Debug.Log("File not found, can not be deleted!");
        }
        else
        {
            File.Delete(dataPath);
        }
    }

    private void SaveCalibrationFile(CalibrationData calibrationData)
    {
        //todo use using
        string jsonString = JsonUtility.ToJson(calibrationData);
        File.WriteAllText(_calibrationFilePath, jsonString);
    }
    
    private string GetPathForSaveFile(string saveFileName)
    {
        return Path.Combine(Application.persistentDataPath, saveFileName + ".txt");
    }
    
    private string GetPathForSaveFolder(string folderName)
    {
        return Path.Combine(Application.persistentDataPath, folderName);
    }
    
    
    private CalibrationData LoadCalibrationFile(string dataPath)
    {
        string jsonString;
        if(!File.Exists(dataPath))
        {
            Debug.Log("File not found!");
            return null;
        }
        else
        {
            Debug.Log("<color=green>Found Calibration Data, loading...</color>");
            jsonString = File.ReadAllText(dataPath);
            //Debug.Log(jsonString);
            return JsonUtility.FromJson<CalibrationData>(jsonString);
        }
    }

    #endregion

    #region PublicMethods

    public void GenerateIDAndCondition()
    { 
        string newParticipantId = System.Guid.NewGuid().ToString();
        StoreParticipantUuid(newParticipantId);
        _uUIDGenerated = true;
        GenerateCondition();
    }

    public void GenerateCondition()
    {
        // todo bring the randomization back
        // int conditionNumber = _random.Next(1, 5);
        
        // todo remove the line below
        int conditionNumber = 1;
        
        
        Debug.Log("condition num: " + conditionNumber);
        
        switch (conditionNumber)
        {
            case 1:
                _experimentalCondition = "FullLoopAR";
                Debug.Log("FullLoopAR");
                break;
            case 2:
                _experimentalCondition = "HUDOnly";
                Debug.Log("HUDOnly");
                break;
            case 3:
                _experimentalCondition = "AudioOnly";
                Debug.Log("AudioOnly");
                break;
            case 4:
                _experimentalCondition = "BaseCondition";
                Debug.Log("BaseCondition");
                break;
        }
        
        ApplicationManager.Instance.SetExperimentalCondition(_experimentalCondition);
        _calibrationData.ExperimentalCondition = _experimentalCondition;
        SaveCalibrationData();
    }

    public void StoreSteeringInputDevice(string steeringDevice)
    {
        _calibrationData.SteeringInputDevice = steeringDevice;
        _steeringInputGiven = true;
        SaveCalibrationData();
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
        SceneManager.LoadSceneAsync("EyetrackingValidation");
    }

    public void EyeValidationSuccessful()
    {
        _eyeTrackerValidationSuccessful = true;
        SceneManager.LoadSceneAsync("MainMenu");
    }

    public void SeatCalibration()
    {
        SceneLoadingHandler.Instance.SceneChange("SeatCalibrationScene");
    }

    public void SeatCalibrationSuccessful()
    {
        _seatCalibrationSuccessful = true;
        SceneManager.LoadSceneAsync("MainMenu");
    }

    public void StartTestDrive()
    {
        SceneLoadingHandler.Instance.SceneChange("TrainingScene");
    }
    
    public void TestDriveSuccessState(bool state, int trials)
    {
        _testDriveSuccessful = state;
        _numberOfTrainingTrials = trials;
    }

    public void TestDriveEnded()
    {
        SceneLoadingHandler.Instance.LoadExperimentScenes();
    }
    
    public void AbortExperiment()
    {
        TimeManager.Instance.SetExperimentEndTime();
        SceneManager.LoadSceneAsync("MainMenu");
        MainMenu.Instance.ReStartMainMenu();
    }
    
    public void StoreSeatCalibrationData(Vector3 seatOffset)
    {
        _calibrationData.SeatCalibrationOffset = seatOffset;
        // StoreVRState(true);
        SaveCalibrationData();
    }
    
    public void StoreValidationErrorData(Vector3 validationError)
    {
        _calibrationData.EyeValidationError = validationError;
        SaveCalibrationData();
    }

    public void StoreVRState(bool vRMode)
    {
        _calibrationData.VRmode = vRMode;
        _wasMainMenuLoaded = true;
        _cameraModeSelected = true;
        SaveCalibrationData();
    }

    public void SaveCalibrationData()
    {
        SaveCalibrationFile(_calibrationData);
    }
    
    public void DeleteCalibrationData()
    {
        DeleteCalibrationFile(_calibrationFilePath);
    }

    public void ExperimentEnded()
    {
        _endOfExperiment = true;
    }

    #endregion
    
    #region Setters

    public void SetCameraMode(bool vrModeState)
    {
        if (vrModeState)
        {
            CameraManager.Instance.VRModeCameraSetUp();
        }
        else
        {
            CameraManager.Instance.NonVRModeCameraSetUp();
        }
    }

    #endregion
    
    #region Getters

    public CalibrationData GetCalibrationData()
    {
        return _calibrationData;
    }
    
    public bool GetWasMainMenuLoaded()
    {
        return _wasMainMenuLoaded;
    }

    public bool GetCameraModeSelectionState()
    {
        return _cameraModeSelected;
    }
    
    public bool GetSteeringInputSelectedState()
    {
        return _steeringInputGiven;
    }

    public bool GetParticipantUUIDState()
    {
        return _uUIDGenerated;
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

    public int GetTestDriveNumberOfTrials()
    {
        return _numberOfTrainingTrials;
    }

    public Vector3 GetSeatCalibrationOffsetPosition()
    {
        return _calibrationData.SeatCalibrationOffset;
    }

    public string GetExperimentalCondition()
    {
        return _calibrationData.ExperimentalCondition;
    }

    public Vector3 GetValidationError()
    {
        return _calibrationData.EyeValidationError;
    }

    private bool GetVRModeState()
    {
        return _calibrationData.VRmode;
    }
        
    public bool GetVRActivationState()
    {
        return GetVRModeState();
    }


    public string GetSteeringInputDevice()
    {
        return _calibrationData.SteeringInputDevice;
    }


    public bool GetEndOfExperimentState()
    {
        return _endOfExperiment;
    }
    
    #endregion
}
