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
            _controller.SetEventEndData(TimeManager.Instance.GetCurrentUnixTimeStamp(), false, this.gameObject.name);
            _controller.StopEndIdleEvent();
            _controller.ResetEventObjectsActivationStates();
            ExperimentManager.Instance.ParticipantFailed();
        }
    }

    public void SetController(CriticalEventController controller)
    {
        _controller = controller;
    }
}
