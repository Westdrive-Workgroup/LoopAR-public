using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using PathCreation;
using UnityEngine;
using Valve.VR;
using UnityEngine.SceneManagement;
using UnityStandardAssets.Utility;

[DisallowMultipleComponent]
public class ExperimentManager : MonoBehaviour
{
    #region Fields

    public static ExperimentManager Instance { get; private set; }

    [Space] [Header("Necessary Elements")]
    [SerializeField] private GameObject participantsCar;
    [Tooltip("0 to 10 seconds")] [Range(0, 10)] [SerializeField] private float respawnDelay;

    private enum Scene
    {
        MainMenu,
        Experiment,
        EndOfExperiment
    }
    
    private SavingManager _savingManager;
    private List<ActivationTrigger> _activationTriggers;
    private CriticalEventController _criticalEventController;
    private Vector3 _respawnPosition;
    private Quaternion _respawnRotation;
    private Scene _scene;
    private bool _activatedEvent;
    private bool _vRScene;

    #endregion

    #region Private Methods
    
    private void Awake()
    {
        _activationTriggers = new List<ActivationTrigger>();
        
        //singleton pattern a la Unity
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
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
    
    private void Start()
    {
        _vRScene = CalibrationManager.Instance.GetVRActivationState();
        
        if (_activationTriggers.Count == 0)
        {
            Debug.Log("<color=red>Error: </color>Please ensure that ActivationTrigger is being executed before ExperimentManager if there are triggers present in the scene.");
        }

        if (EyetrackingManager.Instance == null)
        {
            Debug.Log("<color=red>Error: </color>EyetrackingManager should be present in the scene.");
        }
        
        if (CalibrationManager.Instance == null)
        {
            Debug.Log("<color=red>Error: </color>CalibrationManager should be present in the scene.");
        }
        
        if (SavingManager.Instance == null)
        {
            Debug.Log("<color=red>Error: </color>SavingManager should be present in the scene.");
        }
        
        if (CameraManager.Instance == null)
        {
            Debug.Log("<color=red>Error: </color>CameraManager should be present in the scene.");
        }
        
        InformTriggers();
        RunMainMenu();
    }
    
    // main menu
    private void RunMainMenu()
    { 
        _scene = Scene.MainMenu;
        CameraManager.Instance.FadeOut();
        CameraManager.Instance.SetObjectToFollow(participantsCar);
        CameraManager.Instance.SetSeatPosition(participantsCar.GetComponent<CarController>().GetSeatPosition());
        // participantsCar.transform.parent.gameObject.SetActive(false);
        participantsCar.GetComponent<CarController>().TurnOffEngine();
    }
    
    // inform all triggers to disable their game objects at the beginning of the experiment
    private void InformTriggers()
    {
        foreach (var trigger in _activationTriggers)
        {
            trigger.DeactivateTheGameObjects();
        }
    }
    
    // starting the experiment
    private void StartExperiment()
    {
        Debug.Log("start exp");
        _scene = Scene.Experiment;

        SavingManager.Instance.StartRecordingData();

        CameraManager.Instance.FadeIn();
        // participantsCar.transform.parent.gameObject.SetActive(true);
        participantsCar.GetComponent<CarController>().TurnOnEngine();
    }
    
    private IEnumerator ReSpawnParticipant(float seconds)
    {
        participantsCar.GetComponent<Rigidbody>().velocity = Vector3.zero;
        participantsCar.GetComponent<Rigidbody>().isKinematic = true;
        yield return new WaitForSeconds(seconds);
        participantsCar.GetComponent<Rigidbody>().isKinematic = false;
        participantsCar.GetComponentInChildren<HUD_Advance>().DeactivateHUD();
        CameraManager.Instance.AlphaFadeIn();
        participantsCar.GetComponent<CarController>().TurnOnEngine();
    }
    
    #endregion

    #region Public Methods

    public void ParticipantFailed()
    {
        _activatedEvent = false;

        CameraManager.Instance.AlphaFadeOut();
        PersistentTrafficEventManager.Instance.FinalizeEvent();
        participantsCar.GetComponent<CarController>().TurnOffEngine();
        participantsCar.GetComponent<Rigidbody>().isKinematic = true;
        participantsCar.GetComponent<Rigidbody>().velocity = Vector3.zero;
        participantsCar.transform.SetPositionAndRotation(_respawnPosition, _respawnRotation);
        CameraManager.Instance.ReSpawnBehavior();
        participantsCar.GetComponent<Rigidbody>().isKinematic = false;
        participantsCar.GetComponent<AIController>().SetLocalTargetAndCurveDetection();
        StartCoroutine(ReSpawnParticipant(respawnDelay));
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

        CameraManager.Instance.FadeOut();
        _scene = Scene.MainMenu;

        participantsCar.transform.parent.gameObject.SetActive(false);
        SceneManager.LoadSceneAsync("MainMenu");
    }
    
    // Reception desk for ActivationTriggers to register themselves
    public void RegisterToExperimentManager(ActivationTrigger listener)
    {
        _activationTriggers.Add(listener);
    }

    #endregion
    
    #region Setters

    public void SetRespawnPositionAndRotation(Vector3 position, Quaternion rotation)
    {
        _respawnPosition = position;
        _respawnRotation = rotation;
    }
    
    public void SetInitialTransform(Vector3 position, Quaternion rotation)
    {
        participantsCar.transform.SetPositionAndRotation(position, rotation);
    }
    
    public void SetInitialTransform(Vector3 position)
    {
        participantsCar.transform.SetPositionAndRotation(position, participantsCar.transform.rotation);
    }

    public void SetCarPath(PathCreator newPath)
    {
        participantsCar.GetComponent<AIController>().SetNewPath(newPath);
    }

    public void SetEventActivationState(bool activationState)
    {
        _activatedEvent = activationState;
    }

    #endregion

    #region Getters

    public bool GetEventActivationState()
    {
        return _activatedEvent;
    }

    public GameObject GetSeatPosition()
    {
        return participantsCar.GetComponent<CarController>().GetSeatPosition();
    }

    public GameObject GetParticipantsCar()
    {
        return participantsCar;
    } 

    #endregion
    
    #region GUI

    public void OnGUI()
    {
        float height = Screen.height;
        float width = Screen.width;
        
        float xForButtons = width / 12f;
        float yForButtons = height / 7f;
        
        float xForLable = (width / 12f);
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
        
            if (GUI.Button(new Rect(xForButtons*9, yForButtons, buttonWidth, buttonHeight), "Abort"))
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
                SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().name);
                _scene = Scene.MainMenu;
            }

            if (_activatedEvent)
            {
                GUI.backgroundColor = Color.magenta;

                if (GUI.Button(new Rect(xForButtons, yForButtons, buttonWidth, buttonHeight), "Respawn Manually"))
                {
                    ParticipantFailed();
                }
            }
        }
    }

    #endregion
}