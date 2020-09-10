using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using UnityEngine.SceneManagement;

[DisallowMultipleComponent]
public class CameraManager : MonoBehaviour
{
    #region Fields

    public static CameraManager Instance { get; private set; }
    
    [SerializeField] private Camera mainCamera;
    [SerializeField] private GameObject blackScreen;
    [SerializeField] private GameObject calibrationOffset;
    private GameObject _objectToFollow;
    
    private GameObject _seatPosition;
    [SerializeField] private VRCam vRCamera;
    private SceneLoadingHandler _sceneLoadingHandler;

    #endregion

    #region PrivateMethods

    private void Awake()
    {
        _sceneLoadingHandler = SceneLoadingHandler.Instance;
        
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
    }
    
    public void OnSceneLoaded(bool autoFadeIn)
    {
        _objectToFollow = SceneLoadingHandler.Instance.GetParticipantsCar();
        _seatPosition = SceneLoadingHandler.Instance.GetSeatPosition();
        vRCamera = this.gameObject.GetComponent<VRCam>();
        
        if (CalibrationManager.Instance.GetVRActivationState())
        {
            vRCamera.SetSeatPosition(_seatPosition);
            vRCamera.Seat();
        }
        else
        {
            SetSeatPosition(_objectToFollow);
        }

        if (autoFadeIn)
        {
            StartCoroutine(FadeIntoTheScene());
        }
    }

    private void Start()
    {
        _objectToFollow = SceneLoadingHandler.Instance.GetParticipantsCar();
        _seatPosition = SceneLoadingHandler.Instance.GetSeatPosition();

        if (CalibrationManager.Instance.GetVRActivationState())
        {
            VRModeCameraSetUp();
        }
        else
        {
            NonVRModeCameraSetUp();
        }
        
        StartCoroutine(FadeIntoTheScene());
    }

    IEnumerator FadeIntoTheScene()
    {
        yield return new WaitForSeconds(1);
        FadeIn();
    }

    #endregion

    #region PublicMethods

    public void VRModeCameraSetUp()
    {
        this.gameObject.GetComponent<VRCam>().enabled = true;
        this.gameObject.GetComponent<ChaseCam>().enabled = false;
        blackScreen.gameObject.SetActive(false);
        vRCamera = this.gameObject.GetComponent<VRCam>();
        mainCamera.GetComponent<Camera>().stereoTargetEye = StereoTargetEyeMask.Both;

        // FadeOut();
        
        if (CalibrationManager.Instance != null)
        {
            SetOffset(CalibrationManager.Instance.GetSeatCalibrationOffsetPosition());
        }
        else
        {
            SetOffset(Vector3.zero);
            Debug.Log("<color=red>Error: </color>No Calibration Manager found, please add to the scene.");
        }
        
        if (_objectToFollow != null)
        {
            vRCamera.SetSeatPosition(_seatPosition);
            vRCamera.Seat();
        }
    }

    public void NonVRModeCameraSetUp()
    {
        this.gameObject.GetComponent<ChaseCam>().enabled = true;
        this.gameObject.GetComponent<VRCam>().enabled = false;
        mainCamera.GetComponent<Camera>().stereoTargetEye = StereoTargetEyeMask.None;
        mainCamera.GetComponent<Camera>().fieldOfView = 60f;
        
        // blackScreen.SetActive(true);
        if (_objectToFollow != null)
        {
            SetOffset(Vector3.zero);
            this.transform.position = GetSeatPositionVector3();
        }
    }
    
    public void FadeOut()
    {
        if (CalibrationManager.Instance.GetVRActivationState())
        {
            //set start color
            SteamVR_Fade.Start(Color.clear, 0f);
            //set and start fade to
            SteamVR_Fade.Start(Color.black, 0);
        }
        else
        {
            blackScreen.SetActive(true);
        }
    }

    public void FadeIn()
    {
        if (CalibrationManager.Instance.GetVRActivationState())
        {
            //set start color
            SteamVR_Fade.Start(Color.black, 0f);
            //set and start fade to
            SteamVR_Fade.Start(Color.clear, 0f);
        }
        else
        {
            blackScreen.SetActive(false);
        }
    }
    
    public void AlphaFadeIn()
    {
        _objectToFollow.GetComponent<CarWindows>().SetInsideWindowsAlphaChannel(0);
    }
    
    public void AlphaFadeOut()
    {
        _objectToFollow.GetComponent<CarWindows>().SetInsideWindowsAlphaChannel(1);
    }

    #region Setters

    public void SetObjectToFollow(GameObject theObject)
    {
        _objectToFollow = theObject;
    }

    public void SetSeatPosition(GameObject seat)
    {
        _seatPosition = seat;
    }
        
    public void SetOffset(Vector3 localOffset)
    {
        calibrationOffset.transform.localPosition = localOffset;
    }

    public void RespawnBehavior()
    {
        if (!CalibrationManager.Instance.GetVRActivationState())
        {
            this.gameObject.GetComponent<ChaseCam>().ForceChaseCamRotation();
        }
    }

    #endregion
    

    #region Getters
        
    private Vector3 GetSeatPositionVector3()
    {
        return _objectToFollow.GetComponent<CarController>().GetSeatPosition().transform.position;
    }

    public GameObject GetObjectToFollow()
    {
        return _objectToFollow;
    }

    public VRCam GetVRCamera()
    {
        return vRCamera;
    }

    public GameObject GetSeatPosition()
    {
        return _seatPosition;
    }
        

    public GameObject GetCalibrationOffset()
    {
        return calibrationOffset;
    }

    public Camera GetMainCamera()
    {
        return mainCamera;
    }

    #endregion
    
    
    #endregion
}