using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Schema;
using UnityEngine;

/// <summary>
/// is responsible for activating groups of game objects
///
/// this script has to be executed before ExperimentManager
/// </summary>

[DisallowMultipleComponent]
public class ActivationTrigger : MonoBehaviour
{
    [Tooltip("The game object which should be activated or deactivated when the participant's car drives through")]
    [SerializeField] private GameObject targetGroup;

    [Space] [Header("Functionality of the trigger")] 
    [SerializeField] private bool _activate;

    private ActivationHandler _activationHandler;
    private GameObject _currentTarget;

    // Start is called before the first frame update
   
    void Start()
    {
        _activationHandler = targetGroup.GetComponent<ActivationHandler>();
        ExperimentManager.Instance.RegisterToExperimentManager(this);
    }

    public void DeactivateTheGameObjects()
    {
        _activationHandler.ChangeActivationState(false, targetGroup);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == _currentTarget)
            return;

        _currentTarget = other.gameObject;
        
        if (other.GetComponent<ManualController>() != null)
        {
            if (_activate)
            {
                _activationHandler.ChangeActivationState(true, targetGroup);
            }
            else
            {
                _activationHandler.ChangeActivationState(false, targetGroup);
            }
            Debug.Log("<color=green>Went through</color>: " + this.gameObject.name);
        }
    }
}
