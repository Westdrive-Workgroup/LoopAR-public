using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour
{
    [SerializeField] private WheelCollider[] frontWheels;
    [SerializeField] private WheelCollider[] rearWheels;
    [SerializeField] private float torque = 200f;
    [SerializeField] private float maxSteerAngle = 30f;
    [SerializeField] private float maxBrakeTorque = 500f;
    [SerializeField] private bool allWheelDrive= false;
    [SerializeField] private bool rearBreakOnly = true;
    [SerializeField] private float maximumSpeedInKmH = 120f;
    [SerializeField] private GameObject seatPosition;
    
    private Rigidbody _rigidbody;
    
    private float _maximumSpeed;//meter per seconds
    private float _currentSpeed;
    [SerializeField] private bool _engineOn = true;
    [SerializeField] private Vector3 centerOfMassOffset = new Vector3(0, -0.5f, 0);

    private void Awake()
    {
        _maximumSpeed = maximumSpeedInKmH / 3.6f;
        if(GetComponent<AxisStabilizer>()!=null)
            GetComponent<AxisStabilizer>().AssignWheels(frontWheels[0],frontWheels[1],rearWheels[0],rearWheels[1]);
    }

    // Start is called before the first frame update
    void Start()
    {
        _rigidbody = this.gameObject.GetComponent<Rigidbody>();
        _rigidbody.centerOfMass += centerOfMassOffset;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // Debug.Log("Speed Km/h: " + _currentSpeed * 3.6);
      _currentSpeed = _rigidbody.velocity.magnitude;
    }

    public void MoveVehicle(float accelerationInput, float brakeInput, float steeringInput)
    {
        if (!_engineOn) return;
        
        foreach (var wheel in frontWheels)
        {
            TransferInputToWheels(wheel, accelerationInput,brakeInput, steeringInput);
        }

        foreach (var wheel in rearWheels)
        {
            TransferInputToWheels(wheel, accelerationInput, brakeInput);
        }


        //Debug.Log(_rigidbody.velocity.magnitude);
    }
    
    void TransferInputToWheels(WheelCollider wheelCol, float acceleration, float brake)
    {
        brake = Mathf.Clamp(brake, 0, 1) * maxBrakeTorque;
        acceleration = Mathf.Clamp(acceleration, -1, 1);

        float thrustTorque = CalculateThrustTorque(acceleration);
        //wheelCol.motorTorque = acceelelartionBoost*   thrustTorque;
        wheelCol.motorTorque = Mathf.Clamp(thrustTorque,0, _maximumSpeed);
        /*Debug.Log( "Torque: " + wheelCol.motorTorque + ", rpm: " +    wheelCol.rpm);*/
        wheelCol.brakeTorque = brake;
    }
    

    void TransferInputToWheels(WheelCollider wheelCol, float acceleration, float brake, float steering)
    {
        acceleration = Mathf.Clamp(acceleration, -1, 1);
        //steering = Mathf.Clamp(steering, -1, 1) * maxSteerAngle;
        steering = steering * maxSteerAngle;
        brake = Mathf.Clamp(brake, 0, 1) * maxBrakeTorque;
//        Debug.Log("accel" + acceleration + " brake" + brake);
        if (allWheelDrive)
        {
            wheelCol.motorTorque = CalculateThrustTorque(acceleration);
        }
        
        if(!rearBreakOnly)
            wheelCol.brakeTorque = brake;
        
        wheelCol.steerAngle = steering;
    }


    private float CalculateThrustTorque(float acceleration)
    {
        float thrustTorque = 0;
        if (_currentSpeed < _maximumSpeed)
            thrustTorque = acceleration * torque;
        return thrustTorque;
    }

    public float GetCurrentSpeed()
    {
        return _currentSpeed;
    }

    public float GetCurrentSpeedInKmH()
    {
        return _currentSpeed * 3.6f;
    }

    public float GetTorque()
    {
        return torque;
    }

    public void SetTorque(float torque)
    {
        this.torque = torque;
    }

    public void SetMaximumSpeed(float speedInKmh)
    {
        _maximumSpeed = speedInKmh/3.6f;
    }

    public float GetMaximumSpeed()
    {
        return _maximumSpeed;
    }

    public void TurnOnEngine()
    {
        _engineOn = true;
    }

    public void TurnOffEngine()
    {
        _engineOn = false;
    }

    public void SetEngineState(bool state)
    {
        _engineOn = state;
    }
    
    public Rigidbody GetRigidbody()
    {
        return _rigidbody;
    }

    public GameObject GetSeatPosition()
    {
        return seatPosition;
    }

    public float GetSterring()
    {
        return frontWheels[0].steerAngle;
    }
}
