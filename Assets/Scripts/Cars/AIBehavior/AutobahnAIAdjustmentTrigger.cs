using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutobahnAIAdjustmentTrigger : MonoBehaviour
{
    private GameObject _currentTarget;

    void Start()
    {
        this.gameObject.GetComponent<MeshRenderer>().enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == _currentTarget)
            return;

        _currentTarget = other.gameObject;

        if (other.GetComponent<ManualController>() != null)
        {
            other.gameObject.GetComponent<ChangeCarPath>().SetParticipantsCarPath(AutobahnManager.Instance.GetCarPath(), 
                AutobahnManager.Instance.GetCurveDetectorStepAhead(), 0.008f, 10f, "Autobahn");
        }
    }
}
