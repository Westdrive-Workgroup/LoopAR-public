using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using PathCreation;
using UnityEngine;

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
            
            var calibrationData = JsonUtility.ToJson(_calibrationData);
            Debug.Log("saving " + input.Count + "Data frames of " + calibrationData);
        
        
            using (FileStream stream = File.Open(GetPathForSaveFile(DataName), FileMode.Create))
            {
                File.WriteAllLines(GetPathForSaveFile("Input"), input);
            
                File.WriteAllLines(GetPathForSaveFile("EyeTracking"), eyeTracking);
                
                File.WriteAllText(GetPathForSaveFile("ParticipantCalibrationData"), calibrationData);
            
            }
        }
        
        Debug.Log("saved to " + Application.persistentDataPath);
    }

    public void CreateGUIDFolder(string id)
    {
        string folderName = Application.persistentDataPath;
        _GUIDFolderPath = Path.GetFullPath(Path.Combine(folderName, id));
        Directory.CreateDirectory(_GUIDFolderPath);
        print(Application.persistentDataPath);
        print(_GUIDFolderPath);
    }
    
    private string GetPathForSaveFile(string saveFileName)
    {
        return Path.Combine(_GUIDFolderPath, saveFileName + ".txt");
    }
}
