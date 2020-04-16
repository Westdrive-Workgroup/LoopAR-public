using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

[DisallowMultipleComponent]
public class AIController : MonoBehaviour
{
    [Space] [Header("Debug")] public bool showLocalTargerGizmos = false;
    [Range(0f,100f)]
    [SerializeField] private float localTargetVisualizerRadius  = 10f;
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
    
    
    [Space] [Header("Path Settings")] public BezierSplines path;
    [Range(0f,0.1f)] public float precision = 0.01f;
    [Range(0.5f,20f)] public float trackerSensitivity = 10f;
    [Range(0f,1f)] public float progressPercentage;
    [SerializeField] public bool reverse;
    
    private Vector3 _localTarget;
    private Vector3 _nearestPoint = Vector3.zero;

    [Space] [Header("Car Mode")] public bool manualOverride;
    
    
    private void OnDrawGizmosSelected()
    {
        if (showLocalTargerGizmos)
        {
            Gizmos.color = localTargetColor;
            Gizmos.DrawWireSphere(_localTarget,localTargetVisualizerRadius);
        }
    }

    private void Start()
    {
        // Debug.Log("Heeeey! I have to be first!");
        
        //Debug.Log("target was at 0 " + _localTarget);
        //Debug.Log("local target is "+ _localTarget);

        if (reverse)
        {
            // Debug.Log("Heeeey! I have to be second!");
            // Debug.Log("recognized reverse");
            progressPercentage = 1f;
            // _localTarget = path.GetPoint(1f);
            //reverse = true;
            // Debug.Log("reverse value in start is " + reverse + " and PP is: " + progressPercentage);
        }
        else
        {
            progressPercentage = 0f;
            // _localTarget = path.GetPoint(0f);
        }
        
        
        _carRigidBody = this.gameObject.GetComponent<Rigidbody>();
        _carController = this.GetComponent<CarController>();
        
        _targetAngle = 0;
        manualOverride = false;
        _localTarget = GetClosestPoint(path);
    }

    private void Update()
    {
        // Debug.Log("reverse value beginning of update is " + reverse + " and PP is: " + progressPercentage);
        _aimedSpeed = this.gameObject.GetComponent<AimedSpeed>().GetAimedSpeed();
        //Debug.Log(Vector3.Distance(transform.position, _localTarget));

        if (Vector3.Distance(transform.position, _localTarget) < trackerSensitivity)
        {
             // Debug.Log("in update in if");
            if (reverse)
            {
                
                ReversePathFollowing();
            }
            else
            {
                // Debug.Log("got here");
                NormalPathFollowing();
            }
        }
        
        // Debug.Log("in update after if. PP: " + progressPercentage);

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
        // Debug.Log("Beginning of ReversePF methode. PP: " + progressPercentage);
        if (progressPercentage < 0f)
        {
            progressPercentage = 1f;
            // Debug.Log("Reverse if, has to be 1. PP: " + progressPercentage);
        }
        else
        {
            progressPercentage -= precision;
            // Debug.Log("Reverse else. PP: " + progressPercentage);
        }
        _localTarget = path.GetPoint(progressPercentage);
        // Debug.Log("End ReversePF methode. PP: " + progressPercentage);
    }

    private void NormalPathFollowing()
    {
        // Debug.Log("NormalPF methode");
        if (progressPercentage >= 1f)
        {
            progressPercentage = 0f;
            // Debug.Log("Normal if reset. PP: " + progressPercentage);
        }
        else
        {
            // Debug.Log("Normal else. PP: " + progressPercentage);
            progressPercentage += precision;
        }
        _localTarget = path.GetPoint(progressPercentage);
        // Debug.Log("Normal PP: " + progressPercentage);
    }

    public void SetAimedSpeed(float newSpeed)
    {
        _aimedSpeed = newSpeed;
    }
    
    private Vector3 GetClosestPoint(BezierSplines path)
    { 
        // Debug.Log("In Get to the closest point. PP is: " + progressPercentage);

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

    public bool GetIsReversed()
    {
        return reverse;
    }
}