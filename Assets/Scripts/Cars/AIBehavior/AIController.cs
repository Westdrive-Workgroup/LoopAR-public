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
    public float localTargetVisualizerRadius  = 1f;
    public Color localTargetColor = Color.red;
    
    private CarController _carController;
    public float steeringSensitivity = 0.01f;
    
    public float accelerationCareFactor = 0.75f; //AIs in Racing games might constant push the gas pedal, I dont think that this is correct in ordinary traffic 

    public float brakeFactor = 1f; //Strong Brakes requires potentially a less aggressive braking behavior of the AI.
    private Vector3 _target;
    private Vector3 _nextTarget;
    private Rigidbody _carRigidBody;
    private float _targetAngle;
    private float _aimedSpeed;
    
    
    [Space] [Header("Path Settings")] public BezierSplines path;
    [Range(0f,0.1f)] public float precision = 0.01f;
    [Range(0.5f,20f)] public float trackerSensitivity = 5f;
    [Range(0f,1f)] public float progressPercentage;
    [SerializeField] public bool reverse;
    private Vector3 _localTarget;

    [Space] [Header("Car Mode")] public bool manualOverride;
    //public float trunTreshold = 30f;
    
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
        _carRigidBody = this.gameObject.GetComponent<Rigidbody>();
        _carController = this.GetComponent<CarController>();
        //_localTarget = path.GetPoint(0);
        //Debug.Log("target was at 0 " + _localTarget);
        _localTarget = GetClosestPoint(path);
        //Debug.Log("local target is "+ _localTarget);
        _targetAngle = 0;
        
        if (reverse)
        {
            // Debug.Log("recognized reverse");
            progressPercentage = 1f;
            //reverse = true;
            // Debug.Log("reverse value in awaik: " + reverse);
        }
        else
        {
            progressPercentage = 0f;
        }
        
        manualOverride = false;
    }

    private void Update()
    {
        reverse = true;
        // Debug.Log("reverse value beginning of update: " + reverse);
        _aimedSpeed = this.gameObject.GetComponent<AimedSpeed>().GetAimedSpeed();
        //Debug.Log(Vector3.Distance(transform.position, _localTarget));

        if (Vector3.Distance(transform.position, _localTarget) < trackerSensitivity)
        {
             Debug.Log("in update in if");
            if (reverse)
            {
                
                ReversePathFollowing();
            }
            else
            {
                NormalPathFollowing();
            }
        }
         Debug.Log("in update after if");

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
           // _carController.MoveVehicle(accel, brake, _targetAngle);
            
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
        Debug.Log("Beginning ReversePF methode. PP: " + progressPercentage);
        if (progressPercentage < 0f)
        {
            progressPercentage = 1f;
            Debug.Log("Reverse if reset. PP: " + progressPercentage);
        }
        else
        {
            progressPercentage -= precision;
            Debug.Log("Reverse else. PP: " + progressPercentage);
        }
        _localTarget = path.GetPoint(progressPercentage);
        Debug.Log("End ReversePF methode. PP: " + progressPercentage);
    }

    private void NormalPathFollowing()
    {
        Debug.Log("NormalPF methode");
        if (progressPercentage >= 1f)
        {
            progressPercentage = 0f;
            Debug.Log("Normal if reset. PP: " + progressPercentage);
        }
        else
        {
            Debug.Log("Normal else. PP: " + progressPercentage);
            progressPercentage += precision;
        }
        _localTarget = path.GetPoint(progressPercentage);
        Debug.Log("Normal PP: " + progressPercentage);
    }

    public void SetAimedSpeed(float newSpeed)
    {
        _aimedSpeed = newSpeed;
    }
    
    private Vector3 GetClosestPoint(BezierSplines path)
    { 
        Vector3 currentPoint = Vector3.zero;

        if (reverse)
        {
            for (float i = 1f; i <= 0f; i -= precision)
            {
                Vector3 point = path.GetPoint(i);
                
                if (Vector3.Distance(this.transform.position, point) <
                    (Vector3.Distance(this.transform.position, currentPoint)))
                {
                    currentPoint = point;
                }
                else
                {
                    progressPercentage = i;
                    break;
                }
            }
        }
        else
        {
            for (float i = 0f; i < 1f; i += precision)
            {
            
                Vector3 point = path.GetPoint(i);
            
                //Debug.Log("point is now" + point + " " + i + "sensitivy " + precision);
                //Debug.Log("Distance to currentPoint " + Vector3.Distance(this.transform.position, point));
                //Debug.Log("Distance to previous Point " + Vector3.Distance(this.transform.position, currentPoint));
                if (Vector3.Distance(this.transform.position, point) <
                    (Vector3.Distance(this.transform.position, currentPoint)))
                {
                    currentPoint = point;
                }
                else
                {
                    progressPercentage = i;
                    break;
                }
            }
        }
        
        return currentPoint;
    }
}