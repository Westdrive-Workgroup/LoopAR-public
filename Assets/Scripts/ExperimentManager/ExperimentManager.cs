using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using Valve.VR;
using UnityEngine.SceneManagement;
using UnityStandardAssets.Utility;

[DisallowMultipleComponent]
public class ExperimentManager : MonoBehaviour
{
    public static ExperimentManager Instance { get; private set; }
    
    private bool _vRScene;
    
    [Space] [Header("Necessary Elements")]
    [SerializeField] private GameObject participantsCar;
    [Tooltip("0 to 10 seconds")] [Range(0, 10)] [SerializeField] private float respawnDelay;

    // [Space] [Header("Cameras and accessories")]
    // [SerializeField] private VRCam vRCamera;
    // [SerializeField] private Camera firstPersonCamera;
    /*[Space] [Header("Temporarily-Debug")]*/
    // [SerializeField] private GameObject blackScreen;
    
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
        // CalibrationManager.Instance.SetCameraMode(_vRScene); // todo
        CameraManager.Instance.FadeOut();
        CameraManager.Instance.SetObjectToFollow(participantsCar);
        CameraManager.Instance.SetSeatPosition(participantsCar.GetComponent<CarController>().GetSeatPosition());
        participantsCar.transform.parent.gameObject.SetActive(false);

        /*// todo remove
        // (cameras should be handled undependantly from this if condition
        /*if (_vRScene)
        {
            //
            // vRCamera.SetPosition(firstPersonCamera.transform.position);
        }
        else
        {
            //
            /*firstPersonCamera.enabled = true;
            blackScreen.SetActive(true);
            vRCamera.gameObject.SetActive(false);#2#
        }#1#*/
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


        /*// todo remove
        /*if (!_vRScene)
        {
            //
            /*firstPersonCamera.enabled = true;
            blackScreen.SetActive(false);#2#
        }
        else
        {
            //
            // vRCamera.Seat();
        }#1#*/
        
        SavingManager.Instance.StartRecordingData();

        CameraManager.Instance.FadeIn();
        Debug.Log("Here too");

        participantsCar.transform.parent.gameObject.SetActive(true);
    }
    
    private IEnumerator RespawnParticipant(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        participantsCar.transform.parent.gameObject.SetActive(true);        
        participantsCar.GetComponentInChildren<HUD_Advance>().DeactivateHUD();
        
        CameraManager.Instance.FadeIn();
        
        /*// todo remove
        //blackScreen.SetActive(false);*/
    }
    
    #endregion

    #region Public Methods

    public void ParticipantFailed()
    {
        _activatedEvent = false;
        
        /*// todo remove
        // blackScreen.SetActive(true);*/
        
        CameraManager.Instance.FadeOut();
        
        PersistentTrafficEventManager.Instance.FinalizeEvent();
        participantsCar.transform.parent.gameObject.SetActive(false);
        participantsCar.transform.SetPositionAndRotation(_respawnPosition, _respawnRotation);
        participantsCar.GetComponent<AIController>().SetLocalTargetAndCurveDetection();
        StartCoroutine(RespawnParticipant(respawnDelay));
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
        
        /*// todo remove
        // blackScreen.SetActive(true);
        
        CameraManager.Instance.FadeOut();
        
        if (!_vRScene)
        {
            // firstPersonCamera.enabled = false;
            _scene = Scene.MainMenu;
        }
        else
        {
            //
            // vRCamera.UnSeat();
        }*/
        
        CameraManager.Instance.FadeOut();
        _scene = Scene.MainMenu;

        participantsCar.transform.parent.gameObject.SetActive(false);
        SceneManager.LoadSceneAsync("MainMenu");
        // SceneLoader.Instance.AsyncLoad(0);
    }
    
    // Reception desk for ActivationTriggers to register themselves
    public void RegisterToExperimentManager(ActivationTrigger listener)
    {
        _activationTriggers.Add(listener);
    }
    
        #region Setters

        public void SetRespawnPositionAndRotation(Vector3 position, Quaternion rotation)
        {
            _respawnPosition = position;
            _respawnRotation = rotation;
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
                // SceneLoader.Instance.AsyncLoad(4);
                SceneManager.LoadSceneAsync("safe-mountainroad01");
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
