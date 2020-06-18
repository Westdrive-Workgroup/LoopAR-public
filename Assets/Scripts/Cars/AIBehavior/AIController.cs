using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using PathCreation;
using PathCreationEditor;
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
    [SerializeField] private PathCreator path;
    [Range(0,10)] [SerializeField] private int precision = 1;
    [Range(0.5f,20f)] [SerializeField] private float trackerSensitivity = 10f;
    private int _progressPercentage;
    [SerializeField] private bool reverse;
    private int _pathLength;
    
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
        _pathLength = path.path.NumPoints;
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
        
        
        Vector3 localTargetTransform =  transform.InverseTransformPoint(path.path.GetPoint(_progressPercentage));
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
        if (_progressPercentage < 0)
        {
            _progressPercentage = _pathLength;
        }
        else
        {
            _progressPercentage -= precision;
        }
        _localTarget = path.path.GetPoint(_progressPercentage);
    }

    private void NormalPathFollowing()
    {
        if (_progressPercentage >= _pathLength)
        {
            _progressPercentage = 0;
        }
        else
        {
            _progressPercentage += precision;
        }
        _localTarget = path.path.GetPoint(_progressPercentage);
    }

    
    
    private Vector3 GetClosestPoint(PathCreator path)
    {
        for (int i = 0; i < _pathLength; i += precision)
        {
            Vector3 point = path.path.GetPoint(i);
           
            if (Vector3.Distance(this.transform.position, point) <
                Vector3.Distance(this.transform.position, _nearestPoint))
            {
                _nearestPoint = point;
            }
        }
        return _nearestPoint;
    }
    
    private int SetProgressPercentage(PathCreator path)
    {
        for (int i = 0; i < _pathLength; i += precision)
        {
            Vector3 point = path.path.GetPoint(i);
            
            if (point == _nearestPoint)
            {
                _progressPercentage = i;
            }
        }

        if (reverse)
        {
            _progressPercentage -= (int)(_progressPercentage * 0.02f);
        }
        else
        {
            _progressPercentage += (int)(_progressPercentage * 0.02f);
        }
        
        return _progressPercentage;
    }

    
    #region Public methods and accessors
    public void SetLocalTarget()
    {
        _nearestPoint = GetClosestPoint(path);
        SetProgressPercentage(path);
        _localTarget = path.path.GetPoint(_progressPercentage);
    }
    
    public void SetManualOverride(bool manualState)
    {
        manualOverride = manualState;
    }
    
    public void SetAimedSpeed(float newSpeed)
    {
        _aimedSpeed = newSpeed;
    }
    
    public bool GetIsReversed()
    {
        return reverse;
    }
    #endregion
}