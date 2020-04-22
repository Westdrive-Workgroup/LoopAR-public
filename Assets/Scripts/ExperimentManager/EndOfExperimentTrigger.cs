using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class EndOfExperimentTrigger : MonoBehaviour
{
    private GameObject _currentTarget;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == _currentTarget)
            return;

        _currentTarget = other.gameObject;
        
        if (other.GetComponent<ManualController>() != null)
        {
            Debug.Log("<color=green>End of Experiment!</color>: ");
            ExperimentManager.Instance.EndTheExperiment();
        }
    }
}
