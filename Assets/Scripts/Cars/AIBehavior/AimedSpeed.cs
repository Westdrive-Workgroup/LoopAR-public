using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class AimedSpeed : MonoBehaviour
{
    private float _aimedSpeed; //everyone can get this 
    private float _ruleSpeed; //rule speed is at start Max Speed, until traffic signs set new rules. only traffic signs triggers can set this
    private float _eventSpeed; //only activated Eventtriggers can change this. Only needs to be set in start
    
    private bool _overWrittenAimedSpeed;
    private bool _eventSpeedActivated;
    
    // Start is called before the first frame update
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

    // Update is called once per frame
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

    public void SetAimedSpeed(float speed)
    {
        _overWrittenAimedSpeed = true;
        _aimedSpeed = speed;
    }

    public float GetAimedSpeed()
    {
        return _aimedSpeed;
    }
    

    public void ActivateEventSpeed()
    {
        _eventSpeedActivated=true;
    }

    public void DeActivateEventSpeed()
    {
        _eventSpeedActivated = false;
    }
    
    
    public void SetRuleSpeed(float speed)
    {
        _ruleSpeed = speed;
    }
}
