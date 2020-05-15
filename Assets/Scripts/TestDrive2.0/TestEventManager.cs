using System;
using System.Collections;
using Boo.Lang;
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
    [SerializeField] private GameObject[] eventObjects;

    [SerializeField] private System.Collections.Generic.List<GameObject> eventObjectsToMark;
    
    [SerializeField] private bool resetValue;

    [SerializeField] private GameObject startTrigger;

    [SerializeField] private FloatVariable maxTrials;
    [SerializeField] private FloatVariable trialsDone;

    [SerializeField] private GameObject _participantCar;
    [SerializeField] private WindscreenHUD windscreenHUD;
    [SerializeField] private HudLite_TestDrive hudLite;
    private ManualController manualController;

    private bool sceneStart;


    private void Awake()
    {
        manualController = _participantCar.GetComponent<ManualController>(); ;
    }

    private void Start()
    {
        if (resetValue)
        {
            trialsDone.SetValue(0);
        }
        
        manualController.enabled = false;
        hudLite.gameObject.SetActive(false);
        windscreenHUD.gameObject.SetActive(false);
        
        startTrigger.SetActive(false);
        sceneStart = true;
        
        foreach (var deactivateObjects in eventObjects)
        {
            deactivateObjects.SetActive(false);
        }
    }

    public void StartTestDrive()
    {
        manualController.enabled = true;
        startTrigger.SetActive(true);
        StartCoroutine(PassControl());
    }

    public void StartTrigger()
    {
        windscreenHUD.gameObject.SetActive(true);
        hudLite.gameObject.SetActive(true);
    }
    
    public void EndTrigger(Collider other)
    {
        //FinishEvent();
        windscreenHUD.gameObject.SetActive(true);
        hudLite.gameObject.SetActive(true);

        Debug.Log("The End is nigh!");
        ResetCar(_participantCar);
        _participantCar.gameObject.GetComponent<ControlSwitch>().SwitchControl(true);
        Debug.Log("Control Switched");
    }
    
    public void TrialEndTrigger()
    {
        CalibrationManager.Instance.TestDriveSuccessful();
    }
    
    IEnumerator PassControl()
    {
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
        if (sceneStart)
        {
            sceneStart = false;
            ActivateEvent();
            ResetCar(_participantCar);
            //pylonEvent.GetComponent<AudioSource>().Play();
        }
    }

    private void ActivateEvent()
    {
        foreach (var activateObjects in eventObjects)
        {
            activateObjects.SetActive(true);
        }
        windscreenHUD.gameObject.SetActive(true);
        hudLite.gameObject.SetActive(true);

        //_participantCar.GetComponent<ControlSwitch>().SwitchControl(false);
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
        if (objectToReset.GetComponent<CarController>())
        {
            objectToReset.GetComponent<Rigidbody>().velocity = Vector3.zero;
            objectToReset.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;

            objectToReset.transform.SetPositionAndRotation(respawnPoint.gameObject.transform.position, respawnPoint.gameObject.transform.rotation);
            Debug.Log("ResetCar!");
        }
    }

    public void ActivateHUD()
    {
       //windscreenHUD.gameObject.SetActive(true);
       windscreenHUD.DriverAlert();
       //hudLite.gameObject.SetActive(true);
       hudLite.DriverAlert();
       hudLite.ActivateHUD(eventObjectsToMark);
    }
}
