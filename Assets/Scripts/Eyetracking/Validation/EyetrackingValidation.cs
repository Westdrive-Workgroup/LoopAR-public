using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Valve.VR.InteractionSystem;
using ViveSR.anipal.Eye;

public class EyetrackingValidation : MonoBehaviour
{


    public float distance;
    public List<Vector3> keyPositions;
    private int validationPointIdx;
    private int validationTrial;
    private Transform _hmdTransform;
    private EyeValidationData _eyeValidationData;
    private bool _isRunning;
    private IEnumerator runningValidation;
    public delegate void OnFinishedEyeValidation(bool wasSuccessful);
    public event OnFinishedEyeValidation NotifyEyeValidationObservers;

    
    private void Start()
    {
        _hmdTransform = EyetrackingManager.Instance.GetHmdTransform();
        _isRunning = false;
    }

    public void AbortValidation()
    {
        if (_isRunning)
        {
            Debug.Log("EyeValidation was aborted");
            StopCoroutine(runningValidation);
            gameObject.transform.position = Vector3.zero;
            _isRunning = false;
            gameObject.SetActive(false);
        }
    }
    public void StartValidation(float delay)
    {
        if (!_isRunning)
        {
            _isRunning = true;
            gameObject.SetActive(true);
            runningValidation = Validate(delay);
            StartCoroutine(runningValidation);
        }
    }
    

    private IEnumerator Validate(float delay)
    {
        yield return new WaitForSeconds(delay);
        List<float> anglesX = new List<float>();
        List<float> anglesY = new List<float>();
        List<float> anglesZ = new List<float>();
        validationTrial += 1;
        float startTime = Time.time;
        
        for (int i = 1; i < keyPositions.Count; i++)
        {
            startTime = Time.time;
            float timeDiff = 0;
            while (timeDiff < 1f)
            {
                transform.position = _hmdTransform.position + _hmdTransform.rotation * Vector3.Lerp(keyPositions[i-1], keyPositions[i], timeDiff / 1f);   
                transform.LookAt(_hmdTransform);
                yield return new WaitForEndOfFrame();
                timeDiff = Time.time - startTime;
            }

            validationPointIdx = i;
            startTime = Time.time;
            timeDiff = 0;
            
            while (timeDiff < 3f)
            {
                transform.position = _hmdTransform.position + _hmdTransform.rotation * keyPositions[i] ;
                transform.LookAt(_hmdTransform);
                EyeValidationData validationData = GetEyeValidationData();


                if (validationData != null)
                {
                    anglesX.Add(validationData.CombinedEyeAngleOffset.x);
                    anglesY.Add(validationData.CombinedEyeAngleOffset.y);
                    anglesZ.Add(validationData.CombinedEyeAngleOffset.z);
                    
                    validationData.ValidationResults.x = CalculateValidationError(anglesX);
                    validationData.ValidationResults.y = CalculateValidationError(anglesY);
                    validationData.ValidationResults.z = CalculateValidationError(anglesZ);

                    _eyeValidationData = validationData;

                    EyetrackingManager.Instance.StoreEyeValidationData(_eyeValidationData); 
                    //validationSample.validationData.ValidationTrial);
                }
                
                yield return new WaitForEndOfFrame();
                timeDiff = Time.time - startTime;
            }
        }

        
        string validationResult = "(" + CalculateValidationError(anglesX).ToString("0.00") +
                                    ", " +
                                    CalculateValidationError(anglesY).ToString("0.00") +
                                    ", " +
                                    CalculateValidationError(anglesZ).ToString("0.00") + ")";
        Debug.Log("<color=yellow> Validation Results"+ validationResult+ "(</color>");
        gameObject.transform.position = Vector3.zero;
        
        if (CalculateValidationError(anglesX) > 1 || CalculateValidationError(anglesY) > 1 ||
            CalculateValidationError(anglesZ) > 1)
        {
            _isRunning = false;
            NotifyEyeValidationObservers?.Invoke(false);
            Debug.LogWarning("<color=red>Validation Error is too big (error angles >1) , please relaunch a calibration first </color>");
        }
        else
        {
            _isRunning = false;
            NotifyEyeValidationObservers?.Invoke(true);
        }
        
        gameObject.SetActive(false);
        
    }


    private float CalculateValidationError(List<float> angles)
    {
        return angles.Select(f => f > 180 ? Mathf.Abs(f - 360) : Mathf.Abs(f)).Sum() / angles.Count;
    }
    

    private EyeValidationData GetEyeValidationData()
    {
        EyeValidationData eyeValidationData = new EyeValidationData();
        
        Ray ray;
        
        eyeValidationData.ValidationTrial = validationTrial;
        eyeValidationData.ValidationPointIdx = validationPointIdx;
        
        //block und participant ID fehlen aktuell 
        var debText = "";
        eyeValidationData.UnixTimestamp = GetCurrentTimestamp();
        eyeValidationData.Timestamp = Time.realtimeSinceStartup;
        
        eyeValidationData.HeadTransform = _hmdTransform.transform;
        
        eyeValidationData.PointToFocus = transform.position;

        if (SRanipal_Eye.GetGazeRay(GazeIndex.LEFT, out ray))
        {
            var angles = Quaternion.FromToRotation((transform.position - _hmdTransform.position).normalized, _hmdTransform.rotation * ray.direction)
                .eulerAngles;
            
            eyeValidationData.LeftEyeAngleOffset = angles;
        }
        
        if (SRanipal_Eye.GetGazeRay(GazeIndex.RIGHT, out ray))
        {
            var angles = Quaternion.FromToRotation((transform.position - _hmdTransform.position).normalized, _hmdTransform.rotation * ray.direction)
                .eulerAngles;
            debText += "Right Eye: " + angles + "\n";
            eyeValidationData.RightEyeAngleOffset = angles;
        }

        if (SRanipal_Eye.GetGazeRay(GazeIndex.COMBINE, out ray))
        {
            var angles = Quaternion.FromToRotation((transform.position - _hmdTransform.position).normalized, _hmdTransform.rotation * ray.direction)
                .eulerAngles;
            debText += "Combined Eye: " + angles + "\n"; 
            eyeValidationData.CombinedEyeAngleOffset = angles;
        }

        return eyeValidationData;
    }
    
    private double GetCurrentTimestamp()
    {
        System.DateTime epochStart = new System.DateTime(1970, 1, 1, 0, 0, 0, System.DateTimeKind.Utc);
        return (System.DateTime.UtcNow - epochStart).TotalSeconds;
    }



}
