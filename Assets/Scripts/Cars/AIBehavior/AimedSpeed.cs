using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental;
using UnityEngine;

[DisallowMultipleComponent]
public class AimedSpeed : MonoBehaviour
{
    #region Fields

    private float _aimedSpeed;    //everyone can get this 
    private float _ruleSpeed;    //rule speed is at start Max Speed, until traffic signs set new rules. only traffic signs triggers can set this
    private float _eventSpeed;    //only activated Eventtriggers can change this. Only needs to be set in start
    
    private bool _overWrittenAimedSpeed;
    private bool _eventSpeedActivated;
    private bool _coroutineIsRunning;

    private float _counter = 0;

    #endregion

    #region PrivateMethods

    void Start()
    {
        _overWrittenAimedSpeed = false;
        _ruleSpeed = GetComponent<CarController>().GetMaximumSpeed();
        if (PersistentTrafficEventManager.Instance != null)
        {
            _eventSpeed = PersistentTrafficEventManager.Instance.GetEventSpeed();
        }
        else
        {
            _eventSpeed = 30f;         //will never be used , but just in case
        }
    }

    void Update()
    {
        if (_eventSpeedActivated)
        {
            _aimedSpeed = _eventSpeed;
            return;
        }
        
        if (!_overWrittenAimedSpeed) 
        {
            _aimedSpeed = _ruleSpeed;
        }
        
        _overWrittenAimedSpeed = false;
    }
    
    private IEnumerator CurveSpeedTimer(float speed)
    {
        while (_counter < 2)
        {
            _coroutineIsRunning = true;
            SetAimedSpeed(speed/3.6f);
            _counter += 1;
            yield return new WaitForSeconds(1);
        }
        
        _coroutineIsRunning = false;
    }

    #endregion
    
    #region PublicMethods

    public void ActivateEventSpeed()
    {
        _eventSpeedActivated=true;
    }

    public void DeActivateEventSpeed()
    {
        _eventSpeedActivated = false;
    }
    
    public void InitiateCurvePhase(float speed)
    {
        if (_coroutineIsRunning)
        {
            StopCoroutine(CurveSpeedTimer(speed));
        }
        
        _counter = 0;
        StartCoroutine(CurveSpeedTimer(speed));
    }

        #region Setters
    
        public void SetAimedSpeed(float speed)
        {
            _overWrittenAimedSpeed = true;
            _aimedSpeed = speed;
        }
        
        public void SetRuleSpeed(float speed)
        {
            _ruleSpeed = speed;
        }
    
        #endregion
        
        #region Getters
    
        public float GetAimedSpeed()
        {
            return _aimedSpeed;
        }
    
        public float GetRuleSpeed()
        {
            return _ruleSpeed * 3.6f;
        }
    
        #endregion

    #endregion
}
