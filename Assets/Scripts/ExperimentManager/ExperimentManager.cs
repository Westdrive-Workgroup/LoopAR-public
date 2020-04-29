using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using Valve.VR;

[DisallowMultipleComponent]
public class ExperimentManager : MonoBehaviour
{
    public static ExperimentManager Instance { get; private set; }
    
    [Space] [Header("Necessary Elements")]
    [SerializeField] private GameObject participantsCar;
    [SerializeField] private Camera _camera;
    [SerializeField] private Camera firstPersonCamera;
    [Tooltip("0 to 10 seconds")] [Range(0, 10)] [SerializeField] private float respawnDelay;
    
    [Space] [Header("VR setup")]
    [SerializeField] private bool vRScene;
    [SerializeField] private VRCam vRCamera;

    [Space] [Header("Temporarily-Debug")]
    [SerializeField] private GameObject failurePatch;
    private enum Scene
    {
        MainMenu,
        Experiment,
        CountryRoad,
        MountainRoad,
        Autobahn,
        City,
        EndOfExperiment
    }
    
    private SavingManager _savingManager;
    private List<ActivationTrigger> _activationTriggers;
    private CriticalEventController _criticalEventController;
    private Vector3 _respawnPosition;
    private Quaternion _respawnRotation;
    private Scene _scene;
    private bool _activatedEvent;

    // registers in which scene or state the experiment is

    
    private void Awake()
    {
        _activationTriggers = new List<ActivationTrigger>();

        
        //singleton pattern a la Unity
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);         //the Traffic Manager should be persitant by changing the scenes maybe change it on the the fly
        }
        else
        {
            Destroy(gameObject);
        }

        if (SavingManager.Instance != null)
        {
            _savingManager = SavingManager.Instance;
            _savingManager.SetParticipantCar(participantsCar);    
        }
    }


    void Start()
    {
        vRScene = CalibrationManager.Instance.GetVRModeState();
        
        InformTriggers();
        
        if (_activationTriggers.Count == 0)
        {
            Debug.Log("<color=red>Error: </color>Please ensure that ActivationTrigger is being executed before ExperimentManager if there are triggers present in the scene.");
        }

        if (EyetrackingManager.Instance == null)
        {
            Debug.Log("<color=red>Error: </color>EyetrackingManager should be present in the scene.");
        }
        
        RunMainMenu();

        if (CalibrationManager.Instance == null)
        {
            Debug.Log("<color=red>Please start from MainMenu! </color>");
        }
    }
    

    // main menu
    private void RunMainMenu()
    { 
        _scene = Scene.MainMenu;
        
        if (vRScene)
        {
            vRCamera.SetPosition(_camera.transform.position);
        }
        else
        {
            firstPersonCamera.enabled = true;
            _camera.transform.position = Vector3.zero;
        }
        
        participantsCar.transform.parent.gameObject.SetActive(false);
        failurePatch.SetActive(false);
    }

    // inform all triggers to disable their gameobjects at the beginning of the experiment

    private void InformTriggers()
    {
        foreach (var trigger in _activationTriggers)
        {
            trigger.DeactivateTheGameObjects();
        }
    }


    // Reception desk for ActivationTriggers to register themselves

    public void RegisterToExperimentManager(ActivationTrigger listener)
    {
        _activationTriggers.Add(listener);
    }


    // starting the experiment
    private void StartExperiment()
    {
        _scene = Scene.Experiment;
        _camera.enabled = false;
        
        if (!vRScene)
        {
            firstPersonCamera.enabled = true;
        }
        else
        {
            Debug.Log("vr ");
            vRCamera.Seat();
        }
        
        participantsCar.transform.parent.gameObject.SetActive(true);
        
        if (SavingManager.Instance != null)
        {
            SavingManager.Instance.StartRecordingData();
        }
    }

    // ending the experiment
    public void EndTheExperiment()
    {
        _scene = Scene.EndOfExperiment;
        if (SavingManager.Instance != null)
        {
            SavingManager.Instance.StopRecordingData();
            SavingManager.Instance.SaveData();
        }
        
        if (!vRScene)
        {
            firstPersonCamera.enabled = false;
            _camera.enabled = true;
            _scene = Scene.MainMenu;
        }
        else
        {
            vRCamera.UnSeat();
        }
        _camera.enabled=true;
        participantsCar.transform.parent.gameObject.SetActive(false);
        SceneLoader.Instance.AsyncLoad(0);
    }

    
    public void ParticipantFailed()
    {
        // todo fade to black in VR
        failurePatch.SetActive(true);
        PersistentTrafficEventManager.Instance.FinalizeEvent();
        participantsCar.transform.parent.gameObject.SetActive(false);
        StartCoroutine(RespawnParticipant(respawnDelay));
        _activatedEvent = false;
    }

    IEnumerator RespawnParticipant(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        participantsCar.transform.SetPositionAndRotation(_respawnPosition, _respawnRotation);
        participantsCar.GetComponent<AIController>().SetLocalTarget();
        participantsCar.transform.parent.gameObject.SetActive(true);
        failurePatch.SetActive(false);
    }

    public void SetRespawnPositionAndRotation(Vector3 position, Quaternion rotation)
    {
        _respawnPosition = position;
        _respawnRotation = rotation;
    }

    public void SetEventActivationState(bool activationState)
    {
        _activatedEvent = activationState;
        Debug.Log("event activation state: " + _activatedEvent) ;
    }
    
    public GameObject GetParticipantCar()
    {
        return participantsCar;
    }

    public void OnGUI()
    {
        float height = Screen.height;
        float width = Screen.width;
        
        float xForButtons = width / 12f;
        float yForButtons = height / 7f;
        
        float xForLable = (width / 2f);
        float yForLable = height/1.35f;

        float buttonWidth = 200f;
        float buttonHeight = 30f;
        float heightDifference = 40f;
        
        int labelFontSize = 33;

        
        // Lable
        GUI.color = Color.white;
        GUI.skin.label.fontSize = labelFontSize;
        GUI.skin.label.fontStyle = FontStyle.Bold;
        
        // Buttons
        GUI.backgroundColor = Color.cyan;
        GUI.color = Color.white;
        
        if (_scene == Scene.MainMenu)
        {
            GUI.Label(new Rect(xForLable, yForLable, 500, 100),  "Main Experiment");

            if (GUI.Button(new Rect(xForButtons, yForButtons, buttonWidth, buttonHeight), "Start"))
            {
                StartExperiment();
            }
            
            // Reset Button
            GUI.backgroundColor = Color.red;
            GUI.color = Color.white;
        
            if (GUI.Button(new Rect(xForButtons, yForButtons + (heightDifference*9.5f), buttonWidth, buttonHeight), "Main Menu"))
            {
                CalibrationManager.Instance.AbortExperiment();
            }
        } 
        else if (_scene == Scene.Experiment)
        {
            GUI.backgroundColor = Color.red;
            GUI.color = Color.white;
            
            if (GUI.Button(new Rect(xForButtons*9, yForButtons, buttonWidth, buttonHeight), "End"))
            {
                SceneLoader.Instance.AsyncLoad(4);
                _scene = Scene.MainMenu;
            }

            if (_activatedEvent)
            {
                GUI.backgroundColor = Color.magenta;

                if (GUI.Button(new Rect(xForButtons, yForButtons, buttonWidth, buttonHeight), "Respawn Manualy"))
                {
                    ParticipantFailed();
                }
            }
        }
    }
}
