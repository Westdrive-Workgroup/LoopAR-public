using System;
using System.Collections;
using System.Collections.Generic;
using RoboRyanTron.Unite2017.Events;
using RoboRyanTron.Unite2017.Variables;
using UnityEngine;
using UnityEngine.Serialization;

public class TestEventManager : MonoBehaviour
{
    [Tooltip("Switches to manual control for true, to AI control for false.")]
    [SerializeField] private bool manualDriving;
    
    [Tooltip("Specifies for how many seconds control is switched. If 0 is entered, control will be handed over indefinitely.")]
    [SerializeField] private int timeForControl;

    [Tooltip("Raises this game event if one is given and none if left empty.")]
    [SerializeField] private GameObject pylonEvent;

    [Tooltip("The respawn point for when the free drive is over.")]
    [SerializeField] private GameObject respawnPoint;
    
    [Tooltip("The objects that should be deactivated at the start of the scene.")]
    [SerializeField] private GameObject[] pylonEventObjects;
    
    [SerializeField] private bool resetValue;

    [SerializeField] private GameObject startTrigger;

    [SerializeField] private FloatVariable maxTrials;
    [SerializeField] private FloatVariable trialsDone;

    [SerializeField] private GameObject _participantCar;
    private bool sceneStart;
    private ManualController manualController;

    private void Awake()
    {
        manualController = _participantCar.GetComponent<ManualController>();
    }

    private void Start()
    {
        if (resetValue)
        {
            trialsDone.SetValue(0);
        }
        
        sceneStart = true;
        
        foreach (var deactivateObjects in pylonEventObjects)
        {
            deactivateObjects.SetActive(false);
        }

        manualController.enabled = false;
    }

    public void StartTestDrive()
    {
        manualController.enabled = true;
        startTrigger.SetActive(true);
    }

    public void StartTrigger(Collider other)
    {
        if (other.GetComponent<CarController>())
        {
            //_participantCar = other.gameObject;
            StartCoroutine(PassControl(timeForControl));
        }
    }
    
    public void EndTrigger(Collider other)
    {
        FinishEvent();
        if (other.GetComponent<CarController>())
        {
            StartCoroutine(PassControl(_participantCar, 0, false));
        }
    }
    
    public void TrialEndTrigger()
    {
        CalibrationManager.Instance.TestDriveSuccessful();
    }
    
    IEnumerator PassControl(int time)
    {
        if (time == 0)
        {
            yield break;
        }
        yield return new WaitForSeconds(timeForControl);
        ControlEnded();
    }
    
    IEnumerator PassControl(GameObject car, int time, bool manualControl)
    {
        if (time == 0)
        {
            car.GetComponent<ControlSwitch>().SwitchControl(manualControl);
            yield break;
        }
        car.GetComponent<ControlSwitch>().SwitchControl(manualControl);
        yield return new WaitForSeconds(timeForControl);
        ControlEnded();
    }

    private void ControlEnded()
    {
        Debug.Log("ControlEnded");
        if (sceneStart)
        {
            sceneStart = false;
            ActivateEvent();
            ResetCar(_participantCar);
            pylonEvent.GetComponent<AudioSource>().Play();
        }
    }

    private void ActivateEvent()
    {
        foreach (var activateObjects in pylonEventObjects)
        {
            activateObjects.SetActive(true);
        }
    }

    private void FinishEvent()
    {
        _participantCar.GetComponent<AimedSpeed>().SetAimedSpeed(80);
        pylonEvent.SetActive(false);
        
    }

    public void TrialFailed()
    {
        if (trialsDone.Value <= maxTrials.Value)
        {
            maxTrials.ApplyChange(1);
            //Plus whatever the HUD is gonna do
        }
        //whatever happens when they fail
        else
        {
            CalibrationManager.Instance.TestDriveFailed();
        }
    }

    private void ResetCar(GameObject objectToReset)
    {
        if (objectToReset.GetComponent<ManualController>())
        {
            objectToReset.transform.SetPositionAndRotation(respawnPoint.gameObject.transform.position, respawnPoint.gameObject.transform.rotation);
            objectToReset.GetComponent<Rigidbody>().velocity = new Vector3(0,0,0);
        }
        
    }
}
