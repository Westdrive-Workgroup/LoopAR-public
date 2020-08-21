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
    private GameObject _participantsCar;
    [Tooltip("0 to 10 seconds")] [Range(0, 10)] [SerializeField] private float startExperimentDelay = 3f;
    [Tooltip("0 to 10 seconds")] [Range(0, 10)] [SerializeField] private float respawnDelay = 5f;

    private enum Scene
    {
        MainMenu,
        Experiment,
        EndOfExperiment
    }
    
    private List<ActivationTrigger> _activationTriggers;
    private CriticalEventController _criticalEventController;
    private Vector3 _respawnPosition;
    private Quaternion _respawnRotation;
    private Scene _scene;
    private bool _activatedEvent;
    private bool _vRScene;
    private bool _isStartPressed;
    

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
            SavingManager.Instance.SetParticipantCar(_participantsCar);    
        }
    }

    public void OnSceneLoaded()
    {
        if (SceneManager.GetActiveScene().name != "MainMenu")
        {
            AssignParticipantsCar();
            RunMainMenu();
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
        
        try
        {
            InformTriggers();
            AssignParticipantsCar();
            RunMainMenu();
        }
        catch (Exception e)
        {
            Console.WriteLine("Error: " + e);
            throw;
        }
    }

    private void RunMainMenu()
    {
        _scene = Scene.MainMenu;
        _participantsCar.GetComponent<Rigidbody>().useGravity = false;
        _participantsCar.GetComponent<CarController>().TurnOffEngine();
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
    private IEnumerator StartExperiment()
    {
        TimeManager.Instance.SetExperimentStartTime();
        _isStartPressed = true;
        while (SceneLoadingHandler.Instance.GetAdditiveLoadingState()) yield return null;
        
        _scene = Scene.Experiment;

        SavingManager.Instance.StartRecordingData();
        CameraManager.Instance.FadeIn();
        yield return new WaitForSeconds(startExperimentDelay);
        _participantsCar.GetComponent<Rigidbody>().useGravity = true;
        _participantsCar.GetComponent<CarController>().TurnOnEngine();
    }
    
    private IEnumerator ReSpawnParticipant(float seconds)
    {
        _participantsCar.GetComponent<Rigidbody>().velocity = Vector3.zero;
        _participantsCar.GetComponent<Rigidbody>().isKinematic = true;
        yield return new WaitForSeconds(seconds);
        _participantsCar.GetComponent<Rigidbody>().isKinematic = false;
        _participantsCar.GetComponentInChildren<HUD_Advance>().DeactivateHUD(false);
        CameraManager.Instance.AlphaFadeIn();
        _participantsCar.GetComponent<CarController>().TurnOnEngine();
    }
    
    private void AssignParticipantsCar()
    {
        switch (SceneManager.GetActiveScene().name)
        {
            case "SceneLoader":
                _participantsCar = SceneLoadingSceneManager.Instance.GetParticipantsCar();
                break;
            case "MountainRoad":
                _participantsCar = MountainRoadManager.Instance.GetParticipantsCar();
                break;
            case "Westbrueck":
                _participantsCar = WestbrueckManager.Instance.GetParticipantsCar();
                break;
            case "CountryRoad":
                _participantsCar = CountryRoadManager.Instance.GetParticipantsCar();
                break;
            case "Autobahn":
                _participantsCar = AutobahnManager.Instance.GetParticipantsCar();
                break;
        }
        
        PersistentTrafficEventManager.Instance.SetParticipantsCar(_participantsCar);
    }
    
    #endregion

    #region Public Methods

    public void ParticipantFailed()
    {
        _activatedEvent = false;

        CameraManager.Instance.AlphaFadeOut();
        _participantsCar.GetComponentInChildren<HUD_Advance>().DeactivateHUD(true);
        PersistentTrafficEventManager.Instance.FinalizeEvent();
        _participantsCar.GetComponent<CarController>().TurnOffEngine();
        _participantsCar.GetComponent<Rigidbody>().isKinematic = true;
        _participantsCar.GetComponent<Rigidbody>().velocity = Vector3.zero;
        _participantsCar.transform.SetPositionAndRotation(_respawnPosition, _respawnRotation);
        CameraManager.Instance.RespawnBehavior();
        _participantsCar.GetComponent<Rigidbody>().isKinematic = false;
        _participantsCar.GetComponent<AIController>().SetLocalTargetAndCurveDetection();
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

        _participantsCar.transform.parent.gameObject.SetActive(false);
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
        _participantsCar.transform.SetPositionAndRotation(position, rotation);
    }
    
    public void SetInitialTransform(Vector3 position)
    {
        _participantsCar.transform.SetPositionAndRotation(position, _participantsCar.transform.rotation);
    }

    public void SetCarPath(PathCreator newPath)
    {
        _participantsCar.GetComponent<AIController>().SetNewPath(newPath);
    }

    public void SetEventActivationState(bool activationState)
    {
        _activatedEvent = activationState;
    }
    
    public void SetParticipantsCar(GameObject car)
    {
        _participantsCar = car;
    }
    
    public void SetController(CriticalEventController criticalEventController)
    {
        _criticalEventController = criticalEventController;
    }

    #endregion

    #region Getters

    public bool GetEventActivationState()
    {
        return _activatedEvent;
    }

    public GameObject GetSeatPosition()
    {
        return _participantsCar.GetComponent<CarController>().GetSeatPosition();
    }

    public GameObject GetParticipantsCar()
    {
        return _participantsCar;
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
        float yForLable = height / 1.35f;

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
            if (!_isStartPressed)
            {
                GUI.Label(new Rect(xForLable, yForLable, 500, 100),  "Main Experiment");

                if (GUI.Button(new Rect(xForButtons, yForButtons, buttonWidth, buttonHeight), "Start"))
                {
                    StartCoroutine(StartExperiment());
                }
            }

            if (_isStartPressed)
            {
                GUI.Label(new Rect(width / 4f, height / 2f, 500, 100),  "Main Experiment is Loading...");
            }
            
            // Reset Button
            GUI.backgroundColor = Color.red;
            GUI.color = Color.white;
        
            if (GUI.Button(new Rect(xForButtons*9, yForButtons, buttonWidth, buttonHeight), "Abort"))
            {
                SavingManager.Instance.StopAndSaveData(SceneManager.GetActiveScene().name);
                CalibrationManager.Instance.AbortExperiment();
            }
        } 
        else if (_scene == Scene.Experiment)
        {
            // GUI.backgroundColor = Color.red;
            GUI.color = Color.white;
            
            /*if (GUI.Button(new Rect(xForButtons*9, yForButtons, buttonWidth, buttonHeight), "End"))
            {
                SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().name);
                _scene = Scene.MainMenu;
            }*/

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