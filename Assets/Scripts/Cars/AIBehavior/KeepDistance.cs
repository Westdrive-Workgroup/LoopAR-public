using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class KeepDistance : MonoBehaviour
{
    [Space] [Header("Speed adjuster")] public float speedAdjustmentFactor = 0.9f;
    
    private void FixedUpdate()
    {
        Vector3 fwd = transform.TransformDirection(Vector3.forward);
        RaycastHit hit;

        if (Physics.Raycast(transform.position, fwd, out hit, 15.0f))
        {
            if (hit.collider.gameObject.GetComponent<CarController>() != null)
            {
                SpeedSetter(hit);
            }
        }
    }

    private void SpeedSetter(RaycastHit hit)
    {
        float hitCurrentSpeed;
        
        hitCurrentSpeed = hit.collider.gameObject.GetComponent<CarController>().GetCurrentSpeed();
        float newSpeed = hitCurrentSpeed * speedAdjustmentFactor;
        
        this.gameObject.GetComponent<AimedSpeed>().SetAimedSpeed(newSpeed);
    }
}
