using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

[DisallowMultipleComponent]
public class AIController : MonoBehaviour
{
    [Space] [Header("Debug")] public bool showLocalTargetGizmos = false;
    [Range(0f,100f)]
    [SerializeField] private float localTargetVisualizerRadius  = 5f;
    [SerializeField] private Color localTargetColor = Color.red;
    
    private CarController _carController;
    [SerializeField] private float steeringSensitivity = 0.01f;
    
    [SerializeField] private float accelerationCareFactor = 0.75f; //AIs in Racing games might constant push the gas pedal, I dont think that this is correct in ordinary traffic 

    [SerializeField] private float brakeFactor = 1f; //Strong Brakes requires potentially a less aggressive braking behavior of the AI.
    private Vector3 _target;
    private Vector3 _nextTarget;
    private Rigidbody _carRigidBody;
    private float _targetAngle;
    private float _aimedSpeed;
    
    
    [Space] [Header("Path Settings")] 
    [SerializeField] private BezierSplines path;
    [Range(0f,0.1f)] [SerializeField] private float precision = 0.01f;
    [Range(0.5f,20f)] [SerializeField] private float trackerSensitivity = 10f;
    [Range(0f,1f)] [SerializeField] private float progressPercentage;
    [SerializeField] private bool reverse;
    
    private Vector3 _localTarget;
    private Vector3 _nearestPoint = Vector3.zero;

    [Space] [Header("Car Mode")] 
    public bool manualOverride;
    
    
    private void OnDrawGizmosSelected()
    {
        if (showLocalTargetGizmos)
        {
            Gizmos.color = localTargetColor;
            Gizmos.DrawWireSphere(_localTarget,localTargetVisualizerRadius);
        }
    }

    private void Start()
    {
        _carRigidBody = this.gameObject.GetComponent<Rigidbody>();
        _carController = this.GetComponent<CarController>();
        
        _targetAngle = 0;

        if (this.gameObject.GetComponent<ControlSwitch>() != null)
        {
            manualOverride = this.gameObject.GetComponent<ControlSwitch>().GetManualDrivingState();
        }
        else
        {
            manualOverride = false;
        }
        
        SetLocalTarget();
    }

    public void SetLocalTarget()
    {
        _nearestPoint = GetClosestPoint(path);
        SetProgressPercentage(path);
        _localTarget = path.GetPoint(progressPercentage);
    }

    private void Update()
    {
        _aimedSpeed = this.gameObject.GetComponent<AimedSpeed>().GetAimedSpeed();

        if (Vector3.Distance(transform.position, _localTarget) < trackerSensitivity)
        {
            if (reverse)
            {
                ReversePathFollowing();
            }
            else
            {
                NormalPathFollowing();
            }
        }
        
        
        Vector3 localTargetTransform =  transform.InverseTransformPoint(path.GetPoint(progressPercentage));
        _targetAngle = (localTargetTransform.x / localTargetTransform.magnitude);
        

        float speedFactor = _carController.GetCurrentSpeed() / _carController.GetMaximumSpeed();
        float corner = Mathf.Clamp(Mathf.Abs(_targetAngle), 0, 90);
        float cornerFactor = corner / 90.0f;

        float brake = 0;
        if (corner > 10 && speedFactor > 0.05f)
        {
            brake = Mathf.Lerp(0, 1 + speedFactor * brakeFactor, cornerFactor);
        }

        float accel = 1f;
        if (corner > 20 && speedFactor > 1f)
        {
            accel = Mathf.Lerp(0, 1 * accelerationCareFactor, 1 - cornerFactor);
        }

        if (!manualOverride)
        {
            if (_carController.GetCurrentSpeed() >= _aimedSpeed)
            {
                brake += 50f;
                _carController.MoveVehicle(accel, brake, _targetAngle);
            }
            else
            {
                _carController.MoveVehicle(accel, brake, _targetAngle);
            }
        }
    }

    private void ReversePathFollowing()
    {
        if (progressPercentage < 0f)
        {
            progressPercentage = 1f;
        }
        else
        {
            progressPercentage -= precision;
        }
        _localTarget = path.GetPoint(progressPercentage);
    }

    private void NormalPathFollowing()
    {
        if (progressPercentage >= 1f)
        {
            progressPercentage = 0f;
        }
        else
        {
            progressPercentage += precision;
        }
        _localTarget = path.GetPoint(progressPercentage);
    }

    public void SetManualOverride(bool manualState)
    {
         manualOverride = manualState;
    }
    public void SetAimedSpeed(float newSpeed)
    {
        _aimedSpeed = newSpeed;
    }
    
    private Vector3 GetClosestPoint(BezierSplines path)
    {
        for (float i = 0f; i < 1f; i += precision)
        {
            Vector3 point = path.GetPoint(i);
           
            if (Vector3.Distance(this.transform.position, point) <
                Vector3.Distance(this.transform.position, _nearestPoint))
            {
                _nearestPoint = point;
            }
        }
        return _nearestPoint;
    }
    
    private float SetProgressPercentage(BezierSplines path)
    {
        for (float i = 0f; i < 1f; i += precision)
        {
            Vector3 point = path.GetPoint(i);
            
            if (point == _nearestPoint)
            {
                progressPercentage = i;
            }
        }

        if (reverse)
        {
            progressPercentage -= progressPercentage * 0.02f;
        }
        else
        {
            progressPercentage += progressPercentage * 0.02f;
        }
        
        return progressPercentage;
    }

    public bool GetIsReversed()
    {
        return reverse;
    }
}