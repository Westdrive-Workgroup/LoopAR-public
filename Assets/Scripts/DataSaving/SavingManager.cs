using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
    private List<InputDataFrame> _inputData;
    private SceneData _sceneData;
    private CalibrationData _participantCalibrationData;
    private InputRecorder _inputRecorder;
    private SceneDataRecorder _sceneDataRecorder;

    private bool _readyToSaveToFile;

    private List<string[]> rawData;
    private List<float> _frameRates;

    private GameObject participantCar;
    private string _targetSceneName;
    
    private void Awake()
    {
        _inputRecorder = GetComponent<InputRecorder>();
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
    }

    void Start()
    {
        _frameRates = new List<float>();
        _readyToSaveToFile=false;
        _sceneDataRecorder = SceneDataRecorder.Instance;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.H) && Input.GetKey(KeyCode.LeftShift))
        {
            StopAndSaveData("Incomplete");
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
        Debug.Log("<color=red>Stopped recording Data...</color>");
        
        yield return SavingData();
        Debug.Log("<color=blue>Saving Data...</color>");

        _eyeTrackingData.Clear();
        _inputData.Clear();
        _sceneData.EventBehavior.Clear();
            
        StartRecordingData();
    }

    public void StopAndSaveData(string targetScene=null)
    {
        _targetSceneName = targetScene;
        StartCoroutine(StopingAndSavingData());
    }
    
    IEnumerator StopingAndSavingData()
    {
        StopRecord();
        Debug.Log("<color=red>Stopped recording Data...</color>");
        
        yield return SavingData();
        Debug.Log("<color=blue>Saving Data...</color>");
        
        _eyeTrackingData.Clear();
        _inputData.Clear();
        _sceneData = null;
    }
    
    private void RecordData()
    {
        _readyToSaveToFile = false;
        Debug.Log("<color=green>Recording Data...</color>");
        _inputRecorder.SetParticipantCar(participantCar);
        _inputRecorder.StartInputRecording();
        EyetrackingManager.Instance.StartRecording();
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
        StoreSceneData(_sceneDataRecorder.GetDataFrame());
        StoreCalibrationData();
    }

    private bool TestCompleteness()
    {
        if (_inputData != null && _eyeTrackingData != null && _participantCalibrationData != null && _sceneData != null)
            return true;
        return false;
    }

    public void StoreEyeTrackingData(List<EyeTrackingDataFrame> eyeTrackingdataFrames)
    {
        _eyeTrackingData = eyeTrackingdataFrames;
    }

    public void StoreInputData(List<InputDataFrame> inputDataFrames)
    {
        _inputData = inputDataFrames;
    }

    public void StoreSceneData(SceneData sceneData)
    {
        _sceneData = sceneData;
        _frameRates.Add(_sceneData.AverageSceneFPS);
    }
    
    public void StoreCalibrationData()
    {
        _participantCalibrationData = CalibrationManager.Instance.GetCalibrationData();
        _participantCalibrationData.AverageExperimentFPS = _frameRates.Average();
        _participantCalibrationData.ApplicationDuration = TimeManager.Instance.GetApplicationDuration();
        _participantCalibrationData.ExperimentDuration = TimeManager.Instance.GetExperimentDuration();
        _participantCalibrationData.TrainingSuccessState = CalibrationManager.Instance.GetTestDriveState();
        _participantCalibrationData.NumberOfTrainingTrials = CalibrationManager.Instance.GetTestDriveNumberOfTrials();
    }

    public void SetParticipantCar(GameObject car)
    {
        participantCar = car;
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
            
            var sceneData = JsonUtility.ToJson(_sceneData);
            Debug.Log("saving " + input.Count + "Data frames of " + _sceneData);
            
            var participantCalibrationData = JsonUtility.ToJson(_participantCalibrationData);
            Debug.Log("saving " + input.Count + "Data frames of " + participantCalibrationData);

            var id = _participantCalibrationData.ParticipantUuid;

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
                File.WriteAllText(GetPathForSaveFile("SceneData", id, _targetSceneName), sceneData);
            }
            
            using (FileStream stream = File.Open(GetPathForSaveParticipantCalibrationData(DataName, DataName), FileMode.Create))
            {
                File.WriteAllText(GetPathForSaveParticipantCalibrationData("ParticipantCalibrationData", id), participantCalibrationData);
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
            
            var sceneData = JsonUtility.ToJson(_sceneData);
            Debug.Log("saving " + input.Count + "Data frames of " + _sceneData);
            
            var participantCalibrationData = JsonUtility.ToJson(_participantCalibrationData);
            Debug.Log("saving " + input.Count + "Data frames of " + participantCalibrationData);

            var id = _participantCalibrationData.ParticipantUuid;

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
                File.WriteAllText(GetPathForSaveFile("SceneData", id, _targetSceneName), sceneData);
            }
            
            using (FileStream stream = File.Open(GetPathForSaveParticipantCalibrationData(DataName, DataName), FileMode.Create))
            {
                File.WriteAllText(GetPathForSaveParticipantCalibrationData("ParticipantCalibrationData", id), participantCalibrationData);
            }
        }
        
        Debug.Log("saved to " + Application.persistentDataPath);

        yield return null;
    }

    private string GetPathForSaveFile(string folderFileName, string id, string sceneName)
    {
        return Path.Combine(Path.GetFullPath(Path.Combine(Application.persistentDataPath, folderFileName)), id + "_" + folderFileName + "_" + sceneName + ".txt");
    }
    
    private string GetPathForSaveParticipantCalibrationData(string folderFileName, string id)
    {
        return Path.Combine(Path.GetFullPath(Path.Combine(Application.persistentDataPath, folderFileName)), id + "_" + folderFileName + ".txt");
    }

    public void SaveDataAndStartRecordingAgain(string oldSceneName)
    {
        _targetSceneName = oldSceneName;
        StartCoroutine(StartRecordingAfterSavingData());
    }

    public float GetCurrentFPS()
    {
        return this.gameObject.GetComponent<FPSDisplay>().GetCurrentFPS();
    }
}
