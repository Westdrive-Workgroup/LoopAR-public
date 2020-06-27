using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ShowInput : MonoBehaviour
{
    public GameObject ManualCar;
    private String canvasText;

    private float steering;

    private float accel;

    private float brake;
    
    // Start is called before the first frame update
    void Start()
    {
        ManualCar.GetComponent<ManualController>().NotifyInputObservers += ReceivedInput;
    }

    // Update is called once per frame
    void Update()
    {
        canvasText = string.Format("accel: {0}\n brake: {1}\n steer: {2}", accel, brake, steering);
        //canvasText = $"accel: {accel} {}".FormatIt(accel, brake, steering);
        GetComponent<TextMeshProUGUI>().text = canvasText;
    }






    private void ReceivedInput(float steeringInput, float accelerationInput, float brakeInput)
    {
        accel = (float) System.Math.Round(accelerationInput,2);
        brake = (float) System.Math.Round(brakeInput, 2);
        steering = (float) System.Math.Round(steeringInput, 2);
    }
}
