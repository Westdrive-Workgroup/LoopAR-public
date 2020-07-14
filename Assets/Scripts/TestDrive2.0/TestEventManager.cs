using System;
using System.Collections;
using Boo.Lang;
using RoboRyanTron.Unite2017.Variables;
using UnityEngine;
using UnityEngine.Serialization;

public class TestEventManager : MonoBehaviour
{
    [Tooltip("Specifies for how many seconds control is switched. If 0 is entered, control will be handed over indefinitely.")]
    [SerializeField] private int timeForControl;

    [Tooltip("The respawn point for when the free drive is over.")]
    [SerializeField] private GameObject respawnPoint;
    
    [Tooltip("The objects that should be deactivated at the start of the scene.")]
    private System.Collections.Generic.List<GameObject> eventObjects;
    [SerializeField] private GameObject eventObjectsParent;

    private System.Collections.Generic.List<GameObject> eventObjectsToMark;
    [SerializeField] private GameObject eventObjectsToMarkParent;
    
    [SerializeField] private GameObject startTrigger;

    [SerializeField] private bool resetValue;
    [SerializeField] private FloatVariable trialsDone;
    [SerializeField] private FloatVariable maxTrials;

    

    [SerializeField] private GameObject _participantCar;
    [SerializeField] private HUD_Advance advanced_HUD;

    [SerializeField] private WindscreenHUD windscreenHUD;
    [SerializeField] private HudLite_TestDrive hudLite;
    private ManualController manualController;
    
    [SerializeField] private float timeBeforeObjectsGetMarked;
    [SerializeField] private float delayTillHudActivates;


    private bool sceneStart;


    private void Awake()
    {
        advanced_HUD = advanced_HUD.GetComponent<HUD_Advance>();
        manualController = _participantCar.GetComponent<ManualController>();
        eventObjects = new System.Collections.Generic.List<GameObject>();
        eventObjectsToMark = new System.Collections.Generic.List<GameObject>();
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

        if (eventObjectsParent != null)
        {
            foreach (Transform child in eventObjectsParent.transform)
            {
                eventObjects.Add(child.gameObject);
                child.gameObject.SetActive(false);
            }
        }
        
        if (eventObjectsToMarkParent != null)
        {
            foreach (Transform child in eventObjectsToMarkParent.transform)
            {
                eventObjectsToMark.Add(child.gameObject);
            }
        }
    }

    public void StartTestDrive()
    {
        CameraManager.Instance.FadeIn();
        advanced_HUD.ManualDrive();
        manualController.enabled = true;
        startTrigger.SetActive(true);
        StartCoroutine(PassControl());
    }
    
    public void EndTrigger(Collider other)
    {
        Debug.Log("The End is nigh!");
        _participantCar.gameObject.GetComponent<ControlSwitch>().SwitchControl(true);
        Debug.Log("Control Switched");
    }
    
    public void TrialEndTrigger()
    {
        if (trialsDone.Value > maxTrials.Value)
        {
            CalibrationManager.Instance.TestDriveSuccessState(false, (int)trialsDone.Value);

        }
        else
        {
            CalibrationManager.Instance.TestDriveSuccessState(true, (int)trialsDone.Value);

        }
    }
    
    IEnumerator PassControl()
    {
        yield return new WaitForSecondsRealtime(timeForControl);
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
        yield return new WaitForSecondsRealtime(timeForControl);
        ControlEnded();
    }

    private void ControlEnded()
    {
        if (sceneStart)
        {
            sceneStart = false;
            
            ResetCar(_participantCar);
            ActivateEvent(delayTillHudActivates);
        }
    }

    private void ActivateEvent(float delay)
    {
        foreach (var activateObjects in eventObjects)
        {
            activateObjects.SetActive(true);
        }
        
        //yield return new WaitForSecondsRealtime(delay);
        advanced_HUD.AIDrive();
        //windscreenHUD.gameObject.SetActive(true);
        //hudLite.gameObject.SetActive(true);
    }
    
    public IEnumerator DeactivateEvent(float delay)
    {
        yield return new WaitForSecondsRealtime(delay);
        foreach (var activateObjects in eventObjects)
        {
            activateObjects.SetActive(false);
        }
    }

    private void ResetCar(GameObject objectToReset)
    {
        if (objectToReset.GetComponent<CarController>())
        {
            objectToReset.SetActive(false);
            objectToReset.transform.SetPositionAndRotation(respawnPoint.gameObject.transform.position, respawnPoint.gameObject.transform.rotation);
            objectToReset.SetActive(true);
        }
    }

    public IEnumerator ActivateHUD()
    {
        advanced_HUD.ManualDrive();
        advanced_HUD.DriverAlert();
       //windscreenHUD.DriverAlert();
       //hudLite.DriverAlert();
       yield return new WaitForSecondsRealtime(timeBeforeObjectsGetMarked);
       advanced_HUD.ActivateHUD(eventObjectsToMark);
       //hudLite.ActivateHUD(eventObjectsToMark);
    }

    public void DeactivateHUD()
    {
        advanced_HUD.AIDrive();
        advanced_HUD.DeactivateHUD();
        //windscreenHUD.DeactivateHUD();
        //hudLite.DeactivateHUD();
    }
}
