using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using PathCreation;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SavingManager : MonoBehaviour
{
    public string DataPath;
    public string DataName;
    private string _GUIDFolderPath;
    public static SavingManager Instance { get; private set; }
    public int SetSampleRate = 90;
    private float _sampleRate;

    private List<EyeTrackingDataFrame> _eyeTrackingData;
    private CalibrationData _calibrationData;
    private List<InputDataFrame> _inputData;
    private InputRecorder _inputRecorder;

    private bool _readyToSaveToFile;

    private List<string[]> rawData;

    private GameObject participantCar;
    private string _targetSceneName;
    
    private void Awake()
    {
        _inputRecorder = GetComponent<InputRecorder>();
        _inputRecorder.SetParticipantCar(participantCar);
        _sampleRate = 1f / SetSampleRate;
        
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); 
        }
        else
        {
            Destroy(gameObject);
        }

        // DataPath = GetPathForSaveFile(DataName);
    }

    void Start()
    {
        _readyToSaveToFile=false;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.H))
        {
            StopRecord();
            SaveData();
            Debug.Log("<color=blue>Saving Data!</color>");
        }
    }
    
    public float GetSampleRate()
    {
        return _sampleRate;
    }
    
    public void StartRecordingData()
    {
        RecordData();
    }

    public void StopRecordingData()
    {
        StopRecord();
    }

    public void SaveData()
    {
        _readyToSaveToFile = TestCompleteness();
        
        if (_readyToSaveToFile)
        {
            SaveToJson();
        }
        else
        {
            Debug.Log("error the data collection was not completed or corrupted");
        }
    }

    IEnumerator SavingData()
    {
        _readyToSaveToFile = TestCompleteness();
        
        if (_readyToSaveToFile)
        {
            yield return SavingToJson();
        }
        else
        {
            Debug.Log("error the data collection was not completed or corrupted");
            yield return null;
        }
    }

    IEnumerator StartRecordingAfterSavingData()
    {
        StopRecord();
        
        yield return SavingData();
        
        StartRecordingData();
    }
    
    private void RecordData()
    {
        _readyToSaveToFile = false;
        Debug.Log("<color=green>Recording Data...</color>");
        _inputRecorder.StartInputRecording();
        EyetrackingManager.Instance.StartRecording();
        
        // todo add a class for timestamps and fps
    }

    private void StopRecord()
    {
        Debug.Log("<color=red>Stop recording Data!</color>");
        _inputRecorder.StopRecording();
        EyetrackingManager.Instance.StopRecording();
        RetrieveData();
    }

    private void RetrieveData()
    {
        StoreEyeTrackingData(EyetrackingManager.Instance.GetEyeTrackingData());
        StoreInputData(_inputRecorder.GetDataFrames());
        StoreCalibrationData();
    }

    private bool TestCompleteness()
    {
        // todo check if participant's UUID is available
        
        if (_inputData != null && _eyeTrackingData != null && _calibrationData != null)
        {
            return true;
        }
        else
        {
            return false;
        }
    }


    public void StoreEyeTrackingData(List<EyeTrackingDataFrame> eyeTrackingdataFrames)
    {
        _eyeTrackingData = eyeTrackingdataFrames;
    }

    public void StoreInputData(List<InputDataFrame> inputDataFrames)
    {
        _inputData = inputDataFrames;
    }

    public void StoreCalibrationData()
    {
        _calibrationData = CalibrationManager.Instance.GetCalibrationData();
    }

    public void SetParticipantCar(GameObject car)
    {
        participantCar = car;
        _inputRecorder.SetParticipantCar(car);
    }



    private List<String> ConvertToJson(List<InputDataFrame> inputData)
    {
        List<string> list = new List<string>();
        
        foreach(var frame in inputData)
        {
            string jsonString = JsonUtility.ToJson(frame, true);
            list.Add(jsonString);
        }

        return list;
    }

    private List<String> ConvertToJson(List<EyeTrackingDataFrame> inputData)
    {
        List<string> list = new List<string>();
        
        foreach(var frame in inputData)
        {
            string jsonString = JsonUtility.ToJson(frame, true);
            list.Add(jsonString);
        }

        return list;
    }
    
    public void SaveToJson()
    {
        if (_readyToSaveToFile)
        {
            var input = ConvertToJson(_inputData);
            Debug.Log("saving " + input.Count + "Data frames of " + _inputData);
        
            var eyeTracking = ConvertToJson(_eyeTrackingData);
            Debug.Log("saving " + input.Count + "Data frames of " + _eyeTrackingData);
            
            var participantCalibrationData = JsonUtility.ToJson(_calibrationData);
            Debug.Log("saving " + input.Count + "Data frames of " + participantCalibrationData);

            var id = _calibrationData.ParticipantUuid;

            using (FileStream stream = File.Open(GetPathForSaveFile(DataName, DataName, DataName), FileMode.Create))
            {
                File.WriteAllLines(GetPathForSaveFile("Input", id, SceneManager.GetActiveScene().name), input);
            }
            
            using (FileStream stream = File.Open(GetPathForSaveFile(DataName, DataName, DataName), FileMode.Create))
            {
                File.WriteAllLines(GetPathForSaveFile("EyeTracking", id, SceneManager.GetActiveScene().name), eyeTracking);
            }
            
            using (FileStream stream = File.Open(GetPathForSaveFile(DataName, DataName, DataName), FileMode.Create))
            {
                File.WriteAllText(GetPathForSaveFile("ParticipantCalibrationData", id, SceneManager.GetActiveScene().name), participantCalibrationData);
            }
        }
        
        Debug.Log("saved to " + Application.persistentDataPath);
    }
    
    IEnumerator SavingToJson()
    {
        if (_readyToSaveToFile)
        {
            var input = ConvertToJson(_inputData);
            Debug.Log("saving " + input.Count + "Data frames of " + _inputData);
        
            var eyeTracking = ConvertToJson(_eyeTrackingData);
            Debug.Log("saving " + input.Count + "Data frames of " + _eyeTrackingData);
            
            var participantCalibrationData = JsonUtility.ToJson(_calibrationData);
            Debug.Log("saving " + input.Count + "Data frames of " + participantCalibrationData);

            var id = _calibrationData.ParticipantUuid;

            using (FileStream stream = File.Open(GetPathForSaveFile(DataName, DataName, DataName), FileMode.Create))
            {
                File.WriteAllLines(GetPathForSaveFile("Input", id, _targetSceneName), input);
            }
            
            using (FileStream stream = File.Open(GetPathForSaveFile(DataName, DataName, DataName), FileMode.Create))
            {
                File.WriteAllLines(GetPathForSaveFile("EyeTracking", id, _targetSceneName), eyeTracking);
            }
            
            using (FileStream stream = File.Open(GetPathForSaveFile(DataName, DataName, DataName), FileMode.Create))
            {
                File.WriteAllText(GetPathForSaveFile("ParticipantCalibrationData", id, _targetSceneName), participantCalibrationData);
            }
        }
        
        Debug.Log("saved to " + Application.persistentDataPath);

        yield return null;
    }

    private string GetPathForSaveFile(string folderFileName, string id, string sceneName)
    {
        return Path.Combine(Path.GetFullPath(Path.Combine(Application.persistentDataPath, folderFileName)), id + "_" + folderFileName + "_" + sceneName + ".txt");
    }

    public void SaveDataAndStartRecordingAgain(string oldSceneName)
    {
        _targetSceneName = oldSceneName;
        StartCoroutine(StartRecordingAfterSavingData());
    }
}
