using System.Collections;
using System.Collections.Generic;
using RoboRyanTron.Unite2017.Variables;
using UnityEngine;

public class EventReset : MonoBehaviour
{

    public bool resetValue;

    public FloatVariable maxTrials;
    public FloatVariable trialsDone;

    public GameObject carBody;
    public GameObject resetPosition;
    // Start is called before the first frame update
    void Start()
    {
        if (resetValue)
        {
            trialsDone.SetValue(0);
        }
    }

    private void ResetCar()
    {
        carBody.transform.position = resetPosition.transform.position;
        carBody.transform.rotation = resetPosition.transform.rotation;
        carBody.GetComponent<Rigidbody>().velocity = new Vector3(0,0,0);
    }

    public void ObjectHit()
    {
        TrialFailed();
    }
    
    private void TrialFailed()
    {
        if (trialsDone.Value < maxTrials.Value)
        {
            trialsDone.ApplyChange(1);
            ResetCar();
        }
        else
        {
            endTestTrial(true, trialsDone);
        }
    }

    private void endTestTrial(bool didSucceed, FloatVariable amountOfTrials)
    {
        amountOfTrials = trialsDone;
    }
}
