using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class SpeedLimit : MonoBehaviour
{
    [Space] [Header("Speed Limit")]
    
    [Tooltip("0 - 100")] [Range(0, 100)] [SerializeField] private float speedInNormalPath = 50f;
    [Tooltip("0 - 100")] [Range(0, 100)] [SerializeField] private float speedInReversePath = 50f;


    private void OnTriggerEnter(Collider other)
    {
        AIController aiController = other.GetComponent<AIController>();
        AimedSpeed aimedSpeed = other.GetComponent<AimedSpeed>();
        
        if (aiController != null)
        {
            if (aiController.GetIsReversed() == false)
            {
                
                aimedSpeed.SetRuleSpeed(speedInNormalPath/3.6f);
            }
            else
            {
                aimedSpeed.SetRuleSpeed(speedInReversePath/3.6f);
            }
        }
    }
}
