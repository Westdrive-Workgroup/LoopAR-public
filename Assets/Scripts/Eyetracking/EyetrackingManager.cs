﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EyetrackingManager : MonoBehaviour
{
    public static EyetrackingManager Instance { get; private set; }

    public int SetSampleRate = 90;
    private Transform _hmdTransform;
    
    private EyetrackingDataRecorder _eyeTrackingRecorder;
    private float _sampleRate;
    private void Awake()
    {
        _sampleRate = 1f / SetSampleRate; 
        //singleton pattern a la Unity
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);         //the Traffic Manager should be persitent by changing the scenes maybe change it on the the fly
        }
        else
        {
            Destroy(gameObject);
        }
        
        _hmdTransform = Camera.main.transform;
        //  I do not like this: we still needs tags to find that out.
    }
    // Start is called before the first frame update
    void Start()
    {
        
        _eyeTrackingRecorder = GetComponent<EyetrackingDataRecorder>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            Debug.Log("start recording");
            _eyeTrackingRecorder.StartRecording();
        }

        if (Input.GetKeyDown(KeyCode.L))
        {
            _eyeTrackingRecorder.StopRecording();
        }
    }


    public Transform GetHmdTransform()
    {
        return _hmdTransform;
    }

    public float GetSampleRate()
    {
        return _sampleRate;
    }
}
