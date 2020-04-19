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
    public GameObject participantcar;
    public int SetSampleRate = 90;
    private float _sampleRate;

    private List<EyeTrackingDataFrame> _eyeTrackingData;
    private EyetrackingValidation _eyetrackingValidation;
    private List<InputDataFrame> _inputData;
    private InputRecorder _inputRecorder;

    private List<string[]> rawData;

    
    
    // Start is called before the first frame update
    private void Awake()
    {
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

        DataPath = GetPathFromSaveFile(DataName);
    }

    void Start()
    {
        _inputRecorder = GetComponent<InputRecorder>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            RecordInput();
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            StopRecordInput();
        }

        if (Input.GetKeyDown(KeyCode.L))
        {
            SaveToJson();
        }
        
    }
    
    
    
    public float GetSampleRate()
    {
        return _sampleRate;
    }

    public GameObject GetParticipantCar()
    {
        return participantcar;
    }

    private void RecordInput()
    {
        Debug.Log("record Input");
        _inputRecorder.StartInputRecording();
    }

    private void StopRecordInput()
    {
        _inputRecorder.StopRecording();
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

    
    



    private List<String> SerializeInputDataToJson(List<InputDataFrame> inputData)
    {
        List<string> list = new List<string>();
        
        foreach(var frame in inputData)
        {
            string jsonString = JsonUtility.ToJson(frame);
            list.Add(jsonString);
        }

        return list;
    }
    
    public void SaveToJson()
    {

        var file = SerializeInputDataToJson(_inputData);
        
        Debug.Log("saving... " + file.Count);
        using (FileStream stream = File.Open(DataPath, FileMode.Create))
        {
            File.WriteAllLines(GetPathFromSaveFile("save"), file);
        }
        
        
        

        Debug.Log("saved to " + Application.persistentDataPath);
    }
    private string GetPathFromSaveFile(string saveFile)
    {
        return Path.Combine(Application.persistentDataPath, saveFile + ".txt");
    }
    
    
    
    
    
}
