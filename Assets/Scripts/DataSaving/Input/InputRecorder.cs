using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InputRecorder: MonoBehaviour
{

    private float _sampleRate;
    private GameObject _participantCar;
    private bool _recordingEnded;

    private bool receivedInput;
    private float _steeringInput;
    private float _accelerationInput;
    private float _brakeInput;


    private List<InputDataFrame> InputDataFrames;

    private void Awake()
    {
        
    }

    void Start()
    {
        
        
        if (_participantCar != null)
        {
            if (_participantCar.GetComponent<ManualController>()!=null)
            {
                Debug.Log("found");
                _participantCar.GetComponent<ManualController>().NotifyInputObservers += ReceiveInput;
            }

        }
        
        _sampleRate = SavingManager.Instance.GetSampleRate();
        InputDataFrames = new List<InputDataFrame>();
    }
    
    private void ReceiveInput(float steeringInput, float accelerationInput, float brakeInput)
    {
        _steeringInput = steeringInput;
        _accelerationInput = accelerationInput;
        _brakeInput = brakeInput;
    }

    private IEnumerator RecordInputData()
    {
        while (!_recordingEnded)
        {
            InputDataFrame inputDataFrame = new InputDataFrame();
        
            inputDataFrame.TimeStamp = TimeManager.Instance.GetCurrentUnixTimeStamp();
        
            if (Math.Abs(_steeringInput) > 0 || Math.Abs(_accelerationInput) > 0 || Math.Abs(_brakeInput) > 0)
            {
                inputDataFrame.ReceivedInput = true;
                inputDataFrame.SteeringInput = _steeringInput;
                inputDataFrame.AcellerationInput = _accelerationInput;
                inputDataFrame.BrakeInput = _brakeInput;
            }
            else
            {
                inputDataFrame.ReceivedInput = false;
                inputDataFrame.SteeringInput = 0f;
                inputDataFrame.AcellerationInput = 0f;
                inputDataFrame.BrakeInput = 0f;
            }
            InputDataFrames.Add(inputDataFrame);
            yield return new WaitForSeconds(_sampleRate);
        }
    }
    
    public void StartInputRecording()
    {
        Debug.Log("<color=green>Recording Input started!</color>");
        _recordingEnded = false;
        StartCoroutine(RecordInputData());
    }

    public void StopRecording()
    {
        _recordingEnded = true;
    }

    public void SetParticipantCar(GameObject participantCar)
    {
        _participantCar = participantCar;
    }
    
    public List<InputDataFrame> GetDataFrames()
    {
        if (_recordingEnded)
        {
            return InputDataFrames;
        }
        else
        {
            throw new Exception("Input Data Recording has not been finished");
        }
    }
}
