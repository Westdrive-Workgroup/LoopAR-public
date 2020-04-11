using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Schema;
using UnityEngine;
using UnityEngine.XR.WSA.Input;

/// <summary>
/// is respondible for activating groups of game objects
/// </summary>

[DisallowMultipleComponent]
public class ActivationTrigger : MonoBehaviour
{
    [SerializeField] private GameObject targetGroup;

    private ActivationManager _activationManager;
    private GameObject _currentTarget;

    // Start is called before the first frame update
   
    void Start()
    {
        _activationManager = targetGroup.GetComponent<ActivationManager>();
        ExperimentManager.Instance.RegisterToExperimentManager(this);
        Debug.Log("registered");
    }

    public void DeactivateTheGameObjects()
    {
        _activationManager.ChangeActivationState(false, targetGroup);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == _currentTarget)
            return;

        _currentTarget = other.gameObject;
        
        if (other.GetComponent<ManualController>() != null)
        {
            _activationManager.ChangeActivationState(targetGroup);
        }
    }
}
