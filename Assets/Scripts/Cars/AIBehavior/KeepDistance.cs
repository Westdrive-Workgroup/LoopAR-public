using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class KeepDistance : MonoBehaviour
{
    [Space] [Header("Speed adjuster")] public float adjustFactor = 0.9f;

   // private float _defaultSpeed;
    
    private void Start()
    {
       // _defaultSpeed = this.gameObject.GetComponent<AIController>().GetRuleSpeed();
        //Debug.Log("Default Speed initial: " + _defaultSpeed);
    }

    private void FixedUpdate()
    {
        
        Vector3 fwd = transform.TransformDirection(Vector3.forward);
        RaycastHit hit;
        
        //_defaultSpeed = this.gameObject.GetComponent<AIControler>().aimedSpeed;

        if (Physics.Raycast(transform.position, fwd, out hit, 10.0f))
        {
            if (hit.collider.gameObject.GetComponent<CarController>() != null)
            {
                SpeedSetter(hit);
            }
        }
       //else
        //{
          //  SpeedReset();
        //}
        
    }

    
   // private void SpeedReset()
    //{
       // if (this.gameObject.GetComponent<AIController>().aimedSpeed != _defaultSpeed)
       // {
           // this.gameObject.GetComponent<AIController>().SetAimedSpeed(_defaultSpeed);
       // }
    //}

    
    private void SpeedSetter(RaycastHit hit)
    {
        float hitCurrentSpeed;
        
        hitCurrentSpeed = hit.collider.gameObject.GetComponent<CarController>().GetCurrentSpeed();
        float newSpeed = hitCurrentSpeed * adjustFactor;
            
        //this.gameObject.GetComponent<AIController>().SetAimedSpeed(newSpeed);
        this.gameObject.GetComponent<AimedSpeed>().SetAimedSpeed(newSpeed);
    }
}
