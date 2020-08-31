using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using PathCreation;
// using PathCreationEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

[DisallowMultipleComponent]
public class AIController : MonoBehaviour
{
    #region Fields

    private CarController _carController;
    
    // curve
    private float _angle;
    
    // Gizmos
    [Space] [Header("Debug")] public bool showLocalTargetGizmos = false;
    [Range(0f,20f)]
    private float _localTargetVisualizerRadius  = 5f;
    [SerializeField] private Color localTargetColor = Color.magenta;
    
    // Target
    private Vector3 _target;
    private Vector3 _nextTarget;
    private float _targetAngle;
    private float _aimedSpeed;
    private Vector3 _nearestPoint = Vector3.zero;
    private Vector3 _localTarget;
    private Vector3 _curveDetector;
    
    // Path
    [Space] [Header("Path Settings")] 
    [SerializeField] private PathCreator path;
    [SerializeField] private EndOfPathInstruction endOfPathInstruction;    // todo implement
    [Range(0,0.5f)][SerializeField] private float curveDetectorStepAhead = 0.005f;
    [Tooltip("Distance of follower to the target on the path.")]
    [Range(0,0.1f)] [SerializeField] private float precision = 0.001f;
    [Range(0.5f,20f)] [SerializeField] private float trackerSensitivity = 5f;
    private float _defaultTrackerSensitivity;
    private float _progressPercentage;
    private int _nearestNormalIndex;

    // Driving behavior
    [Space][Header("Driving behavior")]
    [SerializeField] private bool driveInReverse;
    // [SerializeField] private float steeringSensitivity = 0.01f;    // todo use this
    /*[SerializeField] */private float accelerationCareFactor = 0.75f; //AIs in Racing games might constant push the gas pedal, I dont think that this is correct in ordinary traffic 
    /*[SerializeField] */private float brakeFactor = 1f; //Strong Brakes requires potentially a less aggressive braking behavior of the AI.
    private bool _manualOverride;
    
    

    #endregion

    #region Private methods
    private void OnDrawGizmosSelected()
    {
        if (showLocalTargetGizmos)
        {
            Gizmos.color = localTargetColor;
            Gizmos.DrawWireSphere(_localTarget,_localTargetVisualizerRadius);
            Gizmos.DrawSphere(_curveDetector, 1);
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
        
        SetLocalTargetAndCurveDetection();

        if (endOfPathInstruction == EndOfPathInstruction.Stop)
        {
            path.path.EndOfPathActionStop += StopAtEndOfPath;
        }
        else if (endOfPathInstruction == EndOfPathInstruction.Destroy)
        {
            path.path.EndOfPathActionDestroy += DestroyAtEndOfPath;
        }

        _localTargetVisualizerRadius = trackerSensitivity;
        _defaultTrackerSensitivity = trackerSensitivity;
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
            accel = Mathf.Lerp(0, accelerationCareFactor, 1 - cornerFactor);
        }

        if (!_manualOverride)
        {
            if (_carController.GetCurrentSpeed() >= _aimedSpeed)
            {
                brake += 1f;
                _carController.MoveVehicle(accel, brake, _targetAngle);
            }
            else
            {
                _carController.MoveVehicle(accel, brake, _targetAngle);
            }
        }
        
        // Curve detection
        Vector3 targetDir = _curveDetector - transform.position;
        _angle = Vector3.Angle(targetDir, transform.forward);
        
        this.gameObject.GetComponent<CurveDrivingBehaviour>().AdjustSpeedAtCurve(_angle);
        
        _localTargetVisualizerRadius = trackerSensitivity;

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (this.gameObject.GetComponent<ManualController>() != null)
            {
                this.gameObject.transform.position = _curveDetector;
                SetLocalTargetAndCurveDetection();
            }
        }
    }

    private void NormalPathFollowing()
    {
        _localTarget = path.path.GetPointAtTime(_progressPercentage += precision, endOfPathInstruction);
        _curveDetector = path.path.GetPointAtTime(_progressPercentage + curveDetectorStepAhead, endOfPathInstruction);
    }
    
    private void ReversePathFollowing()
    {
        _localTarget = path.path.GetPointAtTime(_progressPercentage -= precision, endOfPathInstruction);
        _curveDetector = path.path.GetPointAtTime(_progressPercentage - curveDetectorStepAhead, endOfPathInstruction);
    }

    private void StopAtEndOfPath()
    {
        // todo implement
    }
    
    private void DestroyAtEndOfPath()
    {
        Destroy(this.transform.parent.gameObject);
    }

    IEnumerator ResetTrackerSensitivityAfterEvent()
    {
        float seconds = SceneManager.GetActiveScene().name == "Westbrueck" ? 0 : 2;

        yield return new WaitForSeconds(seconds);
        trackerSensitivity = _defaultTrackerSensitivity;
        _localTargetVisualizerRadius = trackerSensitivity;
    }

    #endregion
    
    #region Public methods
    public void SetLocalTargetAndCurveDetection()
    {
        _nearestPoint = path.path.GetClosestPointOnPath(transform.position);
        _progressPercentage = path.path.GetClosestTimeOnPath(_nearestPoint);
        _localTarget = path.path.GetPointAtTime(_progressPercentage, endOfPathInstruction);
        _curveDetector = _localTarget + new Vector3(0,0,curveDetectorStepAhead);
    }
    
    public void SetLocalTargetForEvents(Vector3 position)
    {
        _nearestPoint = path.path.GetClosestPointOnPath(position);
        _progressPercentage = path.path.GetClosestTimeOnPath(_nearestPoint);
        _localTarget = path.path.GetPointAtTime(_progressPercentage, endOfPathInstruction);
        _curveDetector = _localTarget + new Vector3(0,0,curveDetectorStepAhead);
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
    
    public Vector3 GetLocalTarget()
    {
        return _localTarget;
    }
    
    public void SetNewPath(PathCreator newPath)
    {
        path = newPath;
        SetLocalTargetAndCurveDetection();
    }
    
    public void SetNewPath(PathCreator newPath, float newCurveDetectorStepAhead, float newPrecision, float newTrackerSensitivity)
    {
        path = newPath;
        curveDetectorStepAhead = newCurveDetectorStepAhead;
        precision = newPrecision;
        trackerSensitivity = newTrackerSensitivity;
        SetLocalTargetAndCurveDetection();
    }

    // during the events
    public void SetTrackerSensitivity(float newTrackerSensitivity = 15)
    {
        trackerSensitivity = newTrackerSensitivity;
        _localTargetVisualizerRadius = trackerSensitivity;
    }
    
    public void ReSetTrackerSensitivity()
    {
        StartCoroutine(ResetTrackerSensitivityAfterEvent());
    }

    #endregion
}