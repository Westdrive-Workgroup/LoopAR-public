using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Matrix4x4 = System.Numerics.Matrix4x4;

[DisallowMultipleComponent]
public class RestrictedZoneTrigger : MonoBehaviour
{
    private CriticalEventController _controller;    
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<ManualController>() != null)
        {
            _controller.StopEndIdleEvent();
            ExperimentManager.Instance.ParticipantFailed();
        }
    }

    public void SetController(CriticalEventController controller)
    {
        _controller = controller;
    }
}
