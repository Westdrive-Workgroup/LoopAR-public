using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SavingManager : MonoBehaviour
{
    private InputRecorder _inputRecorder;
    public static SavingManager Instance { get; private set; }
    public GameObject participantcar;
    public int SetSampleRate = 90;
    private float _sampleRate;
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
    
    
    
}
