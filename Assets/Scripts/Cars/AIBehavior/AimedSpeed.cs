using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CarController))]
[DisallowMultipleComponent]
public class AimedSpeed : MonoBehaviour
{
    private float _aimedSpeed; //everyone can get this 
    private float _ruleSpeed; //rule speed is at start Max Speed , until traffic signs set new rules. only traffic signs triggers can set this
    private float _eventSpeed; //only activated Eventtriggers can change this only needs to be set in start


    private bool overWrittenAimedSpeed;

    private bool EventSpeedActivated;
    // Start is called before the first frame update
    void Start()
    {
        overWrittenAimedSpeed = false;
        _ruleSpeed = GetComponent<CarController>().GetMaximumSpeed();
        _eventSpeed = PersistentTrafficEventManager.Instance.EventSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        if (EventSpeedActivated)
        {
            _aimedSpeed = _eventSpeed;
            return;
        }
        
        if (!overWrittenAimedSpeed) 
        {
            //if (EventSpeedActivated)
           // {
           //     _aimedSpeed = _eventSpeed;
          //  }
           // else
          //  {
                _aimedSpeed = _ruleSpeed;  
          //  }
            
        }
        
        overWrittenAimedSpeed = false;
    }

    public void SetAimedSpeed(float speed)
    {
        overWrittenAimedSpeed = true;
        _aimedSpeed = speed;
    }

    public float GetAimedSpeed()
    {
        return _aimedSpeed;
    }
    

    public void ActivateEventSpeed()
    {
        EventSpeedActivated=true;
    }

    public void DeActivateEventSpeed()
    {
        EventSpeedActivated = false;
    }
    
    
    public void SetRuleSpeed(float speed)
    {
        _ruleSpeed = speed;
    }
    
}
