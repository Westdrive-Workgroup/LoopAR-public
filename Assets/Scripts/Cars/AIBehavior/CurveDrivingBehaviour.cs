using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurveDrivingBehaviour : MonoBehaviour
{
    private CarController _carController;
    private AimedSpeed _aimedSpeed;

    private void Start()
    {
        _carController = this.gameObject.GetComponent<CarController>();
        _aimedSpeed = this.gameObject.GetComponent<AimedSpeed>();
    }

    public void AdjustSpeedAtCurve(float angle)
    {
        if (angle > 5 && angle < 10)
        {
            if (_carController.GetCurrentSpeedInKmH() >= 70)
            {
                _aimedSpeed.InitiateCurvePhase(50);
            }
        } 
        else if (angle > 10 && angle < 20)
        {
            if (_carController.GetCurrentSpeedInKmH() >= 50)
            {
                _aimedSpeed.InitiateCurvePhase(40);
            }
        }
        else if (angle > 20 && angle < 35)
        {
            if (_carController.GetCurrentSpeedInKmH() >= 30)
            {
                _aimedSpeed.InitiateCurvePhase(30);
            }
        }
        else if (angle > 35)
        {
            if (_carController.GetCurrentSpeedInKmH() >= 20)
            {
                _aimedSpeed.InitiateCurvePhase(20);
            }
        }
    }
}