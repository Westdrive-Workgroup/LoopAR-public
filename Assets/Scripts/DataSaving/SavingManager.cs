using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SavingManager : MonoBehaviour
{
    public string DataPath;
    public string DataName;
    public static SavingManager Instance { get; private set; }
    public int SetSampleRate = 90;
    private float _sampleRate;

    private List<EyeTrackingDataFrame> _eyeTrackingData;
    private EyetrackingValidation _eyetrackingValidation;
    private List<InputDataFrame> _inputData;
    private InputRecorder _inputRecorder;

    private bool _readyToSaveToFile;

    private List<string[]> rawData;

    private GameObject participantCar;
    
    // Start is called before the first frame update
    private void Awake()
    {
        _inputRecorder = GetComponent<InputRecorder>();
        _inputRecorder.SetParticipantCar(participantCar);
        _sampleRate = 1f / SetSampleRate;
        
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);         //the Eyetracking Manager should be persitent by changing the scenes maybe change it on the the fly
        }
        else
        {
            Destroy(gameObject);
        }

        DataPath = GetPathForSaveFile(DataName);
        
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
        }
    }
    // Update is called once per frame
    
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
        Debug.Log("record Data...");
        _inputRecorder.StartInputRecording();
        EyetrackingManager.Instance.StartRecording();
    }

    private void StopRecord()
    {
        Debug.Log("stop recording Data");
        _inputRecorder.StopRecording();
        EyetrackingManager.Instance.StopRecording();
        RetrieveData();
    }

    private void RetrieveData()
    {
        StoreEyeTrackingData(EyetrackingManager.Instance.GetEyeTrackingData());
        StoreInputData(_inputRecorder.GetDataFrames());
        
    }

    private bool TestCompleteness()
    {
        if (_inputData != null && _eyeTrackingData != null)
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

   

    public void StoreEyeValidationData(EyetrackingValidation eyeValidation)
    {
        _eyetrackingValidation = eyeValidation;
    }

    public void StoreInputData(List<InputDataFrame> inputDataFrames)
    {
        _inputData = inputDataFrames;
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
        
        
            using (FileStream stream = File.Open(DataPath, FileMode.Create))
            {
                File.WriteAllLines(GetPathForSaveFile("input"), input);
            
                File.WriteAllLines(GetPathForSaveFile("eyeTracking"), eyeTracking);
            
            }
        }
       
        
        
        

        Debug.Log("saved to " + Application.persistentDataPath);
    }
    private string GetPathForSaveFile(string saveFileName)
    {
        return Path.Combine(Application.persistentDataPath, saveFileName + ".txt");
    }
    
    
    
    
    
}
