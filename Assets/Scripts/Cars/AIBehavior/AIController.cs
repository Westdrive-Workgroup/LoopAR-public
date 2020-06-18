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
    private CarController _carController;
    
    // Gizmos
    [Space] [Header("Debug")] public bool showLocalTargetGizmos = false;
    [Range(0f,100f)]
    [SerializeField] private float localTargetVisualizerRadius  = 5f;
    [SerializeField] private Color localTargetColor = Color.magenta;
    
    // Target
    private Vector3 _target;
    private Vector3 _nextTarget;
    private float _targetAngle;
    private float _aimedSpeed;
    private Vector3 _localTarget;
    private Vector3 _nearestPoint = Vector3.zero;
    
    // Path
    [Space] [Header("Path Settings")] 
    [SerializeField] private PathCreator path;
    [SerializeField] private EndOfPathInstruction endOfPathInstruction;    // todo implement
    [Range(0,1f)] [SerializeField] private float precision = 0.01f;
    [Range(0.5f,20f)] [SerializeField] private float trackerSensitivity = 10f;
    private float _progressPercentage;

    // Driving behavior
    [Space][Header("Driving behavior")]
    [SerializeField] private bool driveInReverse;
    // [SerializeField] private float steeringSensitivity = 0.01f;    // todo use this
    [SerializeField] private float accelerationCareFactor = 0.75f; //AIs in Racing games might constant push the gas pedal, I dont think that this is correct in ordinary traffic 
    [SerializeField] private float brakeFactor = 1f; //Strong Brakes requires potentially a less aggressive braking behavior of the AI.
    private bool _manualOverride;
    
    
    #region Private methods
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
        _carController = this.GetComponent<CarController>();
        _targetAngle = 0;

        if (this.gameObject.GetComponent<ControlSwitch>() != null)
        {
            _manualOverride = this.gameObject.GetComponent<ControlSwitch>().GetManualDrivingState();
        }
        else
        {
            _manualOverride = false;
        }
        
        SetLocalTarget();

        if (endOfPathInstruction == EndOfPathInstruction.Stop)
        {
            path.path.EndOfPathActionStop += StopAtEndOfPath;
        }
        else if (endOfPathInstruction == EndOfPathInstruction.Destroy)
        {
            path.path.EndOfPathActionDestroy += DestroyAtEndOfPath;
        }
    }
    
    private void Update()
    {
        _aimedSpeed = this.gameObject.GetComponent<AimedSpeed>().GetAimedSpeed();

        if (Vector3.Distance(transform.position, _localTarget) < trackerSensitivity)
        {
            if (driveInReverse)
            {
                ReversePathFollowing();
            }
            else
            {
                NormalPathFollowing();
            }
        }
        
        
        Vector3 localTargetTransform =  transform.InverseTransformPoint(path.path.GetPointAtTime(_progressPercentage, endOfPathInstruction));
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

        if (!_manualOverride)
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

    private void NormalPathFollowing()
    {
        _localTarget = path.path.GetPointAtTime(_progressPercentage += precision, endOfPathInstruction);
    }
    
    private void ReversePathFollowing()
    {
        _localTarget = path.path.GetPointAtTime(_progressPercentage -= precision, endOfPathInstruction);
    }

    private void StopAtEndOfPath()
    {
        // todo implement
    }
    
    private void DestroyAtEndOfPath()
    {
        Destroy(this.transform.parent.gameObject);
    }

    #endregion
    
    #region Public methods
    public void SetLocalTarget()
    {
        _nearestPoint = path.path.GetClosestPointOnPath(transform.position);
        _progressPercentage = path.path.GetClosestTimeOnPath(_nearestPoint);
        _localTarget = path.path.GetPointAtTime(_progressPercentage, endOfPathInstruction);
    }
    
    public void SetManualOverride(bool manualState)
    {
        _manualOverride = manualState;
    }
    
    public void SetAimedSpeed(float newSpeed)
    {
        _aimedSpeed = newSpeed;
    }
    
    public bool GetIsReversed()
    {
        return driveInReverse;
    }
    #endregion
}